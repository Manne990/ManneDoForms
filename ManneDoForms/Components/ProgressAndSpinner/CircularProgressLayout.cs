using System;
using Xamarin.Forms;

namespace ManneDoForms.Components.ProgressAndSpinner
{
    public class CircularProgressLayout : AbsoluteLayout
    {
        #region Private Members

        private readonly Image _spinnerView;
        private CircularProgressView _progressView;
        private Label _label;
        private AbsoluteLayout _parentContainer;
        private StackLayout _blockerView;

        #endregion

        // ------------------------------------------------------------

        #region Constructors

        public CircularProgressLayout()
        {
            // Init
            Opacity = 0;

            // Spinner View
            _spinnerView = new Image() { Opacity = 0 };

            AbsoluteLayout.SetLayoutFlags(_spinnerView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_spinnerView, new Rectangle(0f, 0f, 1f, 1f));

            Children.Add(_spinnerView);

            // Spinner View
            _progressView = new CircularProgressView() { Opacity = 0 };

            AbsoluteLayout.SetLayoutFlags(_progressView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_progressView, new Rectangle(0f, 0f, 1f, 1f));

            _progressView.SetBinding(CircularProgressView.ProgressProperty, new Binding(path: "Progress", source: this));

            Children.Add(_progressView);

            // Label
            _label = new Label() { FontSize = 7, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };

            _label.SetBinding(Label.TextProperty, new Binding(path: "Text", source: this));
            _label.SetBinding(Label.TextColorProperty, new Binding(path: "TextColor", source: this));
            _label.SetBinding(Label.StyleProperty, new Binding(path: "LabelStyle", source: this));
            _label.SetBinding(Label.FontFamilyProperty, new Binding(path: "LabelFontFamily", source: this));

            AbsoluteLayout.SetLayoutFlags(_label, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(_label, new Rectangle(0.5f, 0.5f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Children.Add(_label);
        }

        #endregion

        // ------------------------------------------------------------

        #region Overrides

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if(propertyName == IsBusyProperty.PropertyName)
            {
                if(IsBusy)
                {
                    if(Progress <= MinProgress || Progress < ProgressLowerThreshold)
                    {
                        ShowSpinner();
                        StartSpinning();
                    }
                    else
                    {
                        ShowProgress();
                    }

                    return;
                }

                StopSpinning();
                HideSpinner();
                HideProgress();

                return;
            }

            if(propertyName == ProgressProperty.PropertyName)
            {
                if(Progress < ProgressLowerThreshold)
                {
                    return;
                }

                if(Progress > MinProgress)
                {
                    HideSpinner();
                    StopSpinning();
                    ShowProgress();
                    return;
                }

                if(IsBusy)
                {
                    ShowSpinner();
                    HideProgress();
                    StartSpinning();
                    return;
                }

                return;
            }
        }

        #endregion

        // ------------------------------------------------------------

        #region Public Properties

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource Source
        {
            get { return _spinnerView.Source; }
            set
            {
                _spinnerView.Source = value; 
                _progressView.Source = value; 
            }
        }


        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(float), typeof(CircularProgressView), 0f);
        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public static readonly BindableProperty ProgressLowerThresholdProperty = BindableProperty.Create(nameof(ProgressLowerThreshold), typeof(float), typeof(CircularProgressView), 0.01f);
        public float ProgressLowerThreshold
        {
            get { return (float)GetValue(ProgressLowerThresholdProperty); }
            set { SetValue(ProgressLowerThresholdProperty, value); }
        }

        public static readonly BindableProperty MinProgressProperty = BindableProperty.Create(nameof(MinProgress), typeof(float), typeof(CircularProgressView), 0f);
        public float MinProgress
        {
            get { return (float)GetValue(MinProgressProperty); }
            set { SetValue(MinProgressProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CircularProgressLayout), string.Empty);
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircularProgressLayout), Color.Black);
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(CircularProgressLayout), false);
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(CircularProgressLayout), null);
        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public static readonly BindableProperty LabelFontFamilyProperty = BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(CircularProgressLayout), string.Empty);
        public string LabelFontFamily
        {
            get { return (string)GetValue(LabelFontFamilyProperty); }
            set { SetValue(LabelFontFamilyProperty, value); }
        }

        #endregion

        // ------------------------------------------------------------

        #region Private Methods

        private AbsoluteLayout FindPageContainer()
        {
            Element parent = Parent;

            while(true)
            {
                if(parent == null)
                {
                    return null;
                }

                if(parent is ContentPage)
                {
                    break;
                }

                parent = parent.Parent;
            }

            ContentPage parentPage = (ContentPage)parent;

            return parentPage.Content is AbsoluteLayout ? (AbsoluteLayout)parentPage.Content : null;
        }

        private void AddBlocker()
        {
            if(_parentContainer != null)
            {
                return;
            }

            _parentContainer = FindPageContainer();

            if(_parentContainer == null)
            {
                return;
            }

            _blockerView = new StackLayout() { BackgroundColor = Color.Transparent };

            AbsoluteLayout.SetLayoutFlags(_blockerView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_blockerView, new Rectangle(0, 1, 1, 1));

            _parentContainer.Children.Add(_blockerView);
        }

        private void RemoveBlocker()
        {
            if(_parentContainer == null || _blockerView == null)
            {
                return;
            }

            _parentContainer.Children.Remove(_blockerView);

            _blockerView = null;
            _parentContainer = null;
        }

        private void ShowSpinner()
        {
            AddBlocker();

            _spinnerView.Opacity = 1;

            Device.BeginInvokeOnMainThread(async () => {
                await this.FadeTo(1, 200, Easing.CubicInOut);

                _spinnerView.Rotation = 0;
            });
        }

        private void ShowProgress()
        {
            AddBlocker();

            if(_progressView.Opacity > 0)
            {
                return;
            }

            _progressView.Opacity = 1;

            Device.BeginInvokeOnMainThread(async () => {
                await this.FadeTo(1, 200, Easing.CubicInOut);
            });
        }

        private void HideSpinner()
        {
            RemoveBlocker();

            if(_progressView.Opacity.Equals(0))
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await this.FadeTo(0, 200, Easing.CubicInOut);
                });
            }

            _spinnerView.Opacity = 0;
        }

        private void HideProgress()
        {
            RemoveBlocker();

            if(_spinnerView.Opacity.Equals(0))
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await this.FadeTo(0, 200, Easing.CubicInOut);
                });
            }

            _progressView.Opacity = 0;
        }

        private void StartSpinning()
        {
            double currentRotation = 0;

            Device.BeginInvokeOnMainThread(() => {
                _spinnerView.Rotation = 0;
            });

            Device.StartTimer(TimeSpan.FromMilliseconds(100), () => {
                currentRotation += 45;

                if (currentRotation > double.MaxValue) {
                    currentRotation = 0;
                }

                Device.BeginInvokeOnMainThread(() => {
                    _spinnerView.Rotation = currentRotation;
                });

                return IsBusy && Progress < ProgressLowerThreshold;
            });
        }

        private void StopSpinning()
        {
            Device.BeginInvokeOnMainThread(() => {
                _spinnerView.Rotation = 0;
            });
        }

        #endregion
    }
}