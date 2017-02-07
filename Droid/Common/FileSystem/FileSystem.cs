using System.IO;
using ManneDoForms.Common;
using ManneDoForms.Droid.Common.FileSystem;

[assembly: Xamarin.Forms.Dependency(typeof(FileSystem))]
namespace ManneDoForms.Droid.Common.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);

            return File.Exists(path);
        }

        public bool SaveBinaryFile(string fileName, Stream data)
        {
            var context = Android.App.Application.Context;
            Stream stream = context.OpenFileOutput(fileName, Android.Content.FileCreationMode.Private);

            return SaveBinaryResource(stream, data);
        }

        public byte[] LoadBinary(string fileName)
        {
            try
            {
                return File.ReadAllBytes(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName));
            }
            catch
            {
                return null;
            }
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
        }

        private bool SaveBinaryResource(Stream stream, Stream data)
        {
            data.CopyTo(stream);

            stream.Close();

            return true;
        }
    }
}