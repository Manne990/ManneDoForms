using System;
using Android.Util;
using Android.Views.Animations;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Java.Lang.Ref;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class PhotoViewDroidAttacher:Java.Lang.Object,IPhotoViewDroid,Android.Views.View.IOnTouchListener,IOnGestureListener,Android.Views.ViewTreeObserver.IOnGlobalLayoutListener 
	{
		public static readonly String LOG_TAG = "PhotoViewAttacher";

		// let debug flag be dynamic, but still Proguard can be used to remove from
		// release builds
		private readonly bool DEBUG = Log.IsLoggable(LOG_TAG,LogPriority.Debug);

		static readonly Android.Views.Animations.IInterpolator sInterpolator = new AccelerateDecelerateInterpolator();
		int ZOOM_DURATION = IPhotoViewDroidConstants.DEFAULT_ZOOM_DURATION;

		static readonly int EDGE_NONE = -1;
		static readonly int EDGE_LEFT = 0;
		static readonly int EDGE_RIGHT = 1;
		static readonly int EDGE_BOTH = 2;

		private float mMinScale = IPhotoViewDroidConstants.DEFAULT_MIN_SCALE;
		private float mMidScale = IPhotoViewDroidConstants.DEFAULT_MID_SCALE;
		private float mMaxScale = IPhotoViewDroidConstants.DEFAULT_MAX_SCALE;

		private bool mAllowParentInterceptOnEdge = true;


		private static void CheckZoomLevels(float minZoom, float midZoom,
			float maxZoom) {
			if (minZoom >= midZoom) {
				throw new Java.Lang.IllegalArgumentException(
					"MinZoom has to be less than MidZoom");
			} else if (midZoom >= maxZoom) {
				throw new Java.Lang.IllegalArgumentException(
					"MidZoom has to be less than MaxZoom");
			}
		}

		private static bool HasDrawable(ImageView imageView) {
			return null != imageView && null != imageView.Drawable;
		}

		private static bool IsSupportedScaleType( Android.Widget.ImageView.ScaleType scaleType) {
			if (null == scaleType) {
				return false;
			}
			if (scaleType.Name().Equals(Android.Widget.ImageView.ScaleType.Matrix.Name())) {
				throw new Java.Lang.IllegalArgumentException(scaleType.Name()
					+ " is not supported in PhotoView");
			}
			else {
				return true;
			}
		}

		private static void SetImageViewScaleTypeMatrix(ImageView imageView) {
			if (null != imageView && !(imageView is IPhotoViewDroid)) {
				if (!Android.Widget.ImageView.ScaleType.Matrix.Name().Equals(imageView.GetScaleType().Name())) {
					imageView.SetScaleType(Android.Widget.ImageView.ScaleType.Matrix);
				}
			}
		}

		private Java.Lang.Ref.WeakReference mImageView;
		// Gesture Detectors
		private GestureDetector mGestureDetector;
		private IGestureDetector mScaleDragDetector;

		// These are set so we don't keep allocating them on the heap
		private readonly Matrix mBaseMatrix = new Matrix();
		private readonly Matrix mDrawMatrix = new Matrix();
		private readonly Matrix mSuppMatrix = new Matrix();
		private readonly RectF mDisplayRect = new RectF();
		private readonly float[] mMatrixValues = new float[9];

		// Listeners
		private IOnMatrixChangedListener mMatrixChangeListener;
		private IOnPhotoTapListener mPhotoTapListener;
		private IOnViewTapListener mViewTapListener;
		private Android.Views.View.IOnLongClickListener mLongClickListener;

		private int mIvTop, mIvRight, mIvBottom, mIvLeft;
		private FlingRunnable mCurrentFlingRunnable;
		private int mScrollEdge = EDGE_BOTH;
		private bool mZoomEnabled;
		private Android.Widget.ImageView.ScaleType mScaleType = Android.Widget.ImageView.ScaleType.FitCenter;

		public PhotoViewDroidAttacher(ImageView imageView) {
			mImageView = new Java.Lang.Ref.WeakReference(imageView);
			imageView.DrawingCacheEnabled = true;
			imageView.SetOnTouchListener (this);
			ViewTreeObserver observer = imageView.ViewTreeObserver;
			if (null != observer)
				observer.AddOnGlobalLayoutListener(this);

			// Make sure we using MATRIX Scale Type
			SetImageViewScaleTypeMatrix (imageView);

			if (imageView.IsInEditMode) {
				return;
			}

			// Create Gesture Detectors...
			mScaleDragDetector = VersionedGestureDetector.NewInstance(
				imageView.Context, this);
			mGestureDetector = new GestureDetector (imageView.Context, new MSimpleOnGestureListener (this));


			mGestureDetector.SetOnDoubleTapListener (new DefaultOnDoubleTapListener (this));

			SetZoomable (true);
		}
		public void Cleanup() {
			if (null == mImageView) {
				return; // cleanup already done
			}
			ImageView imageView = (ImageView)(((Reference)mImageView).Get());

			if (null != imageView) {
				// Remove this as a global layout listener
				ViewTreeObserver observer = imageView.ViewTreeObserver;
				if (null != observer && observer.IsAlive) {
					observer.RemoveGlobalOnLayoutListener(this);
				}

				// Remove the ImageView's reference to this
				imageView.SetOnTouchListener(null);

				// make sure a pending fling runnable won't be run
				CancelFling();
			}
			if (null != mGestureDetector) {
				mGestureDetector.SetOnDoubleTapListener(null);
			}
			// Clear listeners too
			mMatrixChangeListener = null;
			mPhotoTapListener = null;
			mViewTapListener = null;

			// Finally, clear ImageView
			mImageView = null;
		}


		#region LATER WORK
		public ImageView GetImageView () {
			ImageView imageView = null;

			if (null != mImageView) {
				imageView = (ImageView)mImageView.Get();
			}

			// If we don't have an ImageView, call cleanup()
			if (null == imageView) {
				Cleanup();
				Log.Info(LOG_TAG,
					"ImageView no longer exists. You should not use this PhotoViewAttacher any more.");
			}

			return imageView;
		}
		public void SetImageViewMatrix(Matrix matrix)
		{
			ImageView imageView = GetImageView();
			if (null != imageView) {

				CheckImageViewScaleType();
				imageView.ImageMatrix=(matrix);

				// Call MatrixChangedListener if needed
				if (null != mMatrixChangeListener) {
					RectF displayRect = GetDisplayRect(matrix);
					if (null != displayRect) {
						mMatrixChangeListener.OnMatrixChanged(displayRect);
					}
				}
			}
		}
		public  Matrix GetDrawMatrix()
		{
			mDrawMatrix.Set(mBaseMatrix);
			mDrawMatrix.PostConcat(mSuppMatrix);
			return mDrawMatrix;
		}
		private void CancelFling() {
			if (null != mCurrentFlingRunnable) {
				mCurrentFlingRunnable.CancelFling();
				mCurrentFlingRunnable = null;
			}
		}
		private bool CheckMatrixBounds()
		{
			ImageView imageView = GetImageView();
			if (null == imageView) {
				return false;
			}

			RectF rect = GetDisplayRect(GetDrawMatrix());
			if (null == rect) {
				return false;
			}
			float height = rect.Height(), width = rect.Width();
			float deltaX = 0, deltaY = 0;

			int viewHeight = GetImageViewHeight(imageView);

			if (height <= viewHeight) 
			{
				if (mScaleType.Name ().Equals (ImageView.ScaleType.FitStart.Name ())) {
					deltaY = -rect.Top;
				} 
				else if (ImageView.ScaleType.FitEnd.Equals (mScaleType.Name ())) {
					deltaY = viewHeight - height - rect.Top;
				}
				else{
					deltaY = (viewHeight - height) / 2 - rect.Top;
				}

			}
			else if (rect.Top > 0) {
				deltaY = -rect.Top;
			} else if (rect.Bottom < viewHeight) {
				deltaY = viewHeight - rect.Bottom;
			}

			int viewWidth = GetImageViewWidth(imageView);
			if (width <= viewWidth) {
				if (mScaleType.Name().Equals(ImageView.ScaleType.FitStart.Name())) {
					deltaX = -rect.Left;
				}
				else if (mScaleType.Name().Equals(ImageView.ScaleType.FitEnd.Name())) {
					deltaX = viewWidth - width - rect.Left;
				} 
				else {
					deltaX = (viewWidth - width) / 2 - rect.Left;
				}
				mScrollEdge = EDGE_BOTH;
			}
			else if (rect.Left > 0) {
				mScrollEdge = EDGE_LEFT;
				deltaX = -rect.Left;
			} else if (rect.Right < viewWidth) {
				deltaX = viewWidth - rect.Right;
				mScrollEdge = EDGE_RIGHT;
			} else {
				mScrollEdge = EDGE_NONE;
			}
			// Finally actually translate the matrix
			mSuppMatrix.PostTranslate(deltaX, deltaY);
			return true;
		}

		private RectF GetDisplayRect(Matrix matrix) {
			ImageView imageView = GetImageView();

			if (null != imageView) {
				Android.Graphics.Drawables.Drawable d = imageView.Drawable;
				if (null != d) {
					mDisplayRect.Set(0, 0, d.IntrinsicWidth,
						d.IntrinsicHeight);
					matrix.MapRect(mDisplayRect);
					return mDisplayRect;
				}
			}
			return null;
		}

		private void CheckAndDisplayMatrix()
		{
			if (CheckMatrixBounds()) {
				SetImageViewMatrix(GetDrawMatrix());
			}
		}
		private float GetValue(Matrix matrix, int whichValue) 
		{
			matrix.GetValues(mMatrixValues);
			return mMatrixValues[whichValue];
		}
		private int GetImageViewWidth(ImageView imageView) {
			if (null == imageView)
				return 0;
			return imageView.Width - imageView.PaddingLeft - imageView.PaddingRight;
		}

		private int GetImageViewHeight(ImageView imageView) {
			if (null == imageView)
				return 0;
			return imageView.Height - imageView.PaddingTop - imageView.PaddingBottom;
		}
		private void UpdateBaseMatrix(Android.Graphics.Drawables.Drawable d)
		{
			ImageView imageView = GetImageView ();
			if (null == imageView || null == d) {
				return;
			}

			float viewWidth = GetImageViewWidth (imageView);
			float viewHeight = GetImageViewHeight (imageView);
			int drawableWidth = d.IntrinsicWidth;
			int drawableHeight = d.IntrinsicHeight;

			mBaseMatrix.Reset ();


			float widthScale = viewWidth / drawableWidth;
			float heightScale = viewHeight / drawableHeight;

			if (mScaleType == Android.Widget.ImageView.ScaleType.Center) {
				mBaseMatrix.PostTranslate ((viewWidth - drawableWidth) / 2F,
					(viewHeight - drawableHeight) / 2F);

			} else if (mScaleType == Android.Widget.ImageView.ScaleType.CenterCrop) {
				float scale = Math.Max (widthScale, heightScale);
				mBaseMatrix.PostScale (scale, scale);
				mBaseMatrix.PostTranslate ((viewWidth - drawableWidth * scale) / 2F,
					(viewHeight - drawableHeight * scale) / 2F);
			} else if (mScaleType == Android.Widget.ImageView.ScaleType.CenterInside) {
				float scale = Math.Min (1.0f, Math.Min (widthScale, heightScale));
				mBaseMatrix.PostScale (scale, scale);
				mBaseMatrix.PostTranslate ((viewWidth - drawableWidth * scale) / 2F,
					(viewHeight - drawableHeight * scale) / 2F);

			} else {
				RectF mTempSrc = new RectF (0, 0, drawableWidth, drawableHeight);
				RectF mTempDst = new RectF (0, 0, viewWidth, viewHeight);

				if (mScaleType.Name ().Equals (ImageView.ScaleType.FitCenter.Name ())) {
					mBaseMatrix.SetRectToRect (mTempSrc, mTempDst, Matrix.ScaleToFit.Center);
				} else if (mScaleType.Name ().Equals (ImageView.ScaleType.FitStart.Name ())) {
					mBaseMatrix.SetRectToRect (mTempSrc, mTempDst, Matrix.ScaleToFit.Start);
				} else if (mScaleType.Name().Equals (ImageView.ScaleType.FitEnd.Name()))
					mBaseMatrix.SetRectToRect (mTempSrc, mTempDst, Matrix.ScaleToFit.End);
				else if (mScaleType.Name ().Equals (ImageView.ScaleType.FitXy.Name ()))
					mBaseMatrix.SetRectToRect (mTempSrc, mTempDst, Matrix.ScaleToFit.Fill);
			}
			ResetMatrix ();
		}

		private void ResetMatrix() 
		{
			mSuppMatrix.Reset();
			SetImageViewMatrix(GetDrawMatrix());
			CheckMatrixBounds();
		}
		public void Update() {
			ImageView imageView = GetImageView();

			if (null != imageView) {
				if (mZoomEnabled) {
					// Make sure we using MATRIX Scale Type
					SetImageViewScaleTypeMatrix(imageView);

					// Update the base matrix using the current drawable
					UpdateBaseMatrix(imageView.Drawable);
				} else {
					// Reset the Matrix...
					ResetMatrix();
				}
			}
		}

		private void CheckImageViewScaleType() {
			ImageView imageView = GetImageView();

			/**
         * PhotoView's getScaleType() will just divert to this.getScaleType() so
         * only call if we're not attached to a PhotoView.
         */
			if (null != imageView && !(imageView is IPhotoViewDroid)) {
				if (!Android.Widget.ImageView.ScaleType.Matrix.Name().Equals(imageView.GetScaleType().Name())) {
					throw new Java.Lang.IllegalStateException(
						"The ImageView's ScaleType has been changed since attaching a PhotoViewAttacher");
				}
			}
		}
		#endregion
		#region IPhotoView implementation

		public bool CanZoom ()
		{
			return mZoomEnabled;
		}

		public RectF GetDisplayRect ()
		{
			CheckMatrixBounds();
			return GetDisplayRect(GetDrawMatrix());
		}

		public bool SetDisplayMatrix (Matrix finalMatrix)
		{
			if (finalMatrix == null)
				throw new Java.Lang.IllegalArgumentException("Matrix cannot be null");

			ImageView imageView = GetImageView();
			if (null == imageView)
				return false;

			if (null == imageView.Drawable)
				return false;

			mSuppMatrix.Set(finalMatrix);
			SetImageViewMatrix(GetDrawMatrix());
			CheckMatrixBounds();

			return true;
		}

		public Matrix GetDisplayMatrix ()
		{
			return new Matrix(GetDrawMatrix());
		}

		public float GetMinScale ()
		{
			return GetMinimumScale();
		}

		public float GetMinimumScale ()
		{
			return mMinScale;
		}

		public float GetMidScale ()
		{
			return GetMediumScale();
		}

		public float GetMediumScale ()
		{
			return mMidScale;
		}

		public float GetMaxScale ()
		{
			return GetMaximumScale();
		}

		public float GetMaximumScale ()
		{
			return mMaxScale;
		}

		public float GetScale ()
		{
			return FloatMath.Sqrt((float) Math.Pow(GetValue(mSuppMatrix, Matrix.MscaleX), 2) + (float) Math.Pow(GetValue(mSuppMatrix, Matrix.MskewY), 2));
		}

		public ImageView.ScaleType GetScaleType ()
		{
			return mScaleType;
		}

		public void SetAllowParentInterceptOnEdge (bool allow)
		{
			mAllowParentInterceptOnEdge = allow;
		}

		public void SetMinScale (float minScale)
		{
			SetMinimumScale(minScale);
		}

		public void SetMinimumScale (float minimumScale)
		{
			CheckZoomLevels(minimumScale, mMidScale, mMaxScale);
			mMinScale = minimumScale;
		}

		public void SetMidScale (float midScale)
		{
			SetMediumScale(midScale);
		}

		public void SetMediumScale (float mediumScale)
		{
			CheckZoomLevels(mMinScale, mediumScale, mMaxScale);
			mMidScale = mediumScale;
		}

		public void SetMaxScale (float maxScale)
		{
			SetMaximumScale(maxScale);
		}

		public void SetMaximumScale (float maximumScale)
		{
			CheckZoomLevels(mMinScale, mMidScale, maximumScale);
			mMaxScale = maximumScale;
		}

		public void SetOnLongClickListener (View.IOnLongClickListener listener)
		{
			mLongClickListener = listener;
		}

		public void SetRotationTo (float degrees)
		{
			mSuppMatrix.SetRotate(degrees % 360);
			CheckAndDisplayMatrix();
		}

		public void SetRotationBy (float degrees)
		{
			mSuppMatrix.PostRotate(degrees % 360);
			CheckAndDisplayMatrix();
		}

		public void SetScale (float scale)
		{
			SetScale(scale, false);
		}

		public void SetScale (float scale, bool animate)
		{
			ImageView imageView = GetImageView();

			if (null != imageView) {
				SetScale(scale,
					(imageView.Right) / 2,
					(imageView.Bottom) / 2,
					animate);
			}
		}

		public void SetScale (float scale, float focalX, float focalY, bool animate)
		{
			ImageView imageView = GetImageView();

			if (null != imageView) {
				// Check to see if the scale is within bounds
				if (scale < mMinScale || scale > mMaxScale) {
					LogManager
						.GetLogger()
						.i(LOG_TAG,
							"Scale must be within the range of minScale and maxScale");
					return;
				}

				if (animate) {
					imageView.Post(new AnimatedZoomRunnable(this,GetScale(), scale,
						focalX, focalY));
				} else {
					mSuppMatrix.SetScale(scale, scale, focalX, focalY);
					CheckAndDisplayMatrix();
				}
			}
		}

		public void SetScaleType (ImageView.ScaleType scaleType)
		{
			if (IsSupportedScaleType(scaleType) && scaleType != mScaleType) {
				mScaleType = scaleType;

				// Finally update
				Update();
			}
		}

		public void SetZoomable (bool zoomable)
		{
			mZoomEnabled = zoomable;
			Update();
		}

		public void SetPhotoViewRotation (float degrees)
		{
			mSuppMatrix.SetRotate(degrees % 360);
			CheckAndDisplayMatrix();
		}

		public Bitmap GetVisibleRectangleBitmap ()
		{
			ImageView imageView = GetImageView();
			return imageView == null ? null : imageView.DrawingCache;
		}

		public void SetZoomTransitionDuration (int milliseconds)
		{
			if (milliseconds < 0)
				milliseconds = IPhotoViewDroidConstants.DEFAULT_ZOOM_DURATION;
			this.ZOOM_DURATION = milliseconds;
		}

		public IPhotoViewDroid GetIPhotoViewImplementation ()
		{
			return this;
		}


		public void SetOnMatrixChangeListener (IOnMatrixChangedListener listener)
		{
			mMatrixChangeListener = listener;
		}

		public void SetOnPhotoTapListener (IOnPhotoTapListener listener)
		{
			mPhotoTapListener = listener;
		}

		public IOnPhotoTapListener GetOnPhotoTapListener ()
		{
			return mPhotoTapListener;
		}

		public void SetOnViewTapListener (IOnViewTapListener listener)
		{
			mViewTapListener = listener;
		}

		public IOnViewTapListener GetOnViewTapListener ()
		{
			return mViewTapListener;
		}

		public void SetOnDoubleTapListener (GestureDetector.IOnDoubleTapListener newOnDoubleTapListener)
		{
			if (newOnDoubleTapListener != null)
				this.mGestureDetector.SetOnDoubleTapListener(newOnDoubleTapListener);
			else
				this.mGestureDetector.SetOnDoubleTapListener(new DefaultOnDoubleTapListener(this));
		}
		#endregion

		public bool OnTouch (View v, MotionEvent ev)
		{
			bool handled = false;

			if (mZoomEnabled && HasDrawable((ImageView) v)) {
				IViewParent parent = v.Parent;
				switch (ev.Action) {
				case MotionEventActions.Down:
					// First, disable the Parent from intercepting the touch
					// event
					if (null != parent)
						parent.RequestDisallowInterceptTouchEvent(true);
					else
						Log.Info(LOG_TAG, "onTouch getParent() returned null");

					// If we're flinging, and the user presses down, cancel
					// fling
					CancelFling();
					break;

				case MotionEventActions.Cancel:
				case MotionEventActions.Up:
					// If the user has zoomed less than min scale, zoom back
					// to min scale
					if (GetScale() < mMinScale) {
						RectF rect = GetDisplayRect();
						if (null != rect) {
							v.Post(new AnimatedZoomRunnable(this,GetScale(), mMinScale,
								rect.CenterX(), rect.CenterY()));
							handled = true;
						}
					}
					break;
				}

				// Try the Scale/Drag detector
				if (null != mScaleDragDetector
					&& mScaleDragDetector.OnTouchEvent(ev)) {
					handled = true;
				}

				// Check to see if the user double tapped
				if (null != mGestureDetector && mGestureDetector.OnTouchEvent(ev)) {
					handled = true;
				}
			}

			return handled;
		}

		#region IOnGestureListener implementation

		public void OnDrag (float dx, float dy)
		{
			if (mScaleDragDetector.IsScaling()) {
				return; // Do not drag if we are already scaling
			}
			if (DEBUG) {
				LogManager.GetLogger().d(LOG_TAG,
					Java.Lang.String.Format("onDrag: dx: %.2f. dy: %.2f", dx, dy));
			}


			ImageView imageView = GetImageView();
			mSuppMatrix.PostTranslate(dx, dy);
			CheckAndDisplayMatrix();

			IViewParent parent = imageView.Parent;
			if (mAllowParentInterceptOnEdge && !mScaleDragDetector.IsScaling()) {
				if (mScrollEdge == EDGE_BOTH
					|| (mScrollEdge == EDGE_LEFT && dx >= 1f)
					|| (mScrollEdge == EDGE_RIGHT && dx <= -1f)) {
					if (null != parent)
						parent.RequestDisallowInterceptTouchEvent(false);
				}
			} else {
				if (null != parent) {
					parent.RequestDisallowInterceptTouchEvent(true);
				}
			}
		}

		public void OnFling (float startX, float startY, float velocityX, float velocityY)
		{
			if (DEBUG) {
				LogManager.GetLogger().d(
					LOG_TAG,
					"onFling. sX: " + startX + " sY: " + startY + " Vx: "
					+ velocityX + " Vy: " + velocityY);
			}
			ImageView imageView = GetImageView();
			mCurrentFlingRunnable = new FlingRunnable(this,imageView.Context);
			mCurrentFlingRunnable.Fling(GetImageViewWidth(imageView),
				GetImageViewHeight(imageView), (int) velocityX, (int) velocityY);
			imageView.Post(mCurrentFlingRunnable);
		}

		public void OnScale (float scaleFactor, float focusX, float focusY)
		{
			if (DEBUG) {
				LogManager.GetLogger().d(
					LOG_TAG,
					Java.Lang.String.Format("onScale: scale: %.2f. fX: %.2f. fY: %.2f",
						scaleFactor, focusX, focusY));
			}

			if (GetScale() < mMaxScale || scaleFactor < 1f) {
				mSuppMatrix.PostScale(scaleFactor, scaleFactor, focusX, focusY);
				CheckAndDisplayMatrix();
			}
		}

		#endregion

		#region IOnGlobalLayoutListener implementation
		public void OnGlobalLayout ()
		{
			ImageView imageView = GetImageView();

			if (null != imageView) {
				if (mZoomEnabled) {
					int top = imageView.Top;
					 int right = imageView.Right;
					 int bottom = imageView.Bottom;
					 int left = imageView.Left;

					/**
                 * We need to check whether the ImageView's bounds have changed.
                 * This would be easier if we targeted API 11+ as we could just use
                 * View.OnLayoutChangeListener. Instead we have to replicate the
                 * work, keeping track of the ImageView's bounds and then checking
                 * if the values change.
                 */
					if (top != mIvTop || bottom != mIvBottom || left != mIvLeft
						|| right != mIvRight) {
						// Update our base matrix, as the bounds have changed
						UpdateBaseMatrix(imageView.Drawable);

						// Update values as something has changed
						mIvTop = top;
						mIvRight = right;
						mIvBottom = bottom;
						mIvLeft = left;
					}
				} else {
					UpdateBaseMatrix(imageView.Drawable);
				}
			}
		}
		#endregion

		private class FlingRunnable:Java.Lang.Object,Java.Lang.IRunnable
		{
			private readonly ScrollerProxy mScroller;
			private int mCurrentX, mCurrentY;
			PhotoViewDroidAttacher photoViewAttacher;

			public FlingRunnable(PhotoViewDroidAttacher photoViewAttacher,Context context) {
				mScroller = ScrollerProxy.GetScroller(context);
				this.photoViewAttacher=photoViewAttacher;
			}

			public void CancelFling() {
				if (photoViewAttacher.DEBUG) {
					LogManager.GetLogger().d("PhotoViewAttacher", "Cancel Fling");
				}
				mScroller.ForceFinished(true);
			}

			public void Fling(int viewWidth, int viewHeight, int velocityX,
				int velocityY) {
				RectF rect = photoViewAttacher.GetDisplayRect();
				if (null == rect) {
					return;
				}

				int startX =(int) Math.Round(-rect.Left);
				int minX, maxX, minY, maxY;

				if (viewWidth < rect.Width()) {
					minX = 0;
					maxX =(int) Math.Round(rect.Width() - viewWidth);
				} else {
					minX = maxX = startX;
				}

				int startY =(int) Math.Round(-rect.Top);
				if (viewHeight < rect.Height()) {
					minY = 0;
					maxY =(int) Math.Round(rect.Height() - viewHeight);
				} else {
					minY = maxY = startY;
				}

				mCurrentX = startX;
				mCurrentY = startY;

				if (photoViewAttacher.DEBUG) {
					LogManager.GetLogger().d(
						"PhotoViewAttacher",
						"fling. StartX:" + startX + " StartY:" + startY
						+ " MaxX:" + maxX + " MaxY:" + maxY);
				}

				// If we actually can move, fling the scroller
				if (startX != maxX || startY != maxY) {
					mScroller.Fling(startX, startY, velocityX, velocityY, minX,
						maxX, minY, maxY, 0, 0);
				}
			}

			public void Run ()
			{
				if (mScroller.IsFinished()) {
					return; // remaining post that should not be handled
				}
				ImageView imageView =photoViewAttacher.GetImageView();

				if (null != imageView && mScroller.ComputeScrollOffset ()) {
					int newX = mScroller.GetCurrX();
					int newY = mScroller.GetCurrY();
					if (photoViewAttacher.DEBUG) {
						LogManager.GetLogger().d(
							LOG_TAG,
							"fling run(). CurrentX:" + mCurrentX + " CurrentY:"
							+ mCurrentY + " NewX:" + newX + " NewY:"
							+ newY);
					}
					photoViewAttacher.mSuppMatrix.PostTranslate(mCurrentX - newX, mCurrentY - newY);
					photoViewAttacher.SetImageViewMatrix (photoViewAttacher.GetDrawMatrix ());
					mCurrentX = newX;
					mCurrentY = newY;
					Compat.PostOnAnimation (imageView, this);
				}

			}
		}
		private class MSimpleOnGestureListener:GestureDetector.SimpleOnGestureListener
		{
			PhotoViewDroidAttacher photoViewAttacher;
			public MSimpleOnGestureListener(PhotoViewDroidAttacher photoViewAttacher)
			{
				this.photoViewAttacher=photoViewAttacher;
			}
			public override void OnLongPress(MotionEvent e)
			{
				if (null != photoViewAttacher.mLongClickListener) {
					photoViewAttacher.mLongClickListener.OnLongClick(photoViewAttacher.GetImageView());
				}
			}
		}
		private class AnimatedZoomRunnable :Java.Lang.Object, Java.Lang.IRunnable {
			private readonly float mFocalX, mFocalY;
			private readonly long mStartTime;
			private readonly float mZoomStart, mZoomEnd;
			PhotoViewDroidAttacher photoViewAttacher;
			public AnimatedZoomRunnable(PhotoViewDroidAttacher photoViewAttacher, float currentZoom,  float targetZoom,
				 float focalX,  float focalY) {
				this.photoViewAttacher=photoViewAttacher;
				mFocalX = focalX;
				mFocalY = focalY;
				mStartTime =Java.Lang.JavaSystem.CurrentTimeMillis();
				mZoomStart = currentZoom;
				mZoomEnd = targetZoom;
			}

			#region IRunnable implementation

			public void Run ()
			{
				ImageView imageView =photoViewAttacher.GetImageView();
				if (imageView == null) {
					return;
				}

				float t = Interpolate();
				float scale = mZoomStart + t * (mZoomEnd - mZoomStart);
				float deltaScale = scale /photoViewAttacher.GetScale();

				photoViewAttacher.mSuppMatrix.PostScale(deltaScale, deltaScale, mFocalX, mFocalY);
				photoViewAttacher.CheckAndDisplayMatrix();

				// We haven't hit our target scale yet, so post ourselves again
				if (t < 1f) {
					Compat.PostOnAnimation(imageView, this);
				}
			}

			#endregion

			private float Interpolate() {
				float t = 1f * (Java.Lang.JavaSystem.CurrentTimeMillis() - mStartTime) / photoViewAttacher.ZOOM_DURATION;
				t = Math.Min(1f, t);
				t = sInterpolator.GetInterpolation(t);
				return t;
			}
		}
		public interface IOnMatrixChangedListener {
			/**
         * Callback for when the Matrix displaying the Drawable has changed. This could be because
         * the View's bounds have changed, or the user has zoomed.
         *
         * @param rect - Rectangle displaying the Drawable's new bounds.
         */
			void OnMatrixChanged(RectF rect);
		}
		public interface IOnPhotoTapListener {

			/**
         * A callback to receive where the user taps on a photo. You will only receive a callback if
         * the user taps on the actual photo, tapping on 'whitespace' will be ignored.
         *
         * @param view - View the user tapped.
         * @param x    - where the user tapped from the of the Drawable, as percentage of the
         *             Drawable width.
         * @param y    - where the user tapped from the top of the Drawable, as percentage of the
         *             Drawable height.
         */
			void OnPhotoTap(View view, float x, float y);
		}
		public interface IOnViewTapListener {

			/**
         * A callback to receive where the user taps on a ImageView. You will receive a callback if
         * the user taps anywhere on the view, tapping on 'whitespace' will not be ignored.
         *
         * @param view - View the user tapped.
         * @param x    - where the user tapped from the left of the View.
         * @param y    - where the user tapped from the top of the View.
         */
			void OnViewTap(View view, float x, float y);
		}
	}
}

