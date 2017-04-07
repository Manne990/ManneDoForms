using System.IO;
using System.Threading.Tasks;

namespace ManneDoForms.Common.Filesystem
{
    public interface IFileSystem
    {
        bool FileExists(string fileName);
        void SaveBinaryFile(string fileName, Stream data, bool flagAsNotBackup = true);
        void SaveBinaryFile(string fileName, byte[] data, bool flagAsNotBackup = true);
        byte[] LoadBinary(string fileName);
        Task<byte[]> LoadBinaryAsync(string fileName, bool fullPath = false);
        string GetFilePath(string fileName);
    }
}