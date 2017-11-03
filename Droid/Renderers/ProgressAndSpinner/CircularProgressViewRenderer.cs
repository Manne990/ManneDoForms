using Android.Graphics;
using ManneDoForms.Components.ProgressAndSpinner;
using ManneDoForms.Droid.Renderers.ProgressAndSpinner;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CircularProgressView), typeof(CircularProgressViewRenderer))]
namespace ManneDoForms.Droid.Renderers.ProgressAndSpinner
{
    public class CircularProgressViewRenderer : ImageRenderer
    {
        private float _startAngle = 270;
        private float _endAngle = 630;
        private CircularProgressView _formsView;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _formsView = (CircularProgressView)e.NewElement;
                SetWillNotDraw(false);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Progress")
            {
                UpdateProgressView(_formsView.Progress);
            }
        }

        private void UpdateProgressView(float progress)
        {
            var imageView = this.Control;

            if (imageView.Drawable == null)
            {
                return;
            }

            CalculateEndAngle(progress);
            Invalidate();
        }

        private void CalculateEndAngle(float progress)
        {
            _endAngle = (float)((360 * progress) + _startAngle);
        }

        protected override void OnDraw(Canvas canvas)
        {
            var rect = new RectF(0, 0, canvas.Width, canvas.Height);
            var path = new Path();
            path.MoveTo(rect.CenterX(), rect.CenterY());
            path.AddArc(rect, _startAngle, _endAngle - _startAngle);
            path.LineTo(rect.CenterX(), rect.CenterY());
            path.Close();
            canvas.ClipPath(path);

            base.OnDraw(canvas);
        }
    }
}