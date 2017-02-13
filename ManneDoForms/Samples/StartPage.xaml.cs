using ManneDoForms.Common;
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

        private void ShowPdfViewer(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PdfPage());
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