using System.Threading.Tasks;

namespace ManneDoForms.Common
{
    public interface IApi
    {
        Task DownloadAndSaveFile(string url, string filename);
    }
}