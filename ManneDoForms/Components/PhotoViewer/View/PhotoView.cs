using System;
using Xamarin.Forms;

namespace ManneDoForms.Components.PhotoViewer.View
{
    public class PhotoView : ScrollView
    {
        public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(nameof(ImageName), typeof(string), typeof(PhotoView), string.Empty);
        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(PhotoView), false);
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public event EventHandler<EventArgs> Tap;

        protected virtual void OnHandleTap(EventArgs e)
        {
            var handler = Tap;
            if (handler != null)
                handler(this, e);
        }

        public void OnTap()
        {
            OnHandleTap(new EventArgs());
        }
    }
}