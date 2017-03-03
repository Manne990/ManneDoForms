using System;
using System.IO;
using Android.Content;
using ManneDoForms.Components.PdfViewer;
using ManneDoForms.Droid.Renderers.PdfViewer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MannePdfView), typeof(MannePdfViewRenderer))]
namespace ManneDoForms.Droid.Renderers.PdfViewer
{
    //REMARK: Add permission into your manifest file to read and write to/from external storage
    public class MannePdfViewRenderer : ViewRenderer
    {
        #region Private Members

        private MannePdfView _view;

        #endregion

        // ----------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            _view = (MannePdfView)e.NewElement;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(_view.LocalFilePath))
            {
                // Open the file with an intent
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var pdfFilePath = Path.Combine(documentsPath, _view.LocalFilePath.Replace("file://", ""));
                var result = OpenFileWithIntent(pdfFilePath);

                if (result)
                {
                    _view.SetFinished();
                }
                else
                {
                    _view.SetError();
                }
            }
        }

        #endregion

        // ----------------------------------------------

        #region Private Methods

        public bool OpenFileWithIntent(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var externalStorageState = Android.OS.Environment.ExternalStorageState;
            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, $"{Guid.NewGuid().ToString()}.pdf");

            File.WriteAllBytes(externalPath, bytes);

            var file = new Java.IO.File(externalPath);
            file.SetReadable(true);

            var uri = Android.Net.Uri.FromFile(file);

            var intent = new Intent(Intent.ActionView);

            intent.SetDataAndType(uri, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Forms.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("No Application Available to View PDF");
                return false;
            }

            return true;
        }

        #endregion
    }
}