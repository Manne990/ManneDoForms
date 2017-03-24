using System;
using System.Linq;
using Autofac;
using Foundation;
using ManneDoForms.Common.Rotation;
using mTouchPDFReader.Library.Interfaces;
using mTouchPDFReader.Library.Managers;
using TinyIoC;
using UIKit;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;

namespace ManneDoForms.iOS
{
    [Register("AppDelegate")]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
            // Init IoC Container
            var container = TinyIoCContainer.Current;
            var tinyContainer = new TinyContainer(container);

            container.Register<IDependencyContainer>(tinyContainer);

            Resolver.SetResolver(new TinyResolver(container));

            // Init Forms
            Forms.Init();

			LoadApplication(new App());

            // Init PDF Viewer
            var builder = new ContainerBuilder();

            builder.RegisterType<DocumentBookmarksManager>().As<IDocumentBookmarksManager>().SingleInstance();
            builder.RegisterType<DocumentNoteManager>().As<IDocumentNoteManager>().SingleInstance();
            builder.RegisterType<SettingsManager>().As<ISettingsManager>().SingleInstance();

            MgrAccessor.Initialize(builder);

            // Print out path to the documents folder
            System.Diagnostics.Debug.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            // Return...
			return base.FinishedLaunching(uiApplication, launchOptions);
		}

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            // Handle Device Rotation
            if (Xamarin.Forms.Application.Current == null || Xamarin.Forms.Application.Current.MainPage == null)
            {
                return UIInterfaceOrientationMask.Portrait;
            }

            var page = FindCurrentPage();

            return GetPageRequestedOrientation(page);
        }

        private static Page FindCurrentPage()
        {
            var page = FindCurrentModalPage(Xamarin.Forms.Application.Current.MainPage);

            if (page == null)
            {
                page = FindCurrentChildPage(Xamarin.Forms.Application.Current.MainPage);
            }

            return page;
        }

        private static Page FindCurrentModalPage(Page mainPage)
        {
            Page currentPage = null;

            if (mainPage.Navigation != null && mainPage.Navigation.ModalStack.Count > 0)
            {
                var page = mainPage.Navigation.ModalStack.LastOrDefault();

                currentPage = FindCurrentChildPage(page);
            }

            return currentPage;
        }

        private static Page FindCurrentChildPage(Page mainPage)
        {
            Page currentPage = null;

            if (mainPage is NavigationPage)
            {
                // Root Page is NavigationPage
                currentPage = FindCurrentChildPage(((NavigationPage)mainPage).CurrentPage);
            }
            else if (mainPage is TabbedPage)
            {
                // Root Page is TabbedPage
                currentPage = FindCurrentChildPage(((TabbedPage)mainPage).CurrentPage);
            }
            else
            {
                currentPage = mainPage;
            }

            return currentPage;
        }

        private static UIInterfaceOrientationMask GetPageRequestedOrientation(Page page)
        {
            if (page is RotationAwarePage)
            {
                return MapOrientation(((RotationAwarePage)page).RequestedOrientation);
            }
            else
            {
                return UIInterfaceOrientationMask.Portrait;
            }
        }

        private static UIInterfaceOrientationMask MapOrientation(InterfaceOrientationTypes orientation)
        {
            switch (orientation)
            {
                case InterfaceOrientationTypes.All:
                    return UIInterfaceOrientationMask.All;

                case InterfaceOrientationTypes.AllButUpsideDown:
                    return UIInterfaceOrientationMask.AllButUpsideDown;

                case InterfaceOrientationTypes.Portrait:
                    return UIInterfaceOrientationMask.Portrait;

                case InterfaceOrientationTypes.Landscape:
                    return UIInterfaceOrientationMask.Landscape;
            }

            return UIInterfaceOrientationMask.Portrait;
        }
	}
}