using System;
using System.Net.Http;
using System.Threading.Tasks;
using ManneDoForms.Common;
using Xamarin.Forms;
using XLabs.Ioc;

namespace ManneDoForms.Samples
{
    public partial class PdfPage : ContentPage
    {
        public PdfPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadPdf();
        }

        private async Task LoadPdf()
        {
            var url = "http://www.pdf995.com/samples/pdf.pdf";
            var localFilename = "pdf.pdf";

            var fileSystem = DependencyService.Get<IFileSystem>();
            if (fileSystem.FileExists(localFilename) == false)
            {
                var api = Resolver.Resolve<IApi>();

                await api.DownloadAndSaveFile(url, localFilename);
            }

            pdfView.LocalFilePath = fileSystem.GetFilePath(localFilename);
        }
    }
}