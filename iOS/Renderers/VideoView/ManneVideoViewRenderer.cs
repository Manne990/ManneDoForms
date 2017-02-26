using CoreGraphics;
using ManneDoForms.Components.VideoView;
using ManneDoForms.iOS.Renderers.VideoView;
using MediaPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinForms = Xamarin.Forms;

[assembly: ExportRenderer(typeof(ManneVideoView), typeof(ManneVideoViewRenderer))]
namespace ManneDoForms.iOS.Renderers.VideoView
{
    public class ManneVideoViewRenderer : ViewRenderer
    {
        #region Private Members

        private ManneVideoView _view;
        private MPMoviePlayerController _controller;

        #endregion

        // ----------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<XamarinForms.View> e)
        {
            base.OnElementChanged(e);

            // Check if we have a control
            if (e.NewElement == null)
            {
                return;
            }

            // Get the forms view
            _view = (ManneVideoView)e.NewElement;

            // Check if we have an URL
            if (string.IsNullOrWhiteSpace(_view.Url))
            {
                return;
            }

            // Create the movie player controller
            _controller = new MPMoviePlayerController(Foundation.NSUrl.FromString(_view.Url));

            // Create the native view
            SetNativeControl(_controller.View);

            // Play the movie
            _controller.Play();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Size")
            {
                Control.Superview.Frame = new CGRect(this.Control.Superview.Frame.X, this.Control.Superview.Frame.Y, _view.Size.Width, _view.Size.Height);
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Dispose all components
            _controller.Stop();
            _controller.Dispose();
            _controller = null;

            base.Dispose(disposing);
        }

        #endregion
    }
}