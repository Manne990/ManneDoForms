using System;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using ManneDoForms.Common;
using ManneDoForms.Common.Api;
using ManneDoForms.Components.ImageView;
using ManneDoForms.iOS.Common;
using ManneDoForms.iOS.Common.FileSystem;
using ManneDoForms.iOS.Renderers.ImageView;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Ioc;

[assembly: ExportRenderer(typeof(ManneImageView), typeof(ManneImageViewRenderer))]
namespace ManneDoForms.iOS.Renderers.ImageView
{
    public class ManneImageViewRenderer : ImageRenderer
    {
        // Private Members
        private ManneImageView _formsView;


        // -----------------------------------------------------------------------------

        // Overrides
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _formsView = e.NewElement as ManneImageView;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ManneImageView.Source))
            {
                LoadImage();
            }

            //if (e.PropertyName == "Renderer")
            //{
            //    LoadImage();
            //}
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            LoadImage();
        }


        // -----------------------------------------------------------------------------

        // Private Methods
        private void LoadImage()
        {
            if (_formsView?.Source == null)
            {
                return;
            }

            var uriSource = _formsView.Source as UriImageSource;
            if (uriSource != null)
            {
                Control.Image = null;
                _formsView.IsBusy = true;

                Task.Run(async () => await LoadImageFromUrlTask(uriSource.Uri.AbsoluteUri, (int)Bounds.Width, (int)Bounds.Height, _formsView.ScaleType, (image) => 
                {
                    InvokeOnMainThread(() => 
                    {
                        _formsView.IsBusy = false;
                        Control.Image = image;
                    });
                }));
            }

            //TODO: Implement other types of image sources
        }

        private async Task LoadImageFromUrlTask(string url, int width, int height, ManneImageView.ImageScaleTypes scaleType = ManneImageView.ImageScaleTypes.Crop, Action<UIImage> loadedListener = null)
        {
            var shouldScale = true;
            var fileSystem = new FileSystem();

            try
            {
                // Construct the filename
                var filename = $"{Base32.ToBase32String(Encoding.ASCII.GetBytes($"{url}|{width}|{height}"))}.jpg";

                if (shouldScale)
                {
                    // Try to get a cached image
                    var cachedImageBytes = await fileSystem.LoadBinaryAsync(filename);
                    if (cachedImageBytes != null)
                    {
                        var cachedImage = ImageHelper.ImageFromBytes(cachedImageBytes);

                        loadedListener?.Invoke(cachedImage);
                        return;
                    }
                }

                // Load the image from the API
                var loader = Resolver.Resolve<IApi>();
                var imageBytes = await loader.DownloadFile(url);
                if (imageBytes == null)
                {
                    System.Diagnostics.Debug.WriteLine($"LoadImageTask: Error for file '{url}', 'Image could not be loaded!'");
                    loadedListener?.Invoke(null);
                    return;
                }

                var image = ImageHelper.ImageFromBytes(imageBytes);
                if (image == null)
                {
                    System.Diagnostics.Debug.WriteLine($"LoadImageTask: Error for file '{url}', 'Image could not be parsed!'");
                    loadedListener?.Invoke(null);
                    return;
                }

                if (shouldScale)
                {
                    switch (scaleType)
                    {
                        case ManneImageView.ImageScaleTypes.Crop:
                            image = ImageHelper.CropAndScaleImage(image, new CGSize(width, height), ImageHelper.CropOrigoTypes.Center);
                            break;

                        case ManneImageView.ImageScaleTypes.TopCrop:
                            image = ImageHelper.CropAndScaleImage(image, new CGSize(width, height), ImageHelper.CropOrigoTypes.Top);
                            break;

                        case ManneImageView.ImageScaleTypes.ScaleToFit:
                            image = ImageHelper.ScaleImage(image, new CGSize(width, height));
                            break;
                    }

                    // Save the cached image
                    var bytes = ImageHelper.ImageToBytes(image, "jpg");
                    if (bytes == null)
                    {
                        loadedListener?.Invoke(image);
                        return;
                    }

                    fileSystem.SaveBinaryFile(filename, bytes);
                }

                // Return and Notify
                loadedListener?.Invoke(image);
                return;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadImageTask: Error for file '{url}', '{ex.Message}'");
            }

            loadedListener?.Invoke(null);
            return;
        }
    }
}