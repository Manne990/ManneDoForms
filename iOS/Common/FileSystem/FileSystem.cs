using Foundation;
using System.IO;
using ManneDoForms.iOS.Common.FileSystem;
using ManneDoForms.Common.Filesystem;
using System;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FileSystem))]
namespace ManneDoForms.iOS.Common.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);
            var filePath = docUrl.AbsoluteString.Replace("file://", "");

            return File.Exists(filePath);
        }

        public void SaveBinaryFile(string fileName, Stream data, bool flagAsNotBackup = true)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);
            SaveBinaryResource(docUrl.AbsoluteString.Replace("file://", ""), data, flagAsNotBackup);
        }

        public void SaveBinaryFile(string fileName, byte[] data, bool flagAsNotBackup = true)
        {
        	var docUrl = applicationDocumentsDirectory().Append(fileName, false);
        	var filePath = docUrl.AbsoluteString.Replace("file://", "");

        	File.WriteAllBytes(filePath, data);
        	File.SetLastAccessTime(filePath, DateTime.Now);

        	if (flagAsNotBackup)
        	{
        		NSFileManager.SetSkipBackupAttribute(filePath, true);
        	}
        }

        public byte[] LoadBinary(string fileName)
        {
            // if file path already is a absolute path
            if (File.Exists(fileName))
            {
                return LoadBinaryResource(fileName);
            }
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);
            var filePath = docUrl.AbsoluteString.Replace("file://", "");
            return LoadBinaryResource(filePath);
        }

        public async Task<byte[]> LoadBinaryAsync(string fileName, bool fullPath = false)
        {
        	var filePath = fileName;

        	if (fullPath == false)
        	{
        		var docUrl = applicationDocumentsDirectory().Append(fileName, false);
        		filePath = docUrl.AbsoluteString.Replace("file://", "");
        	}

        	if (File.Exists(filePath) == false)
        	{
        		return null;
        	}

        	File.SetLastAccessTime(filePath, DateTime.Now);

        	byte[] result;
        	const int BufferSize = 4096;

        	using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, BufferSize, true))
        	{
        		result = new byte[stream.Length];
        		await stream.ReadAsync(result, 0, result.Length);
        	}

        	return result;
        }

        public string GetFilePath(string fileName)
        {
            var docUrl = applicationDocumentsDirectory().Append(fileName, false);

            return docUrl.AbsoluteString;
        }

        NSUrl applicationDocumentsDirectory()
        {
            return NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
        }

        private void SaveBinaryResource(string filePath, Stream data, bool flagAsNotBackup = true)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                data.CopyTo(fileStream);
            }

            if (flagAsNotBackup)
            {
                NSFileManager.SetSkipBackupAttribute(filePath, true);
            }
        }

        private byte[] LoadBinaryResource(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            return File.ReadAllBytes(filePath);
        }
    }
}