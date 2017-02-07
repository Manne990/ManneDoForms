using System;
using CoreGraphics;
using ManneDoForms.Common.PhotoViewer.View;
using ManneDoForms.iOS.Common;
using ManneDoForms.iOS.Common.FileSystem;
using ManneDoForms.iOS.Renderers.PhotoViewer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PhotoView), typeof(PhotoViewRenderer))]
namespace ManneDoForms.iOS.Renderers.PhotoViewer
{
    public class PhotoViewRenderer : ViewRenderer<PhotoView, UIView>
    {
        #region Private Members

        private UIScrollView _scrollView;
        private UIImageView _imageView;
        private PhotoView _view;
        private bool _doubleTapHandled = false;

        #endregion

        // ---------------------------------------------------------

        #region Enums

        public enum AutoScaleModes
        {
            AutoWidth,
            AutoHeight
        }

        #endregion

        // ---------------------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<PhotoView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                CleanUpRenderer();
            }

            if (e.NewElement != null)
            {
                _view = e.NewElement;

                InitializeRenderer(_view);

                LoadImage();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsActive")
            {
                // Reset zoom scale when image gets active (visible)
                _scrollView.SetZoomScale(_scrollView.MinimumZoomScale, false);
            }

            if (e.PropertyName == "ImageName")
            {
                LoadImage();

                this.LayoutSubviews();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_scrollView == null || _imageView == null)
            {
                return;
            }

            // Update the scroll view
            _scrollView.ContentSize = new CGSize(_scrollView.Frame.Width, _scrollView.Frame.Height);
            _imageView.Frame = new CGRect(0, 0, _imageView.Image.Size.Width, _imageView.Image.Size.Height);

            UpdateMinimumMaximumZoom();

            _scrollView.SetZoomScale(_scrollView.MinimumZoomScale, false);

            // Center the image view
            CenterImageInScrollView();
        }

        #endregion

        // ---------------------------------------------------------

        #region Event Handlers

        private void OnDoubleTap(UIGestureRecognizer gesture)
        {
            //double tap handled. So do not process single tap 
            _doubleTapHandled = true;

            var location = gesture.LocationInView(_scrollView);

            if (_scrollView.ZoomScale > _scrollView.MinimumZoomScale)
            {
                ZoomToPoint(location, _scrollView.MinimumZoomScale, true);
            }
            else
            {
                ZoomToPoint(location, _scrollView.MinimumZoomScale * 3, true);
            }
        }

        private bool OnTap()
        {
            if (_doubleTapHandled == false)
            {
                OnItemTapped();
            }
            return false;
        }

        private void OnTapDelayed(UIGestureRecognizer gesture)
        {
            _doubleTapHandled = false;
            Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), OnTap);
        }

        protected void OnItemTapped()
        {
            if (_view != null)
                _view.OnTap();
        }
        #endregion

        // ---------------------------------------------------------

        #region Public Properties

        public AutoScaleModes AutoScaleMode { get; set; }

        #endregion

        // ---------------------------------------------------------

        #region Private Methods

        private void InitializeRenderer(PhotoView view)
        {
            // Init
            this.AutoScaleMode = AutoScaleModes.AutoHeight;

            // Create the scroll view
            _scrollView = new UIScrollView
            {
                PagingEnabled = false,
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = false,
                ScrollsToTop = false
            };

            // Create the image view
            _imageView = new UIImageView();

            _scrollView.AddSubview(_imageView);

            // Handle events
            _scrollView.DidZoom += (object sender, EventArgs ee) => { CenterImageInScrollView(); };
            _scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return _imageView; };

            // Set the scroll view as the native control
            this.SetNativeControl(_scrollView);

            // Add gesture recognizers
            this.Control.AddGestureRecognizer(new UITapGestureRecognizer(OnTapDelayed) { NumberOfTapsRequired = 1 });
            this.Control.AddGestureRecognizer(new UITapGestureRecognizer(OnDoubleTap) { NumberOfTapsRequired = 2 });
        }

        private void CleanUpRenderer()
        {
            _imageView.Dispose();
            _scrollView.Dispose();
        }

        private void LoadImage()
        {
            if (string.IsNullOrWhiteSpace(_view.ImageName))
            {
                _imageView.Image = UIImage.FromFile("placeholder.png");
                return;
            }

            var fileSystem = new FileSystem();
            var imageBytes = fileSystem.LoadBinary(_view.ImageName);

            if (imageBytes == null)
            {
                _imageView.Image = UIImage.FromFile("placeholder.png");
                return;
            }

            _imageView.Image = imageBytes.ToUIImage();
        }

        private void UpdateMinimumMaximumZoom()
        {
            nfloat zoomScale = GetZoomScaleThatFits(this.Bounds.Size, _imageView.Bounds.Size);

            if (nfloat.IsNaN(zoomScale) || nfloat.IsPositiveInfinity(zoomScale) || nfloat.IsNegativeInfinity(zoomScale))
            {
                zoomScale = 1.0f;
            }

            _scrollView.MinimumZoomScale = zoomScale * 0.99f;
            _scrollView.MaximumZoomScale = _scrollView.MinimumZoomScale * 6;
        }

        private nfloat GetZoomScaleThatFits(CGSize target, CGSize source)
        {
            nfloat wScale = target.Width / source.Width;
            nfloat hScale = target.Height / source.Height;

            return AutoScaleMode == AutoScaleModes.AutoWidth ? (wScale < hScale ? hScale : wScale) : (wScale < hScale ? wScale : hScale);
        }

        private void CenterImageInScrollView()
        {
            // Calculate the left/right paddings
            nfloat left = 0;
            if (_scrollView.ContentSize.Width < _scrollView.Bounds.Size.Width)
            {
                left = (_scrollView.Bounds.Size.Width - _scrollView.ContentSize.Width) * 0.5f;
            }

            // Calculate the top/bottom paddings
            nfloat top = 0;
            if (_scrollView.ContentSize.Height < _scrollView.Bounds.Size.Height)
            {
                top = (_scrollView.Bounds.Size.Height - _scrollView.ContentSize.Height) * 0.5f;
            }

            // Apply the paddings/insets
            _scrollView.ContentInset = new UIEdgeInsets(top, left, top, left);
        }

        private void ZoomToPoint(CGPoint zoomPoint, nfloat scale, bool animated)
        {
            // Normalize current content size back to content scale of 1.0f
            var contentSize = new CGSize(_scrollView.ContentSize.Width / _scrollView.ZoomScale, _scrollView.ContentSize.Height / _scrollView.ZoomScale);

            // Translate the zoom point to relative to the content rect
            zoomPoint.X = (zoomPoint.X / this.Bounds.Size.Width) * contentSize.Width;
            zoomPoint.Y = (zoomPoint.Y / this.Bounds.Size.Height) * contentSize.Height;

            // Derive the size of the region to zoom to
            var zoomSize = new CGSize(this.Bounds.Size.Width / scale, this.Bounds.Size.Height / scale);

            var zoomRect = new CGRect(zoomPoint.X - zoomSize.Width / 2.0f,
                               zoomPoint.Y - zoomSize.Height / 2.0f,
                               zoomSize.Width,
                               zoomSize.Height);

            // Apply the resize
            _scrollView.ZoomToRect(zoomRect, animated);
        }

        #endregion
    }
}