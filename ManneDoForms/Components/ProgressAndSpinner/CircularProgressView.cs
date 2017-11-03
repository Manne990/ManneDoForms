using System;
using Xamarin.Forms;

namespace ManneDoForms.Components.ProgressAndSpinner
{
    public class CircularProgressView : Image
    {
        #region Public Properties

        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(float), typeof(CircularProgressView), 0f);
        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set
            {
                var newValue = value;

                if(newValue > 1.0f)
                {
                    newValue = 1.0f;
                }

                if(newValue < 0f)
                {
                    newValue = 0f;
                }

                SetValue(ProgressProperty, newValue); 
            }
        }

        #endregion
    }
}