using System;
using CoreAnimation;
using CoreGraphics;
using ManneDoForms.Components.ProgressAndSpinner;
using ManneDoForms.iOS.Renderers.ProgressAndSpinner;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer (typeof(CircularProgressView), typeof(CircularProgressViewRenderer))]
namespace ManneDoForms.iOS.Renderers.ProgressAndSpinner
{
    public class CircularProgressViewRenderer : ImageRenderer
    {
        private nfloat _startAngle = -((nfloat)Math.PI / 2f);
        private CircularProgressView _formsView;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _formsView = (CircularProgressView)e.NewElement;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName == "Progress")
            {
                UpdateProgressView(_formsView.Progress);
            }
        }

        private void UpdateProgressView(float progress)
        {
            var imageView = this.Control;

            if(imageView.Image == null)
            {
                return;
            }

            nfloat endAngle = (nfloat)((Math.PI * 2f * progress) + _startAngle);

            var maskImage = CreatePieSegment(imageView.Frame.Size, endAngle);

            MaskImageWithImage(imageView, maskImage);
        }

        private UIImage CreatePieSegment(CGSize size, nfloat endAngle)
        {
            // Add the arc
            var arc = new CGPath();

            arc.MoveToPoint(size.Width / 2.0f, size.Height / 2.0f);
            arc.AddLineToPoint(size.Width / 2.0f, 0);
            arc.AddArc(size.Width / 2.0f, size.Height / 2.0f, size.Width / 2.0f, _startAngle, endAngle, false);
            arc.AddLineToPoint(size.Width / 2.0f, size.Height / 2.0f);

            // Stroke the arc
            UIGraphics.BeginImageContextWithOptions(size, false, 0);

            var context = UIGraphics.GetCurrentContext();

            context.AddPath(arc);
            context.SetFillColor(UIColor.FromRGBA(0f, 0f, 0f, 1f).CGColor);
            context.FillPath();

            // Get the mask image
            var image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return image;
        }

        private void MaskImageWithImage(UIImageView imgView, UIImage maskImage)
        {
            var aMaskLayer = new CALayer();

            aMaskLayer.Frame = new CGRect(0, 0, imgView.Frame.Size.Width, imgView.Frame.Size.Height);
            aMaskLayer.Contents = maskImage.CGImage;

            imgView.Layer.Mask = aMaskLayer;
        }
    }
}