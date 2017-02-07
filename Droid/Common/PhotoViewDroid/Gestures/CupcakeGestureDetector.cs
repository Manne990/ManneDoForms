using System;
using Android.Views;
using Android.Content;
using Android.Util;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class CupcakeGestureDetector:IGestureDetector
	{
		protected IOnGestureListener mListener;
		private static readonly String LOG_TAG = "CupcakeGestureDetector";
		public float mLastTouchX;
		public float mLastTouchY;
		public readonly float mTouchSlop;
		public readonly float mMinimumVelocity;

		public void SetOnGestureListener (IOnGestureListener listener)
		{
			this.mListener = listener;
		}


		public CupcakeGestureDetector(Context context) {
			 ViewConfiguration configuration = ViewConfiguration
				.Get(context);
			mMinimumVelocity = configuration.ScaledMinimumFlingVelocity;
			mTouchSlop = configuration.ScaledTouchSlop;
		}

		private VelocityTracker mVelocityTracker;
		private bool mIsDragging;


		public virtual float GetActiveX(MotionEvent ev) {
			return ev.GetX();
		}

		public virtual float GetActiveY(MotionEvent ev) {
			return ev.GetY();
		}


		#region IGestureDetector implementation

		public virtual bool OnTouchEvent (Android.Views.MotionEvent ev)
		{
			switch (ev.Action) {
			case MotionEventActions.Down: 
				{
					mVelocityTracker = VelocityTracker.Obtain();
					if (null != mVelocityTracker) {
						mVelocityTracker.AddMovement(ev);
					} else {
						Log.Info(LOG_TAG, "Velocity tracker is null");
					}

					mLastTouchX = GetActiveX(ev);
					mLastTouchY = GetActiveY(ev);
					mIsDragging = false;
					break;
				}

			case MotionEventActions.Move: {
					float x = GetActiveX(ev);
					float y = GetActiveY(ev);
				    float dx = x - mLastTouchX, dy = y - mLastTouchY;

					if (!mIsDragging) {
						// Use Pythagoras to see if drag length is larger than
						// touch slop
						mIsDragging = FloatMath.Sqrt((dx * dx) + (dy * dy)) >= mTouchSlop;
					}

					if (mIsDragging) {
						mListener.OnDrag(dx, dy);
						mLastTouchX = x;
						mLastTouchY = y;

						if (null != mVelocityTracker) {
							mVelocityTracker.AddMovement(ev);
						}
					}
					break;
				}

			case MotionEventActions.Cancel: {
					// Recycle Velocity Tracker
					if (null != mVelocityTracker) {
						mVelocityTracker.Recycle();
						mVelocityTracker = null;
					}
					break;
				}

			case MotionEventActions.Up: {
					if (mIsDragging) {
						if (null != mVelocityTracker) {
							mLastTouchX = GetActiveX(ev);
							mLastTouchY = GetActiveY(ev);

							// Compute velocity within the last 1000ms
							mVelocityTracker.AddMovement(ev);
							mVelocityTracker.ComputeCurrentVelocity(1000);

							float vX = mVelocityTracker.GetXVelocity(0), vY = mVelocityTracker
								.GetYVelocity(0);

							// If the velocity is greater than minVelocity, call
							// listener
							if (Math.Max(Math.Abs(vX), Math.Abs(vY)) >= mMinimumVelocity) {
								mListener.OnFling(mLastTouchX, mLastTouchY, -vX,
									-vY);
							}
						}
					}

					// Recycle Velocity Tracker
					if (null != mVelocityTracker) {
						mVelocityTracker.Recycle();
						mVelocityTracker = null;
					}
					break;
				}
			}

			return true;
		}

		public virtual bool IsScaling ()
		{
			return false;
		}


		#endregion
	}
}

