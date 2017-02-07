using System;
using Android.Widget;
using Android.Content;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class GingerScroller:ScrollerProxy
	{
		protected readonly OverScroller mScroller;
		private bool mFirstScroll = false;

		public GingerScroller(Context context) {
			mScroller = new OverScroller(context);
		}

		#region implemented abstract members of ScrollerProxy
		public override bool ComputeScrollOffset ()
		{
			if (mFirstScroll) {
				mScroller.ComputeScrollOffset();
				mFirstScroll = false;
			}
			return mScroller.ComputeScrollOffset();
		}
		public override void Fling (int startX, int startY, int velocityX, int velocityY, int minX, int maxX, int minY, int maxY, int overX, int overY)
		{
			mScroller.Fling(startX, startY, velocityX, velocityY, minX, maxX, minY, maxY, overX, overY);
		}
		public override void ForceFinished (bool finished)
		{
				mScroller.ForceFinished(finished);
		}
		public override bool IsFinished ()
		{
			return mScroller.IsFinished;
		}
		public override int GetCurrX ()
		{
			return mScroller.CurrX;
		}
		public override int GetCurrY ()
		{
			return mScroller.CurrY;
		}
		#endregion
	}
}

