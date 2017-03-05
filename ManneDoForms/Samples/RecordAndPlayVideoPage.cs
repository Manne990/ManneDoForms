using System;
using System.Threading.Tasks;
using ManneDoForms.Components.VideoView;
using Plugin.Media;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public class RecordAndPlayVideoPage : ContentPage
    {
        private readonly AbsoluteLayout _parentContainer;
        private Button _button;
        private ManneVideoView _videoView;

        public RecordAndPlayVideoPage()
        {
            // Create Container
            _parentContainer = new AbsoluteLayout
            {
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Create Button
            _button = new Button
            {
                Text = "Record!"
            };

            AbsoluteLayout.SetLayoutFlags(_button, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(_button, new Rectangle(0.5f, 0.5f, 100f, AbsoluteLayout.AutoSize));

            _button.Clicked += async (sender, e) =>
            {
                await RecordVideo();
            };

            _parentContainer.Children.Add(_button);

            // Create Video View
            _videoView = new ManneVideoView { IsVisible = false };

            AbsoluteLayout.SetLayoutFlags(_videoView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_videoView, new Rectangle(0f, 0f, 1f, 1f));

            _parentContainer.Children.Add(_videoView);

            // Finish
            Content = _parentContainer;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await CrossMedia.Current.Initialize();
        }

        private async Task RecordVideo()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Recorder", "No camera available", "OK");
                return;
            }

            var filename = $"{Guid.NewGuid().ToString()}.mp4";
            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                Name = filename
            });

            if (file == null)
            {
                await DisplayAlert("Recorder", "The recording was canceled or an error occured", "OK");
                return;
            }

            PlayVideo(file.Path);
        }

        private void PlayVideo(string localFilePath)
        {
            _button.IsVisible = false;
            _videoView.IsVisible = true;

            _videoView.Url = localFilePath;
        }
    }
}