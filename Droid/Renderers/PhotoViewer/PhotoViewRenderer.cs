using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using ManneDoForms.Components.PhotoViewer.View;
using ManneDoForms.Droid.Common.FileSystem;
using ManneDoForms.Droid.Common.PhotoViewDroid;
using ManneDoForms.Droid.Renderers.PhotoViewer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PhotoView), typeof(PhotoViewRenderer))]
namespace ManneDoForms.Droid.Renderers.PhotoViewer
{
    public class PhotoViewRenderer : ViewRenderer<PhotoView, PhotoViewDroid>, PhotoViewDroidAttacher.IOnViewTapListener
    {
        #region Private Members

        private static readonly int MAX_IMAGE_SIZE_WIDTH = 512;
        private static readonly int MAX_IMAGE_SIZE_HEIGHT = 512;

        private PhotoView _view;
        private PhotoViewDroid _photoView;
        private PhotoViewDroidAttacher _photoViewAttacher;

        #endregion

        // ---------------------------------------------------------

        #region Overrides

        protected async override void OnElementChanged(ElementChangedEventArgs<PhotoView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                CleanUpRenderer();
            }

            if (e.NewElement != null)
            {
                _view = e.NewElement;

                if (string.IsNullOrWhiteSpace(_view.ImageName) == false)
                {
                    var fileSystem = new FileSystem();
                    var filePath = fileSystem.GetFilePath(_view.ImageName);

                    await InitializeRenderer(_view, filePath);
                }
            }
        }

        protected async override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsActive" && _photoViewAttacher != null)
            {
                // Reset zoom scale when image gets active (visible)
                _photoViewAttacher.SetScale(_photoViewAttacher.GetMinimumScale(), 0, 0, false);
            }

            if (e.PropertyName == "ImageName" && string.IsNullOrWhiteSpace(_view.ImageName) == false)
            {
                var fileSystem = new FileSystem();
                var filePath = fileSystem.GetFilePath(_view.ImageName);

                await InitializeRenderer(_view, filePath);
            }
        }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private async Task InitializeRenderer(PhotoView view, string filePath)
        {
            // Create the Photo View
            _photoView = new PhotoViewDroid(this.Context);
            _photoViewAttacher = _photoView.GetPhotoViewDroidAttacher();

            _photoViewAttacher.SetOnViewTapListener(this);

            // Prepare the image
            var options = await GetBitmapOptionsOfImageAsync(filePath);
            var bitmapToDisplay = await LoadScaledDownBitmapForDisplayAsync(Resources, filePath, options, MAX_IMAGE_SIZE_WIDTH, MAX_IMAGE_SIZE_HEIGHT);

            // Set the scroll view as the native control
            this.SetNativeControl(_photoView);

            // Set the image
            this.Control.SetImageBitmap(bitmapToDisplay);
        }

        private void CleanUpRenderer()
        {
            _photoViewAttacher.Cleanup();
            _photoViewAttacher.Dispose();

            _photoView.Dispose();
        }

        private async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync(string filePath)
        {
            var options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            // The result will be null because InJustDecodeBounds == true.
            await BitmapFactory.DecodeFileAsync(filePath, options);

            int imageHeight = options.OutHeight;
            int imageWidth = options.OutWidth;

            return options;
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }

        private async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, string filePath, BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;

            return await BitmapFactory.DecodeFileAsync(filePath, options);
        }

        #endregion

        #region Private Event Forwarding

        public void OnViewTap(Android.Views.View view, float x, float y)
        {
            OnItemTapped();
        }

        protected void OnItemTapped()
        {
            ((PhotoView)Element).OnTap();
        }

        #endregion
    }
}