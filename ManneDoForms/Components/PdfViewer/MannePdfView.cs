using System;
using Xamarin.Forms;

namespace ManneDoForms.Components.PdfViewer
{
    public class MannePdfView : Xamarin.Forms.View
    {
        #region Public Properties

        public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(MannePdfView),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (MannePdfView)bindable;
                ctrl.Url = newValue.ToString();
            }
        );

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        #endregion

        // ---------------------------------------------------------

        #region Events

        public event EventHandler<EventArgs> Finished;
        public event EventHandler<EventArgs> Error;

        #endregion

        // ---------------------------------------------------------

        #region Public Methods

        public void SetFinished()
        {
            if (this.Finished != null)
            {
                this.Finished(this, new EventArgs());
            }
        }

        public void SetError()
        {
            if (this.Error != null)
            {
                this.Error(this, new EventArgs());
            }
        }

        #endregion
    }
}