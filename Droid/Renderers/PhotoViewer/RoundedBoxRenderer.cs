using System;
using System.ComponentModel;
using Android.Graphics;
using ManneDoForms.Common.PhotoViewer.View;
using ManneDoForms.Droid.Renderers.PhotoViewer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedBox), typeof(RoundedBoxRenderer))]
namespace ManneDoForms.Droid.Renderers.PhotoViewer
{
    public class RoundedBoxRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            SetWillNotDraw(false);

            Invalidate();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundedBox.CornerRadiusProperty.PropertyName)
            {
                Invalidate();
            }
        }

        public override void Draw(Canvas canvas)
        {
            var box = Element as RoundedBox;
            var rect = new Rect();
            var androidColor = box.BackgroundColor.ToAndroid();
            var paint = new Paint()
            {
                AntiAlias = true,
            };

            paint.SetARGB(Convert.ToInt32(box.Opacity * 255), (int)androidColor.R, (int)androidColor.G, (int)androidColor.B);

            GetDrawingRect(rect);

            var radius = (float)(rect.Width() / box.Width * box.CornerRadius);

            canvas.DrawRoundRect(new RectF(rect), radius, radius, paint);
        }
    }
}