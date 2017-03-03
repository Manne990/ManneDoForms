using Android.App;
using Android.Content.PM;
using Android.OS;
using ManneDoForms.Droid.Common;
using TinyIoC;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;

namespace ManneDoForms.Droid
{
    [Activity(Label = "Manne Do Forms", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            // Init IoC Container
            var container = TinyIoCContainer.Current;
            var tinyContainer = new TinyContainer(container);

            container.Register<IDependencyContainer>(tinyContainer);

            Resolver.SetResolver(new TinyResolver(container));

            // Init Forms
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.Forms.DependencyService.Register<RepeaterViewSample>();

			LoadApplication(new App());
		}
	}
}