using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class PhotoViewerPage : ContentPage
    {
        public PhotoViewerPage()
        {
            InitializeComponent();
        }

        private void ShowSingleImage(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SingleImagePage());
        }

        private void ShowPhotoCarousel(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PhotoCarouselPage());
        }
    }
}