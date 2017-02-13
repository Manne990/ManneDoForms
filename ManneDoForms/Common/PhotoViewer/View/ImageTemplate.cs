using System;
using System.Net.Http;
using System.Threading.Tasks;
using ManneDoForms.Common.PhotoViewer.Model;
using Xamarin.Forms;

namespace ManneDoForms.Common.PhotoViewer.View
{
    public class ImageTemplate : ContentView, ICarouselLayoutChildDelegate
    {
        #region Private Members

        private PhotoView _photoView;

        #endregion

        // ------------------------------------------------

        #region Constructors

        public ImageTemplate()
        {
            _photoView = new PhotoView() { BackgroundColor = Color.Gray };
        }

        #endregion

        // ------------------------------------------------

        #region Overrides

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var fileSystem = DependencyService.Get<IFileSystem>();
            var imageName = ((ImageViewModel)this.BindingContext).ImageName;

            if (string.IsNullOrWhiteSpace(imageName) == false && fileSystem.FileExists(imageName))
            {
                _photoView.ImageName = imageName;
            }
        }

        #endregion

        // ------------------------------------------------

        #region ICarouselLayoutChildDelegate implementation

        public async Task Refresh()
        {
            var fileSystem = DependencyService.Get<IFileSystem>();
            var imageName = ((ImageViewModel)this.BindingContext).ImageName;

            if (fileSystem.FileExists(imageName))
            {
                Device.BeginInvokeOnMainThread(() => {
                    Content = _photoView;
                });
            }
        }

        public async Task WillBeActive()
        {
            var fileSystem = DependencyService.Get<IFileSystem>();
            var content = ((ImageViewModel)this.BindingContext);
            var imageName = content.ImageName;

            if (fileSystem.FileExists(imageName) == false)
            {
                var imageUrl = ((ImageViewModel)this.BindingContext).ImageUrl;

                Device.BeginInvokeOnMainThread(() => {
                    Content = new SpinnerView() { IsBusy = true };
                });

                await DownloadAndSaveFile(imageUrl, imageName);

                Device.BeginInvokeOnMainThread(() => {
                    Content = _photoView;
                    _photoView.ImageName = imageName;
                });
            }
        }

        public async Task GotActive()
        {
            Device.BeginInvokeOnMainThread(() => {
                _photoView.IsActive = true;
            });

            //TODO: Find a way to unload invisible images to save memory
        }

        public async Task GotInactive()
        {
            Device.BeginInvokeOnMainThread(() => {
                _photoView.IsActive = false;
            });

            //TODO: Find a way to unload invisible images to save memory
        }

        #endregion

        // ------------------------------------------------

        #region Private Methods

        private async Task DownloadAndSaveFile(string url, string filename)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            var httpClient = new HttpClient();
            var fileSystem = DependencyService.Get<IFileSystem>();

            try
            {
                // Download file
                var streamAsync = await httpClient.GetStreamAsync(url);

                // Save the file
                fileSystem.SaveBinaryFile(filename, streamAsync);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Download failed! {0}", ex.Message);
            }
        }

        #endregion
    }
}