using System.IO;

namespace ManneDoForms.Common.Filesystem
{
    public interface IFileSystem
    {
        bool FileExists(string fileName);
        bool SaveBinaryFile(string fileName, Stream data);
        byte[] LoadBinary(string fileName);
        string GetFilePath(string fileName);
    }
}