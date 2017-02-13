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
        #region Private Members

        private MannePdfView _view;

        #endregion

        // ----------------------------------------------

        #region Overrides

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
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

            if (e.PropertyName == nameof(_view.Filename))
            {
                // Open the file with an intent
                //var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //var pdfFilePath = Path.Combine(documentsPath, _view.Filename.Replace("file://", ""));
                //var result = OpenFileWithIntent(pdfFilePath);
                var result = OpenFileWithIntent("file:///android_asset/example.pdf");

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

        private bool OpenFileWithIntent(string filePath)
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

            //try
            //{
            //    var file = new Java.IO.File(filePath);
            //    var uri = FileProvider.GetUriForFile(Context, "se.idoapps.mannedoforms.manne_do_forms.fileprovider", file);

            //    //REMARK: In the AndroidManifest.xml file the id above "se.idoapps.mannedoforms.manne_do_forms.fileprovider" must be present as a provider.

            //    var intent = new Intent();

            //    intent.SetAction(Intent.ActionView);
            //    intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            //    intent.SetDataAndType(uri, "application/pdf");

            //    Context.StartActivity(intent);

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        #endregion
    }
}