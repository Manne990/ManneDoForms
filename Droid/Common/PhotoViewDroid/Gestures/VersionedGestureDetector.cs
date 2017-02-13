using System;
using Android.Views;
using Android.Content;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public sealed class VersionedGestureDetector
	{
		public static IGestureDetector NewInstance(Context context,
			IOnGestureListener listener) {
			int sdkVersion = (int)Android.OS.Build.VERSION.SdkInt;
			IGestureDetector detector;

			if (sdkVersion < (int)Android.OS.Build.VERSION_CODES.Eclair) {
				detector = new CupcakeGestureDetector(context);
			} else if (sdkVersion < (int)Android.OS.Build.VERSION_CODES.Froyo) {
				detector = new EclairGestureDetector(context);
			} else {
				detector = new FroyoGestureDetector(context);
			}

			detector.SetOnGestureListener(listener);

			return detector;
		}
	}
}

