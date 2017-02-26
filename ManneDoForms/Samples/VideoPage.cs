using System;
using System.Net.Http;
using System.Threading.Tasks;
using ManneDoForms.Common;
using ManneDoForms.Components.VideoView;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public class VideoPage : ContentPage
    {
        private AbsoluteLayout _parentContainer;

        public VideoPage()
        {
            _parentContainer = new AbsoluteLayout()
            {
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Content = _parentContainer;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await LoadVideo();
        }

        private async Task LoadVideo()
        {
            var url = "http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_1mb.mp4";
            var localFilename = "bunny.mp4";

            var fileSystem = DependencyService.Get<IFileSystem>();
            if (fileSystem.FileExists(localFilename) == false)
            {
                await DownloadAndSaveFile(url, localFilename);
            }

            var localFilePath = fileSystem.GetFilePath(localFilename);
            var videoView = new ManneVideoView() { Url = localFilePath };

            AbsoluteLayout.SetLayoutFlags(videoView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(videoView, new Rectangle(0f, 0f, 1f, 1f));

            _parentContainer.Children.Add(videoView);
        }

        private async Task DownloadAndSaveFile(string url, string filename)
        {
            var httpClient = new HttpClient();
            var fileSystem = DependencyService.Get<IFileSystem>();

            try
            {
                // Download file
                var streamAsync = await httpClient.GetStreamAsync(url);

                // Save the file
                fileSystem.SaveBinaryFile(filename, streamAsync);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Download failed! {0}", ex.Message);
            }
        }
    }
}