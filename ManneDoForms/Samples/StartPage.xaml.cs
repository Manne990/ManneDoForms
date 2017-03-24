using ManneDoForms.Common.Sample;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void ShowPaintView(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PaintViewPage());
        }

        private void ShowRangeSliderView(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RangeSliderViewPage());
        }

        private void ShowTableView(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new TableViewPage());
        }

        private void ShowPhotoViewer(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PhotoViewerPage());
        }

        private void ShowVideoViewer(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new VideoViewerPage());
        }

        private void ShowPdfViewer(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PdfPage());
        }

        private void ShowDropDown(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new DropDownViewPage());
        }

        private void ShowFormWithValidation(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FormWithValidationPage());
        }

        private void ShowRotation(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RotationPage());
        }

        private void ShowRepeaterView(object sender, System.EventArgs e)
        {
            if (Device.OS != TargetPlatform.Android)
            {
                DisplayAlert("Not Available", "This is only for Android.", "Ok");
                return;
            }

            DependencyService.Get<IRepeaterViewSample>().Show(); 
        }
    }
}