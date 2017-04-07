using System;
using System.Net.Http;
using System.Threading.Tasks;
using ManneDoForms.Common.Filesystem;
using Xamarin.Forms;

namespace ManneDoForms.Common.Api
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

        public async Task<byte[]> DownloadFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                System.Diagnostics.Debug.WriteLine("DownloadFile: url is empty!");
                return null;
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    return await httpClient.GetByteArrayAsync(url);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DownloadFile: Error for file '{0}', {1}", url, ex.Message);
                return null;
            }
        }
    }
}