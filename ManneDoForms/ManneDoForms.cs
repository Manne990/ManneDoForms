using ManneDoForms.Samples;
using Xamarin.Forms;
using XLabs.Ioc;

namespace ManneDoForms
{
	public class App : Application
	{
		public App()
		{
            // Register services in IoC Container
            var container = Resolver.Resolve<IDependencyContainer>();

            container.RegisterSingle<Common.Api.IApi, Common.Api.Api>();
            container.RegisterSingle<IFormWithValidationViewModel, FormWithValidationViewModel>();

            // Set Main Page
            MainPage = new NavigationPage(new StartPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}