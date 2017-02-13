using System;
using Android.Widget;
using Android.Content;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class PreGingerScroller:ScrollerProxy
	{
		private readonly Scroller mScroller;
		public PreGingerScroller(Context context) {
			mScroller = new Scroller(context);
		}

		#region implemented abstract members of ScrollerProxy

		public override bool ComputeScrollOffset ()
		{
			return mScroller.ComputeScrollOffset();
		}

		public override void Fling (int startX, int startY, int velocityX, int velocityY, int minX, int maxX, int minY, int maxY, int overX, int overY)
		{
			mScroller.Fling(startX, startY, velocityX, velocityY, minX, maxX, minY, maxY);
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

