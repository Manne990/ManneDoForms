using System;
using Android.Util;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public class LoggerDefault:ILogger
	{
		#region ILogger implementation

		public int v (string tag, string msg)
		{
			return Log.Verbose(tag, msg);
		}

		public int v (string tag, string msg, Java.Lang.Throwable tr)
		{
			return Log.Verbose(tag, msg, tr);
		}

		public int d (string tag, string msg)
		{
			return Log.Debug(tag, msg);
		}

		public int d (string tag, string msg, Java.Lang.Throwable tr)
		{
			return Log.Debug(tag, msg, tr);
		}

		public int i (string tag, string msg)
		{
			return Log.Info(tag, msg);
		}

		public int i (string tag, string msg, Java.Lang.Throwable tr)
		{
			return Log.Info(tag, msg, tr);
		}

		public int w (string tag, string msg)
		{
			return Log.Warn(tag, msg);
		}

		public int w (string tag, string msg, Java.Lang.Throwable tr)
		{
			return Log.Warn(tag, msg, tr);
		}

		public int e (string tag, string msg)
		{
			return Log.Error(tag, msg);
		}

		public int e (string tag, string msg, Java.Lang.Throwable tr)
		{
			return Log.Error(tag, msg, tr);
		}

		#endregion


	}
}

