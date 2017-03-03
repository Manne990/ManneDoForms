using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManneDoForms.Common
{
    public class Api : IApi
    {
        public async Task DownloadAndSaveFile(string url, string filename)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

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