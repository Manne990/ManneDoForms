using System;
using Android.Views;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public interface IGestureDetector
	{
		 bool OnTouchEvent(MotionEvent ev);

		 bool IsScaling();

		 void SetOnGestureListener(IOnGestureListener listener);
	}
}

