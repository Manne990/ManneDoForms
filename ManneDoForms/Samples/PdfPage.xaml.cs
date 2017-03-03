using System;
using System.Net.Http;
using System.Threading.Tasks;
using ManneDoForms.Common;
using Xamarin.Forms;

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
                await DownloadAndSaveFile(url, localFilename);
            }

            pdfView.LocalFilePath = fileSystem.GetFilePath(localFilename);
        }

        private async Task DownloadAndSaveFile(string url, string filename)
        {
            var httpClient = new HttpClient();
            var fileSystem = DependencyService.Get<IFileSystem>();

            try
            {
                // Download file
                var streamAsync = await httpClient.GetStreamAsync(url);

                // Save the file
                fileSystem.SaveBinaryFile(filename, streamAsync);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Download failed! {0}", ex.Message);
            }
        }
    }
}