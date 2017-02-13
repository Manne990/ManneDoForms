using System;
using Android.Content;
using Android.Views;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class EclairGestureDetector:CupcakeGestureDetector
	{
		private static readonly int INVALID_POINTER_ID = -1;
		private int mActivePointerId = INVALID_POINTER_ID;
		private int mActivePointerIndex = 0;

		public EclairGestureDetector(Context context) :base(context){

		}
		public override float GetActiveX(MotionEvent ev){
			try {
				return ev.GetX(mActivePointerIndex);
			} catch (Exception e) {
				return ev.GetX();
			}
		}
		public override float GetActiveY (MotionEvent ev)
		{
			try {
				return ev.GetY(mActivePointerIndex);
			} catch (Exception e) {
				return ev.GetY();
			}
		}
		public override bool OnTouchEvent (MotionEvent ev)
		{
			MotionEventActions action = ev.Action;
			switch (action & MotionEventActions.Mask) {
			case MotionEventActions.Down:
				mActivePointerId = ev.GetPointerId(0);
				break;
			case MotionEventActions.Cancel:
			case MotionEventActions.Up:
				mActivePointerId = INVALID_POINTER_ID;
				break;
			case MotionEventActions.PointerUp:
				// Ignore deprecation, ACTION_POINTER_ID_MASK and
				// ACTION_POINTER_ID_SHIFT has same value and are deprecated
				// You can have either deprecation or lint target api warning
				int pointerIndex = Compat.GetPointerIndex(ev.Action);
				 int pointerId = ev.GetPointerId(pointerIndex);
				if (pointerId == mActivePointerId) {
					// This was our active pointer going up. Choose a new
					// active pointer and adjust accordingly.
					 int newPointerIndex = pointerIndex == 0 ? 1 : 0;
					mActivePointerId = ev.GetPointerId(newPointerIndex);
					mLastTouchX = ev.GetX(newPointerIndex);
					mLastTouchY = ev.GetY(newPointerIndex);
				}
				break;
			}

			mActivePointerIndex = ev
				.FindPointerIndex(mActivePointerId != INVALID_POINTER_ID ? mActivePointerId
					: 0);
			return base.OnTouchEvent(ev);
		}
	}
}

