using Android.App;
using Android.Content;
using Java.Lang;
using ManneDoForms.Common.Sample;
using ManneDoForms.Droid.Common;
using ManneDoForms.Droid.Samples;

[assembly: Xamarin.Forms.Dependency(typeof(RepeaterViewSample))]
namespace ManneDoForms.Droid.Common
{
    public class RepeaterViewSample : IRepeaterViewSample
    {
        public void Show()
        {
            var concreteTypeJava = Class.FromType(typeof(RepeaterViewActivity));
            var intent = new Intent(Application.Context, concreteTypeJava);

            intent.SetFlags(ActivityFlags.NewTask);

            Application.Context.StartActivity(intent);
        }
    }
}