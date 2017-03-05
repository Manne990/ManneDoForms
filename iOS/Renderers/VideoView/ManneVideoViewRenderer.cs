using CoreGraphics;
using ManneDoForms.Components.VideoView;
using ManneDoForms.iOS.Renderers.VideoView;
using MediaPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            // Check if we have a control
            if (e.NewElement == null)
            {
                return;
            }

            // Get the forms view
            _view = (ManneVideoView)e.NewElement;

            // Create the movie player controller
            _controller = new MPMoviePlayerController();

            // Create the native view
            SetNativeControl(_controller.View);

            // Try to play
            LoadAndPlay();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ManneVideoView.Size))
            {
                if (Control == null)
                {
                    return;
                }

                Control.Superview.Frame = new CGRect(Control.Superview.Frame.X, Control.Superview.Frame.Y, _view.Size.Width, _view.Size.Height);
            }

            if (e.PropertyName == nameof(ManneVideoView.Url))
            {
                LoadAndPlay();
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Dispose all components
            _controller?.Stop();
            _controller?.Dispose();
            _controller = null;

            base.Dispose(disposing);
        }

        #endregion

        // ----------------------------------------------

        #region Private Methods

        private void LoadAndPlay()
        {
            // Check the URL
            if (string.IsNullOrWhiteSpace(_view.Url))
            {
                return;
            }

            var url = string.Empty;
            if (_view.Url.StartsWith("file://", System.StringComparison.CurrentCulture) == false)
            {
                url = $"file://{_view.Url}";
            }
            else
            {
                url = _view.Url;
            }

            // Play the movie
            _controller.ContentUrl = Foundation.NSUrl.FromString(url);
            _controller.PrepareToPlay();
            _controller.Play();
        }

        #endregion
    }
}