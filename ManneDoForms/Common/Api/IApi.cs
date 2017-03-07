using System.Threading.Tasks;

namespace ManneDoForms.Common.Api
{
    public interface IApi
    {
        Task DownloadAndSaveFile(string url, string filename);
    }
}