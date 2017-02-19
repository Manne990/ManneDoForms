using Xamarin.Forms;

namespace ManneDoForms.Components.LabelView
{
    public class ManneLabel : Label
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ManneLabel), default(Thickness));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
    }
}