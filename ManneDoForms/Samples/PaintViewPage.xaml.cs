using System;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class PaintViewPage : ContentPage
    {
        public PaintViewPage()
        {
            InitializeComponent();
        }

        private void BlackButtonClicked(object sender, EventArgs args)
        {
            paintView.PaintColor = Color.Black;
        }

        private void WhiteButtonClicked(object sender, EventArgs args)
        {
            paintView.PaintColor = Color.White;
        }

        private void RedButtonClicked(object sender, EventArgs args)
        {
            paintView.PaintColor = Color.Red;
        }

        private void GreenButtonClicked(object sender, EventArgs args)
        {
            paintView.PaintColor = Color.Green;
        }

        private void BlueButtonClicked(object sender, EventArgs args)
        {
            paintView.PaintColor = Color.Blue;
        }

        private void LineWidthValueChanged(object sender, ValueChangedEventArgs e)
        {
            paintView.LineWidth = (float)e.NewValue;
        }
    }
}