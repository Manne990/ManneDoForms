using System.Collections.Generic;
using ManneDoForms.Components.PhotoViewer.Model;
using Xamarin.Forms;

namespace ManneDoForms.Components.PhotoViewer.View
{
    public class PhotoCarousel : ContentView
    {
        #region Private Members

        private CarouselLayout _carouselLayout;

        #endregion

        // ---------------------------------------------------------

        #region Constructors

        public PhotoCarousel()
        {
            Content = CreatePagesCarousel();
        }

        #endregion

        // ---------------------------------------------------------

        #region Public Methods

        public void LoadPhotos(List<ImageViewModel> images, int selectedIndex)
        {
            if (_carouselLayout == null)
            {
                return;
            }

            _carouselLayout.ItemsSource = images;
            _carouselLayout.SelectedIndex = selectedIndex;
        }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private CarouselLayout CreatePagesCarousel()
        {
            _carouselLayout = new CarouselLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IndicatorStyle = CarouselLayout.IndicatorStyleEnum.None,
                ItemTemplate = new DataTemplate(typeof(ImageTemplate))
            };

            return _carouselLayout;
        }

        #endregion
    }
}