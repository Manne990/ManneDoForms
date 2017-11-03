using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class ProgressAndSpinnerPage : ContentPage
    {
        public ProgressAndSpinnerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), HandleFunc);
        }

        private bool HandleFunc()
        {
            Device.BeginInvokeOnMainThread(() => 
            {
                progress.Progress += 0.1f;
                if (progress.Progress > 1.1f)
                {
                    progress.Progress = 0.0001f;
                }
            });

            return true;
        }
    }
}