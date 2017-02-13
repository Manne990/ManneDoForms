using System;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public sealed class LogManager
	{
		private static ILogger logger = new LoggerDefault();

		public static void SetLogger(ILogger newLogger) {
			logger = newLogger;
		}

		public static ILogger GetLogger() {
			return logger;
		}
	}
}

