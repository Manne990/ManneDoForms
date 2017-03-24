using Android.App;
using Android.Content.PM;
using Android.OS;
using ManneDoForms.Common.Rotation;
using Plugin.Permissions;
using TinyIoC;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;

namespace ManneDoForms.Droid
{
    [Activity(Label = "Manne Do Forms", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
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
            Forms.Init(this, savedInstanceState);

            // Handle Device Rotation
            MessagingCenter.Subscribe<RotationAwarePage, InterfaceOrientationTypes>(this, RotationAwarePage.NewPageOrientationRequestedMessage, NewOrientationRequested);

            // Set default orientation
            RequestedOrientation = ScreenOrientation.Portrait;

            // Load App
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void NewOrientationRequested(object sender, InterfaceOrientationTypes arg)
        {
            RequestedOrientation = MapOrientation(arg);
        }

        private ScreenOrientation MapOrientation(InterfaceOrientationTypes orientation)
        {
            switch (orientation)
            {
                case InterfaceOrientationTypes.All:
                    return ScreenOrientation.FullSensor;

                case InterfaceOrientationTypes.AllButUpsideDown:
                    return ScreenOrientation.FullSensor;

                case InterfaceOrientationTypes.Portrait:
                    return ScreenOrientation.Portrait;

                case InterfaceOrientationTypes.Landscape:
                    return ScreenOrientation.Landscape;
            }

            return ScreenOrientation.Portrait;
        }
    }
}