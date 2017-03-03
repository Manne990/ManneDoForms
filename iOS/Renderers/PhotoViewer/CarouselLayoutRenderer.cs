using System;
using System.ComponentModel;
using CoreGraphics;
using ManneDoForms.Components.PhotoViewer.View;
using ManneDoForms.iOS.Renderers.PhotoViewer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]
namespace ManneDoForms.iOS.Renderers.PhotoViewer
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        CarouselLayout _view;
        UIScrollView _native;
        nfloat _lastContentOffsetX;

        public CarouselLayoutRenderer()
        {
            PagingEnabled = true;
            ShowsHorizontalScrollIndicator = false;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) return;

            _view = (CarouselLayout)e.NewElement;
            _native = (UIScrollView)NativeView;
            _native.Scrolled += NativeScrolled;

            e.NewElement.PropertyChanged += ElementPropertyChanged;
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);

            ScrollToSelection(false);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        private void NativeScrolled(object sender, EventArgs e)
        {
            if (_native.ContentOffset.X == 0 && _lastContentOffsetX > 100)
            {
                ScrollToSelection(false);
                return;
            }

            var center = _native.ContentOffset.X + (_native.Bounds.Width / 2);

            ((CarouselLayout)Element).SelectedIndex = ((int)center) / ((int)_native.Bounds.Width);

            _lastContentOffsetX = _native.ContentOffset.X;
        }

        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName && !Dragging)
            {
                ScrollToSelection(false);
            }
        }

        private void ScrollToSelection(bool animate)
        {
            if (Element == null)
                return;

            _native.SetContentOffset(new CGPoint(_native.Bounds.Width * Math.Max(0, ((CarouselLayout)Element).SelectedIndex), _native.ContentOffset.Y), animate);
        }
    }
}