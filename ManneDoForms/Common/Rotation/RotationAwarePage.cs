using Xamarin.Forms;

namespace ManneDoForms.Common.Rotation
{
    public class RotationAwarePage : ContentPage
    {
        public static string NewPageOrientationRequestedMessage = "NewPageOrientationRequestedMessage";

        public RotationAwarePage()
        {
            RequestedOrientation = InterfaceOrientationTypes.Portrait;
        }

        public RotationAwarePage(InterfaceOrientationTypes requestedOrientation)
        {
            RequestedOrientation = requestedOrientation;
        }

        public InterfaceOrientationTypes RequestedOrientation { get; private set; }

        protected override void OnAppearing()
        {
            MessagingCenter.Send<RotationAwarePage, InterfaceOrientationTypes>(this, NewPageOrientationRequestedMessage, RequestedOrientation);

            base.OnAppearing();
        }
    }
}