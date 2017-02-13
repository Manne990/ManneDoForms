using ManneDoForms.Components.PdfViewer;
using ManneDoForms.iOS.Renderers.PdfViewer;
using mTouchPDFReader.Library.Views.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MannePdfView), typeof(MannePdfViewRenderer))]
namespace ManneDoForms.iOS.Renderers.PdfViewer
{
    public class MannePdfViewRenderer : ViewRenderer
    {
        #region Private Members

        private MannePdfView _view;
        private UIView _nativeView;

        #endregion

        // ----------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            // Check if we have a control
            if (e.NewElement == null)
            {
                return;
            }

            // Get the forms view
            _view = (MannePdfView)e.NewElement;

            // Create the native view
            var docViewController = new DocumentVC(1, "Name", _view.Url.Replace("file://", string.Empty));

            _nativeView = docViewController.View;

            SetNativeControl(_nativeView);
        }

        protected override void Dispose(bool disposing)
        {
            // Dispose all components
            if (_nativeView != null)
            {
                _nativeView.Dispose();
                _nativeView = null;
            }

            _view = null;

            base.Dispose(disposing);
        }

        #endregion
    }
}