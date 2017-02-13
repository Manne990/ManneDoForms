using System.Collections.Generic;
using ManneDoForms.Common.PhotoViewer.Model;
using ManneDoForms.Common.PhotoViewer.View;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public class PhotoCarouselPage : ContentPage
    {
        #region Private Members

        private PhotoCarousel _photoCarousel;

        #endregion

        // ------------------------------------------------

        #region Constructors

        public PhotoCarouselPage()
        {
            // Create the photo carousel
            var layout = new RelativeLayout();

            _photoCarousel = new PhotoCarousel() { BackgroundColor = Color.Gray };

            layout.Children.Add(_photoCarousel,
                Constraint.RelativeToParent((parent) => { return parent.X; }),
                Constraint.RelativeToParent((parent) => { return parent.Y; }),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );

            Content = layout;
        }

        #endregion

        // ------------------------------------------------

        #region Lifecycle

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Add some photos
            var photos = new List<ImageViewModel>();

            photos.Add(new ImageViewModel() { ImageName = "bild1.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild1.jpg" });
            photos.Add(new ImageViewModel() { ImageName = "bild2.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild2.jpg" });
            photos.Add(new ImageViewModel() { ImageName = "bild3.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild3.jpg" });
            photos.Add(new ImageViewModel() { ImageName = "bild4.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild4.jpg" });
            photos.Add(new ImageViewModel() { ImageName = "bild5.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild5.jpg" });
            photos.Add(new ImageViewModel() { ImageName = "bild6.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild6.jpg" });
            photos.Add(new ImageViewModel() { ImageName = "bild7.jpg", ImageUrl = "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild7.jpg" });

            _photoCarousel.LoadPhotos(photos, 0);
        }

        #endregion
    }
}