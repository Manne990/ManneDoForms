using ManneDoForms.Components.LabelView;
using ManneDoForms.Droid.Renderers.LabelView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ManneLabel), typeof(ManneLabelRenderer))]
namespace ManneDoForms.Droid.Renderers.LabelView
{
    public class ManneLabelRenderer : LabelRenderer
    {
        // Private Members
        private ManneLabel _formsView;


        // -----------------------------------------------------------------------------

        // Overrides
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            if (e.NewElement != null)
            {
                _formsView = e.NewElement as ManneLabel;
            }

            base.OnElementChanged(e);

            Control.SetPadding((int)_formsView.Padding.Left, (int)_formsView.Padding.Top, (int)_formsView.Padding.Right, (int)_formsView.Padding.Bottom);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ManneLabel.Padding))
            {
                Control.SetPadding((int)_formsView.Padding.Left, (int)_formsView.Padding.Top, (int)_formsView.Padding.Right, (int)_formsView.Padding.Bottom);
                return;
            }
        }
    }
}