using System;
using Android.Content;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public abstract class ScrollerProxy
	{
		public static ScrollerProxy GetScroller(Context context) {
			if ((int)Android.OS.Build.VERSION.SdkInt < (int)Android.OS.Build.VERSION_CODES.Gingerbread) {
				return new PreGingerScroller(context);
			} else if ((int)Android.OS.Build.VERSION.SdkInt < (int)Android.OS.Build.VERSION_CODES.IceCreamSandwich) {
				return new GingerScroller(context);
			} else {
				return new IcsScroller(context);
			}
		}

		public abstract bool ComputeScrollOffset();

		public abstract void Fling(int startX, int startY, int velocityX, int velocityY, int minX, int maxX, int minY,
			int maxY, int overX, int overY);

		public abstract void ForceFinished(bool finished);

		public abstract bool IsFinished();

		public abstract int GetCurrX();

		public abstract int GetCurrY();
	}
}

