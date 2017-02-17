using Android.Graphics.Drawables;
using Android.Widget;
using ManneDoForms.Components.DropDownView;
using ManneDoForms.Droid.Common;
using ManneDoForms.Droid.Renderers.DropDownView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ManneDropDownView), typeof(ManneDropDownViewRenderer))]
namespace ManneDoForms.Droid.Renderers.DropDownView
{
    public class ManneDropDownViewRenderer : ViewRenderer
    {
        // Private Members
        private ManneDropDownView _formsView;
        private ArrayAdapter<string> _spinnerAdapter;
        private Spinner _marketSpinner;


        // -----------------------------------------------------------------------------

        // Overrides
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                // Init
                _formsView = e.NewElement as ManneDropDownView;

                // Create Native View
                _marketSpinner = new Spinner(Context);

                _marketSpinner.SetBackgroundResource(Resource.Drawable.spinner_background);
                _marketSpinner.SetPadding(10 * (int)AndroidDevice.DisplayMetrics.Density, 0, 0, 0);
                _marketSpinner.ItemSelected += ItemSelected;

                SetNativeControl(_marketSpinner);

                // Handle Spinner
                UpdateDropDownList();
                SelectItem();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ManneDropDownView.ItemsSource))
            {
                UpdateDropDownList();
                return;
            }

            if (e.PropertyName == nameof(ManneDropDownView.SelectedItem))
            {
                SelectItem();
                return;
            }

            if (e.PropertyName == nameof(ManneDropDownView.BackgroundColor))
            {
                Control.Background = new ColorDrawable(_formsView.BackgroundColor.ToAndroid());
                return;
            }
        }


        // -----------------------------------------------------------------------------

        // Private Methods
        private void ItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            var item = _formsView.ItemsSource[itemSelectedEventArgs.Position];

            _formsView?.SetSelected(item);
        }

        private void SelectItem()
        {
            var index = _formsView?.ItemsSource?.IndexOf(_formsView.SelectedItem) ?? -1;

            if (index > 0)
            {
                _marketSpinner.SetSelection(index, false);
            }
        }

        private void UpdateDropDownList()
        {
            if (_formsView?.ItemsSource == null)
            {
                return;
            }

            _spinnerAdapter = new ArrayAdapter<string>(Context, Resource.Layout.SpinnerItemLayout, _formsView.ItemsSource);

            _spinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            _marketSpinner.Adapter = _spinnerAdapter;
        }
    }
}