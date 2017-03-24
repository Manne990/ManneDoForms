using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class RotationPage : ContentPage
    {
        public RotationPage()
        {
            InitializeComponent();
        }

        private void ShowFullRotation(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new RotationFullPage()));
        }

        private void ShowPortraitOnly(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new RotationPortraitOnlyPage()));
        }

        private void ShowLandscapeOnly(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new RotationLandscapeOnlyPage()));
        }

    }
}