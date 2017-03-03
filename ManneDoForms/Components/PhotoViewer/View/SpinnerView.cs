using ManneDoForms.Common;
using Xamarin.Forms;

namespace ManneDoForms.Components.PhotoViewer.View
{
    public class SpinnerView : ContentView
    {
        private RoundedBox _roundedBox;
        private ProgressBar _spinnerViewProgressBar;
        private Label _spinnerViewLabel;
        private ActivityIndicator _spinnerViewActivityIndicator;

        public SpinnerView()
        {
            // Init
            this.Opacity = 0;

            // Create Controls
            var relativeLayout = new RelativeLayout()
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

            _roundedBox = new RoundedBox()
                {
                    BackgroundColor = Color.Black,
                    CornerRadius = 10,
                    Opacity = 1
                };

            _spinnerViewProgressBar = new ProgressBar();
            _spinnerViewLabel = new Label() { TextColor = Color.White, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), HorizontalTextAlignment = TextAlignment.Center };
            _spinnerViewActivityIndicator = new ActivityIndicator() { Color = Color.White };

            // Rounded Box
            relativeLayout.Children.AddInOrigo(_roundedBox, 120, 100);

            // Progress Bar
            //_spinnerViewProgressBar.IsVisible = this.ShowProgress;
            _spinnerViewProgressBar.Opacity = this.ShowProgress ? 1 : 0;
            _spinnerViewProgressBar.Progress = this.Progress;

            relativeLayout.Children.Add(_spinnerViewProgressBar, 
                xConstraint:Constraint.RelativeToView(_roundedBox, (RelativeLayout arg1, Xamarin.Forms.View arg2) =>
                    {
                        return arg2.Bounds.Left + 10;
                    }), 
                yConstraint:Constraint.RelativeToView(_roundedBox, (RelativeLayout arg1, Xamarin.Forms.View arg2) =>
                    {
                        return arg2.Bounds.Bottom - 15;
                    }), 
                widthConstraint:Constraint.RelativeToView(_roundedBox, (RelativeLayout arg1, Xamarin.Forms.View arg2) =>
                    {
                        return arg2.Bounds.Width - 20;
                    }), 
                heightConstraint:Constraint.Constant(10));

            // Label
            _spinnerViewLabel.Text = this.Message;

            relativeLayout.Children.Add(_spinnerViewLabel, 
                xConstraint:Constraint.RelativeToView(_roundedBox, (RelativeLayout arg1, Xamarin.Forms.View arg2) =>
                    {
                        return arg2.Bounds.Left + 10;
                    }), 
                yConstraint:Constraint.RelativeToView(_roundedBox, (RelativeLayout arg1, Xamarin.Forms.View arg2) =>
                    {
                        return arg2.Bounds.Top + 5;
                    }), 
                widthConstraint:Constraint.RelativeToView(_roundedBox, (RelativeLayout arg1, Xamarin.Forms.View arg2) =>
                    {
                        return arg2.Bounds.Width - 20;
                    }));

            // Activity Indicator
            relativeLayout.Children.AddInOrigo(_spinnerViewActivityIndicator, 50, 50);

            // Return
            this.Content = relativeLayout;
        }

        public static BindableProperty IsBusyProperty = BindableProperty.Create<SpinnerView, bool>(ctrl => ctrl.IsBusy,
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) => {
            var ctrl = (SpinnerView)bindable;
            ctrl.IsBusy = newValue;
        }
        );

        public Color SpinnerBackgroundColor
        {
            get
            { 
                return _roundedBox.BackgroundColor;
            }
            set
            { 
                _roundedBox.BackgroundColor = value;
            }
        }

        public bool IsBusy
        {
            get 
            {
                return (bool)GetValue(IsBusyProperty); 
            }
            set
            { 
                SetValue (IsBusyProperty, value);

                //this.IsVisible = value;
                this.Opacity = value ? 1 : 0;
                _spinnerViewActivityIndicator.IsRunning = value;
            }
        }

        public static BindableProperty ShowProgressProperty = BindableProperty.Create<SpinnerView, bool>(ctrl => ctrl.ShowProgress,
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) => {
            var ctrl = (SpinnerView)bindable;
            ctrl.ShowProgress = newValue;
        }
        );

        public bool ShowProgress
        {
            get 
            { 
                return (bool)GetValue(ShowProgressProperty); 
            }
            set
            { 
                SetValue(ShowProgressProperty, value); 

                //_spinnerViewProgressBar.IsVisible = value;
                _spinnerViewProgressBar.Opacity = value ? 1 : 0;
            }
        }

        public static BindableProperty ProgressProperty = BindableProperty.Create<SpinnerView, double>(ctrl => ctrl.Progress,
            defaultValue: 0.0,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) => {
            var ctrl = (SpinnerView)bindable;
            ctrl.Progress = newValue;
        }
        );

        public double Progress
        {
            get 
            {
                return (double)GetValue(ProgressProperty); 
            }
            set
            { 
                SetValue (ProgressProperty, value);

                _spinnerViewProgressBar.Progress = value;
            }
        }

        public static BindableProperty MessageProperty = BindableProperty.Create<SpinnerView, string>(ctrl => ctrl.Message,
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) => {
            var ctrl = (SpinnerView)bindable;
            ctrl.Message = newValue;
        }
        );

        public string Message
        {
            get 
            {
                return (string)GetValue(MessageProperty); 
            }
            set
            { 
                SetValue (MessageProperty, value);

                _spinnerViewLabel.Text = value;
            }
        }
    }
}