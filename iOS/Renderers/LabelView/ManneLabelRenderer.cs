using System;
using CoreGraphics;
using ManneDoForms.Components.LabelView;
using ManneDoForms.iOS.Renderers.LabelView;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ManneLabel), typeof(ManneLabelRenderer))]
namespace ManneDoForms.iOS.Renderers.LabelView
{
    public class ManneLabelRenderer : LabelRenderer
    {
        // Private Members
        private ManneLabel _formsView;
        private CustomLabel _nativeView;


        // -----------------------------------------------------------------------------

        // Overrides
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            if (e.NewElement != null)
            { 
                _formsView = e.NewElement as ManneLabel;

                _nativeView = new CustomLabel();

                _nativeView.SetInset(_formsView.Padding);

                SetNativeControl(_nativeView);
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ManneLabel.Padding))
            {
                _nativeView.SetInset(_formsView.Padding);
                return;
            }
        }

        private class CustomLabel : UILabel
        {
            private UIEdgeInsets _inset;

            public override void DrawText(CGRect rect)
            {
                base.DrawText(_inset.InsetRect(rect));
            }

            public void SetInset(Thickness inset)
            { 
                _inset = new UIEdgeInsets((nfloat)inset.Top, (nfloat)inset.Left, (nfloat)inset.Bottom, (nfloat)inset.Right);

                SetNeedsDisplay();
            }
        }
    }
}