using System;
using System.IO;
using System.Threading.Tasks;
using ManneDoForms.Common.Filesystem;
using ManneDoForms.Droid.Common.FileSystem;

[assembly: Xamarin.Forms.Dependency(typeof(FileSystem))]
namespace ManneDoForms.Droid.Common.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);

            return File.Exists(path);
        }

        public void SaveBinaryFile(string fileName, Stream data, bool flagAsNotBackup = true)
        {
            var context = Android.App.Application.Context;
            Stream stream = context.OpenFileOutput(fileName, Android.Content.FileCreationMode.Private);

            SaveBinaryResource(stream, data);
        }

        public void SaveBinaryFile(string fileName, byte[] data, bool flagAsNotBackup = true)
        {
        	throw new NotImplementedException();
        }

        public byte[] LoadBinary(string fileName)
        {
            try
            {
                return File.ReadAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName));
            }
            catch
            {
                return null;
            }
        }

        public Task<byte[]> LoadBinaryAsync(string fileName, bool fullPath = false)
        {
        	throw new NotImplementedException();
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }

        private void SaveBinaryResource(Stream stream, Stream data)
        {
            data.CopyTo(stream);

            stream.Close();
        }
    }
}