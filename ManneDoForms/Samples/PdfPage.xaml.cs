using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class PdfPage : ContentPage
    {
        public PdfPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            pdfView.Filename = "example.pdf";
        }
    }
}