using System;
using Android.Widget;
using Android.Content;
using Android.Util;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class PhotoViewDroid:ImageView,IPhotoViewDroid
	{
		private PhotoViewDroidAttacher mAttacher;

		private ScaleType mPendingScaleType;

		public PhotoViewDroid(Context context):base(context, null, 0) {
			base.SetScaleType(ScaleType.Matrix);
			mAttacher = new PhotoViewDroidAttacher(this);

			if (null != mPendingScaleType) {
				SetScaleType(mPendingScaleType);
				mPendingScaleType = null;
			}
		}

		public PhotoViewDroid(Context context, IAttributeSet attr) :base(context, attr, 0){
			base.SetScaleType(ScaleType.Matrix);
			mAttacher = new PhotoViewDroidAttacher(this);

			if (null != mPendingScaleType) {
				SetScaleType(mPendingScaleType);
				mPendingScaleType = null;
			}
		}

		public PhotoViewDroid(Context context, IAttributeSet attr, int defStyle):base(context, attr, defStyle) {

			base.SetScaleType(ScaleType.Matrix);
			mAttacher = new PhotoViewDroidAttacher(this);

			if (null != mPendingScaleType) {
				SetScaleType(mPendingScaleType);
				mPendingScaleType = null;
			}
		}

    public PhotoViewDroidAttacher GetPhotoViewDroidAttacher()
    {
      return mAttacher;
    }

    #region IPhotoView imp
    public bool CanZoom ()
		{
			return mAttacher.CanZoom();
		}
		public Android.Graphics.RectF GetDisplayRect ()
		{
			return mAttacher.GetDisplayRect();
		}
		public bool SetDisplayMatrix (Android.Graphics.Matrix finalMatrix)
		{
			return mAttacher.SetDisplayMatrix(finalMatrix);
		}
		public Android.Graphics.Matrix GetDisplayMatrix ()
		{
			return mAttacher.GetDrawMatrix();
		}
		public float GetMinScale ()
		{
			return GetMinimumScale();
		}
		public float GetMinimumScale ()
		{
			return mAttacher.GetMinimumScale();
		}
		public float GetMidScale ()
		{
			return GetMediumScale();
		}
		public float GetMediumScale ()
		{
			return mAttacher.GetMediumScale();
		}
		public float GetMaxScale ()
		{
			return GetMaximumScale();
		}
		public float GetMaximumScale ()
		{
			return mAttacher.GetMaximumScale();
		}
		public float GetScale ()
		{
			return mAttacher.GetScale();
		}
		public override ImageView.ScaleType GetScaleType()
		{
			return mAttacher.GetScaleType();
		}
		public void SetAllowParentInterceptOnEdge (bool allow)
		{
			mAttacher.SetAllowParentInterceptOnEdge(allow);
		}
		public void SetMinScale (float minScale)
		{
			SetMinimumScale(minScale);
		}
		public void SetMinimumScale (float minimumScale)
		{
			mAttacher.SetMinimumScale(minimumScale);
		}
		public void SetMidScale (float midScale)
		{
			SetMediumScale(midScale);
		}
		public void SetMediumScale (float mediumScale)
		{
			mAttacher.SetMediumScale(mediumScale);
		}
		public void SetMaxScale (float maxScale)
		{
			SetMaximumScale(maxScale);
		}
		public void SetMaximumScale (float maximumScale)
		{
			mAttacher.SetMaximumScale(maximumScale);
		}
		public void SetOnMatrixChangeListener (PhotoViewDroidAttacher.IOnMatrixChangedListener listener)
		{
			mAttacher.SetOnMatrixChangeListener(listener);
		}
		public override void SetOnLongClickListener(Android.Views.View.IOnLongClickListener listener)
		{
			mAttacher.SetOnLongClickListener(listener);
		}
		public void SetOnPhotoTapListener (PhotoViewDroidAttacher.IOnPhotoTapListener listener)
		{
			mAttacher.SetOnPhotoTapListener(listener);
		}
		public PhotoViewDroidAttacher.IOnPhotoTapListener GetOnPhotoTapListener ()
		{
			return mAttacher.GetOnPhotoTapListener();
		}
		public void SetOnViewTapListener (PhotoViewDroidAttacher.IOnViewTapListener listener)
		{
			mAttacher.SetOnViewTapListener(listener);
		}
		public void SetRotationTo (float rotationDegree)
		{
			mAttacher.SetRotationTo(rotationDegree);
		}
		public void SetRotationBy (float rotationDegree)
		{
			mAttacher.SetRotationBy(rotationDegree);
		}
		public PhotoViewDroidAttacher.IOnViewTapListener GetOnViewTapListener ()
		{
			return mAttacher.GetOnViewTapListener();
		}
		public void SetScale (float scale)
		{
			mAttacher.SetScale(scale);
		}
		public void SetScale (float scale, bool animate)
		{
			mAttacher.SetScale(scale, animate);
		}
		public void SetScale (float scale, float focalX, float focalY, bool animate)
		{
			mAttacher.SetScale(scale, focalX, focalY, animate);
		}
		public override void SetScaleType (ScaleType scaleType)
		{
			if (null != mAttacher) {
				mAttacher.SetScaleType(scaleType);
			} else {
				mPendingScaleType = scaleType;
			}
		} 
		public void SetZoomable (bool zoomable)
		{
			mAttacher.SetZoomable(zoomable);
		}
		public void SetPhotoViewRotation (float rotationDegree)
		{
			mAttacher.SetRotationTo(rotationDegree);
		}
		public Android.Graphics.Bitmap GetVisibleRectangleBitmap ()
		{
			return mAttacher.GetVisibleRectangleBitmap();
		}
		public void SetZoomTransitionDuration (int milliseconds)
		{
			mAttacher.SetZoomTransitionDuration(milliseconds);
		}
		public IPhotoViewDroid GetIPhotoViewImplementation ()
		{
			return mAttacher;
		}
		public void SetOnDoubleTapListener (Android.Views.GestureDetector.IOnDoubleTapListener newOnDoubleTapListener)
		{
			mAttacher.SetOnDoubleTapListener(newOnDoubleTapListener);
		}
		#endregion
		#region ImageView
		public override void SetImageDrawable (Android.Graphics.Drawables.Drawable drawable)
		{
			base.SetImageDrawable (drawable);
			if (null != mAttacher) {
				mAttacher.Update();
			}
		}

		public override void SetImageResource (int resId)
		{
			base.SetImageResource (resId);
			if (null != mAttacher) {
				mAttacher.Update();
			}
		}
		public override void SetImageURI (Android.Net.Uri uri)
		{
			base.SetImageURI (uri);
			if (null != mAttacher) {
				mAttacher.Update();
			}
		}
		protected override void OnDetachedFromWindow ()
		{
			mAttacher.Cleanup();
			base.OnDetachedFromWindow ();
		}
		#endregion
	}
}

