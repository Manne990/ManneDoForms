using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void ShowPaintView(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PaintViewPage());
        }

        private void ShowRangeSliderView(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new RangeSliderViewPage());
        }

        private void ShowTableView(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new TableViewPage());
        }
    }
}