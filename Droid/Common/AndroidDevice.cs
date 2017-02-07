using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace ManneDoForms.Droid.Common
{
	public static class AndroidDevice
	{
		private static IWindowManager _windowManager;
		private static DisplayMetrics _displayMetrics;

		static AndroidDevice()
		{
			_windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
		}

		public static DisplayMetrics DisplayMetrics
		{
			get
			{
				if (_displayMetrics == null)
				{
					_displayMetrics = new DisplayMetrics();

					_windowManager.DefaultDisplay.GetMetrics(_displayMetrics);
				}

				return _displayMetrics;
			}
		}
	}
}