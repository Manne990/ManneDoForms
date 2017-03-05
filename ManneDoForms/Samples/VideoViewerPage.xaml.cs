using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class VideoViewerPage : ContentPage
    {
        public VideoViewerPage()
        {
            InitializeComponent();
        }

        private void ShowDownloadVideoPage(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new DownloadAndPlayVideoPage());
        }

        private void ShowRecordVideoPage(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RecordAndPlayVideoPage());
        }
    }
}