using System;
using System.Net.Http;
using System.Threading.Tasks;
using ManneDoForms.Common;
using ManneDoForms.Components.VideoView;
using Xamarin.Forms;
using XLabs.Ioc;

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
                var api = Resolver.Resolve<IApi>();

                await api.DownloadAndSaveFile(url, localFilename);
            }

            var localFilePath = fileSystem.GetFilePath(localFilename);
            var videoView = new ManneVideoView() { Url = localFilePath };

            AbsoluteLayout.SetLayoutFlags(videoView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(videoView, new Rectangle(0f, 0f, 1f, 1f));

            _parentContainer.Children.Add(videoView);
        }
    }
}