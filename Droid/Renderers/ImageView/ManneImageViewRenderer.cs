using ManneDoForms.Components.ImageView;
using ManneDoForms.Droid.Renderers.ImageView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ManneImageView), typeof(ManneImageViewRenderer))]
namespace ManneDoForms.Droid.Renderers.ImageView
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
                // Init
                _formsView = e.NewElement as ManneImageView;


            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}