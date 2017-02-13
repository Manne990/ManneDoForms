using System.IO;
using Android.Content;
using Android.Support.V4.Content;
using ManneDoForms.Components.PdfViewer;
using ManneDoForms.Droid.Renderers.PdfViewer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MannePdfView), typeof(MannePdfViewRenderer))]
namespace ManneDoForms.Droid.Renderers.PdfViewer
{
    public class MannePdfViewRenderer : ViewRenderer
    {
        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            // Get the forms view
            var view = (MannePdfView)e.NewElement;

            // Open the file with an intent
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var pdfFilePath = Path.Combine(documentsPath, view.Url.Replace("file://", ""));
            var result = OpenFileWithIntent(pdfFilePath);

            if (result)
            {
                view.SetFinished();
            }
            else
            {
                view.SetError();
            }
        }

        #endregion

        // ----------------------------------------------

        #region Private Methods

        private bool OpenFileWithIntent(string filePath)
        {
            try
            {
                var file = new Java.IO.File(filePath);
                var uri = FileProvider.GetUriForFile(Context, "se.idoapps.mannedoforms.manne_do_forms.fileprovider", file);

                //REMARK: In the AndroidManifest.xml file the id above "se.idoapps.mannedoforms.manne_do_forms.fileprovider" must be present as a provider.

                var intent = new Intent();

                intent.SetAction(Intent.ActionView);
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                intent.SetDataAndType(uri, "application/pdf");

                Context.StartActivity(intent);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}