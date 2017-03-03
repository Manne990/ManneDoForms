using Autofac;
using Foundation;
using mTouchPDFReader.Library.Interfaces;
using mTouchPDFReader.Library.Managers;
using TinyIoC;
using UIKit;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;

namespace ManneDoForms.iOS
{
    [Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
            // Init IoC Container
            var container = TinyIoCContainer.Current;
            var tinyContainer = new TinyContainer(container);

            container.Register<IDependencyContainer>(tinyContainer);

            Resolver.SetResolver(new TinyResolver(container));

            // Init Forms
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

            // Init PDF Viewer
            var builder = new ContainerBuilder();

            builder.RegisterType<DocumentBookmarksManager>().As<IDocumentBookmarksManager>().SingleInstance();
            builder.RegisterType<DocumentNoteManager>().As<IDocumentNoteManager>().SingleInstance();
            builder.RegisterType<SettingsManager>().As<ISettingsManager>().SingleInstance();

            MgrAccessor.Initialize(builder);

			return base.FinishedLaunching(uiApplication, launchOptions);
		}
	}
}