using System.ComponentModel;
using ManneDoForms.Components.PhotoViewer.View;
using ManneDoForms.iOS.Renderers.PhotoViewer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedBox), typeof(RoundedBoxRenderer))]
namespace ManneDoForms.iOS.Renderers.PhotoViewer
{
    public class RoundedBoxRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                Layer.MasksToBounds = true;
                UpdateCornerRadius(Element as RoundedBox);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundedBox.CornerRadiusProperty.PropertyName)
            {
                UpdateCornerRadius(Element as RoundedBox);
            }
        }

        private void UpdateCornerRadius(RoundedBox box)
        {
            Layer.CornerRadius = (float)box.CornerRadius;
        }
    }
}