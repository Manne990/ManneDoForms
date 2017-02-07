using System;
using Android.Views;
using Java.Lang;
using Android.Annotation;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class Compat
	{
		private static readonly int SIXTY_FPS_INTERVAL = 1000 / 60;

		public static void PostOnAnimation(View view, IRunnable runnable) {
			if ((int)Android.OS.Build.VERSION.SdkInt >= (int)Android.OS.Build.VERSION_CODES.JellyBean) {
				PostOnAnimationJellyBean(view, runnable);
			} else {
				view.PostDelayed(runnable, SIXTY_FPS_INTERVAL);
			}
		}


		private static void PostOnAnimationJellyBean(View view, IRunnable runnable) {
			view.PostOnAnimation (runnable);
		}

		public static int GetPointerIndex(MotionEventActions action) {
			if ((int)Android.OS.Build.VERSION.SdkInt >= (int)Android.OS.Build.VERSION_CODES.Honeycomb)
				return GetPointerIndexHoneyComb(action);
			else
				return GetPointerIndexEclair(action);
		}



		private static int GetPointerIndexEclair(MotionEventActions action) {
			return ((int)action & (int)MotionEventActions.PointerIdMask) >> (int)MotionEventActions.PointerIdShift;
		}


		private static int GetPointerIndexHoneyComb(MotionEventActions action) {
			return ((int)action & (int)MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
		}
	}
}

