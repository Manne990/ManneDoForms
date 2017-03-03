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

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            _view = (MannePdfView)e.NewElement;

            _nativeView = new UIView();

            SetNativeControl(_nativeView);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(_view.LocalFilePath))
            {
                try
                {
                    var docViewController = new DocumentVC(1, "Document", _view.LocalFilePath.Replace("file://", string.Empty));

                    if (docViewController.View == null)
                    {
                        return;
                    }

                    _nativeView = docViewController.View;

                    SetNativeControl(_nativeView);

                    _view.SetFinished();
                }
                catch
                {
                    _view.SetError();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
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