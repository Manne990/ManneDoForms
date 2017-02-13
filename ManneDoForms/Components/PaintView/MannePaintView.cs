using Xamarin.Forms;

namespace ManneDoForms.Components.PaintView
{
	public class MannePaintView : Image
	{
		public MannePaintView()
		{
		}

		public static readonly BindableProperty PaintColorProperty = BindableProperty.Create(nameof(PaintColor), typeof(Color), typeof(MannePaintView), Color.Black);
		public Color PaintColor
		{
			get { return (Color)GetValue(PaintColorProperty); }
			set { SetValue(PaintColorProperty, value); }
		}

		public static readonly BindableProperty LineWidthProperty = BindableProperty.Create(nameof(LineWidth), typeof(float), typeof(MannePaintView), 10.0f);
		public float LineWidth
		{
			get { return (float)GetValue(LineWidthProperty); }
			set { SetValue(LineWidthProperty, value); }
		}

		public void Clear()
		{
			this.Source = null;
		}
	}
}