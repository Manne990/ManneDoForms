using System;
using Xamarin.Forms;

namespace ManneDoForms.Components.ImageView
{
    public class ManneImageView : Image
    {
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            //System.Diagnostics.Debug.WriteLine(propertyName);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            //System.Diagnostics.Debug.WriteLine("OnBindingContextChanged");
        }

        // Public Properties
        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ManneImageView), false);
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly BindableProperty ScaleTypeProperty = BindableProperty.Create(nameof(ScaleType), typeof(ImageScaleTypes), typeof(ManneImageView), ImageScaleTypes.Crop);
        public ImageScaleTypes ScaleType
        {
        	get { return (ImageScaleTypes)GetValue(ScaleTypeProperty); }
        	set { SetValue(ScaleTypeProperty, value); }
        }

        public static readonly new BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(ManneImageView), null);
        [TypeConverter(typeof(ImageSourceConverter))]
        public new ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Enums
        public enum ImageScaleTypes
        {
        	Crop,
        	TopCrop,
        	ScaleToFit
        }
    }
}