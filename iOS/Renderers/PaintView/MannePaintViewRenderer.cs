using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(ManneDoForms.Components.PaintView.MannePaintView), typeof(ManneDoForms.iOS.Renderers.PaintView.MannePaintViewRenderer))]
namespace ManneDoForms.iOS.Renderers.PaintView
{
	public class MannePaintViewRenderer : ImageRenderer
	{
		#region Private Members

		private ManneDoForms.Components.PaintView.MannePaintView _formsView;

		#endregion

		// ---------------------------------------------------------

		#region Overrides

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				_formsView = (ManneDoForms.Components.PaintView.MannePaintView)e.NewElement;

				this.Control.UserInteractionEnabled = true;
				this.Control.MultipleTouchEnabled = true;
				this.Control.Image = new UIImage();
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			// Loop all touch positions
			foreach(var touch in touches)
			{
				// Get the current point
				var currentPoint = ((UITouch)touch).LocationInView(this);

				// Draw
				DrawLine(currentPoint, currentPoint);
			}
		}

		public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			// Loop all touch positions
			foreach(var touch in touches)
			{
				// Get the current and last points
				var points = new NSMutableDictionary();

				points.Add(new NSString("currentPoint"), NSValue.FromCGPoint(((UITouch)touch).LocationInView(this)));
				points.Add(new NSString("lastPoint"), NSValue.FromCGPoint(((UITouch)touch).PreviousLocationInView(this)));

				// Draw
				PerformSelector(new Selector("DrawLine:"), points, 0.0f);
			}
		}

		#endregion

		// ---------------------------------------------------------

		#region Private Methods

		[Export("DrawLine:")]
		private void DrawLine(NSDictionary dict)
		{
			var point1 = ((NSValue)dict["lastPoint"]).CGPointValue;
			var point2 = ((NSValue)dict["currentPoint"]).CGPointValue;

			DrawLine(point1, point2);
		}

		private void DrawLine(CGPoint point1, CGPoint point2)
		{
			var size = this.Frame.Size;

			UIGraphics.BeginImageContext(size);

			this.Control.Image.Draw(new CGRect(0, 0, size.Width, size.Height));

			var context = UIGraphics.GetCurrentContext();

			context.SetLineCap(CGLineCap.Round);
			context.SetLineWidth((nfloat)_formsView.LineWidth);
			context.SetStrokeColor(_formsView.PaintColor.ToCGColor());
			context.BeginPath();
			context.MoveTo(point1.X - 0.5f, point1.Y - 0.5f);
			context.AddLineToPoint(point2.X - 0.5f, point2.Y - 0.5f);
			context.StrokePath();

			this.Control.Image = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();
		}

		#endregion
	}
}