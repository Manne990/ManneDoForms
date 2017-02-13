using System;
using Android.Views;
using Android.Content;
using Java.Lang;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class FroyoGestureDetector:EclairGestureDetector
	{
		protected readonly ScaleGestureDetector mDetector;

		public FroyoGestureDetector(Context context) :base(context){
			OnScaleGestureListener mScaleListener = new OnScaleGestureListener (this);
			mDetector = new ScaleGestureDetector(context, mScaleListener);
		}
		public override bool IsScaling(){
			return mDetector.IsInProgress;
		}
		public override bool OnTouchEvent (MotionEvent ev)
		{
			mDetector.OnTouchEvent(ev);
			return base.OnTouchEvent(ev);
		}
		private class OnScaleGestureListener:Java.Lang.Object,Android.Views.ScaleGestureDetector.IOnScaleGestureListener
		{
			FroyoGestureDetector froyoGestureDetector;
			public OnScaleGestureListener(FroyoGestureDetector froyoGestureDetector)
			{
				this.froyoGestureDetector=froyoGestureDetector;
			}
			#region IOnScaleGestureListener implementation

			public bool OnScale (ScaleGestureDetector detector)
			{
				float scaleFactor = detector.ScaleFactor;

				if (Float.InvokeIsNaN(scaleFactor) || Float.InvokeIsInfinite(scaleFactor))
					return false;

				froyoGestureDetector.mListener.OnScale (scaleFactor,
					detector.FocusX, detector.FocusY);
				return true;
			}

			public bool OnScaleBegin (ScaleGestureDetector detector)
			{
				return true;
			}

			public void OnScaleEnd (ScaleGestureDetector detector)
			{

			}

			#endregion
		}
	}
}

