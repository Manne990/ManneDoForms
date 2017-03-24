using ManneDoForms.Common.Rotation;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class RotationLandscapeOnlyPage : RotationAwarePage
    {
        public RotationLandscapeOnlyPage() : base(InterfaceOrientationTypes.Landscape)
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("X", string.Empty, () =>
            {
                Navigation.PopModalAsync(true);

                // REMARK: Due to this bug https://bugzilla.xamarin.com/show_bug.cgi?id=52419
                MessagingCenter.Send<RotationAwarePage, InterfaceOrientationTypes>(this, RotationAwarePage.NewPageOrientationRequestedMessage, InterfaceOrientationTypes.Portrait);
            }));
        }
    }
}