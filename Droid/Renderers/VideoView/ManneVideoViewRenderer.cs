using System;
using Android.Media;
using Android.Views;
using Android.Widget;
using ManneDoForms.Components.VideoView;
using ManneDoForms.Droid.Renderers.VideoView;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ManneVideoView), typeof(ManneVideoViewRenderer))]
namespace ManneDoForms.Droid.Renderers.VideoView
{
    public class ManneVideoViewRenderer : ViewRenderer<ManneVideoView, Android.Widget.RelativeLayout>, ISurfaceHolderCallback
    {
        #region Private Members

        private Android.Widget.VideoView _videoview;
        private ManneVideoView _view;
        private MediaController _controller;
        private Size _videoSizePortrait;
        private Size _videoSizeLandscape;

        #endregion

        // ----------------------------------------------

        #region ISurfaceHolderCallback Implementation

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {

        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            holder.RemoveCallback(this);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (_view.Url.StartsWith("http://") || _view.Url.StartsWith("https://"))
            {
                _videoview.SetVideoURI(Android.Net.Uri.Parse(_view.Url));
            }
            else
            {
                _videoview.SetVideoPath(_view.Url);
            }

            _videoview.Start();

            _controller.Show(3000);
        }

        #endregion

        // ----------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<ManneVideoView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            // Get the Forms control
            _view = e.NewElement;

            // Check for an URL
            if (string.IsNullOrWhiteSpace(_view.Url))
            {
                return;
            }

            // Init Sizes
            SetVideoDimensions(new Size(710, 400));

            // Get the video dimensions and calculate sizes
            GetVideoDimensions(_view.Url);

            // Create the container
            var layout = new Android.Widget.RelativeLayout(Forms.Context);

            var layoutParams = new Android.Widget.RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

            layout.LayoutParameters = layoutParams;

            // Create the VideoView
            _videoview = new Android.Widget.VideoView(Forms.Context);

            var videoLayoutParams = new Android.Widget.RelativeLayout.LayoutParams(0, 0);

            videoLayoutParams.AddRule(LayoutRules.CenterInParent);

            _videoview.LayoutParameters = videoLayoutParams;

            // Create the Media Controller
            _controller = new MediaController(Forms.Context, true);

            _videoview.SetMediaController(_controller);
            _controller.SetMediaPlayer(_videoview);
            _controller.SetAnchorView(_videoview);

            // Set the View View as the native control for this renderer
            layout.AddView(_videoview);

            SetNativeControl(layout);

            // Receive Callbacks for the surface
            _videoview.Holder.AddCallback(this);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            // Update the video size from the new orientation
            Android.Widget.RelativeLayout.LayoutParams videoLayoutParams;

            if (w < h)
            {
                // Portrait
                videoLayoutParams = new Android.Widget.RelativeLayout.LayoutParams((int)_videoSizePortrait.Width, (int)_videoSizePortrait.Height);
            }
            else
            {
                // Landscape
                videoLayoutParams = new Android.Widget.RelativeLayout.LayoutParams((int)_videoSizeLandscape.Width, (int)_videoSizeLandscape.Height);
            }

            videoLayoutParams.AddRule(LayoutRules.CenterInParent);

            _videoview.LayoutParameters = videoLayoutParams;
        }

        protected override void Dispose(bool disposing)
        {
            _videoview.StopPlayback();
            _videoview.Dispose();
            _videoview = null;

            _controller.Dispose();
            _controller = null;

            _view = null;
        }

        #endregion

        // ----------------------------------------------

        #region Private Methods

        private void SetVideoDimensions(Size videoDimensions)
        {
            if (videoDimensions.Width < 1 || videoDimensions.Height < 1)
            {
                return;
            }

            var metrics = Resources.DisplayMetrics;
            if (metrics.WidthPixels < metrics.HeightPixels)
            {
                // Portrait
                _videoSizePortrait = ExpandToBound(videoDimensions, new Size(metrics.WidthPixels, metrics.HeightPixels));
                _videoSizeLandscape = ExpandToBound(videoDimensions, new Size(metrics.HeightPixels, metrics.WidthPixels));
            }
            else
            {
                // Landscape
                _videoSizePortrait = ExpandToBound(videoDimensions, new Size(metrics.HeightPixels, metrics.WidthPixels));
                _videoSizeLandscape = ExpandToBound(videoDimensions, new Size(metrics.WidthPixels, metrics.HeightPixels));
            }
        }

        private Size ExpandToBound(Size video, Size screen)
        {
            double widthScale = 0, heightScale = 0;

            if (video.Width != 0)
            {
                widthScale = (double)screen.Width / (double)video.Width;
            }

            if (video.Height != 0)
            {
                heightScale = (double)screen.Height / (double)video.Height;
            }

            double scale = Math.Min(widthScale, heightScale);

            Size result = new Size((int)(video.Width * scale), (int)(video.Height * scale));

            return result;
        }

        private void GetVideoDimensions(string url)
        {
            var player = new MediaPlayer();

            player.Prepared += (sender, args) =>
            {
                SetVideoDimensions(new Size(player.VideoWidth, player.VideoHeight));
            };

            player.SetDataSource(_view.Url);
            player.PrepareAsync();
        }

        #endregion
    }
}