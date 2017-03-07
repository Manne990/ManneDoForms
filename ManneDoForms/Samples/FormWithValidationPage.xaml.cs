using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManneDoForms.Common.Validation;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class FormWithValidationPage : ContentPage
    {
        private FormWithValidationViewModel _viewModel;

        public FormWithValidationPage()
        {
            InitializeComponent();

            _viewModel = new FormWithValidationViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.Load();
        }
    }

    public class FormWithValidationViewModel : ViewModelBase
    {
        private ValidableProperty<string> _firstName;
        public ValidableProperty<string> FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private ValidableProperty<string> _lastName;
        public ValidableProperty<string> LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(async () =>
                    {
                        if (Validate() == false)
                        {
                            return;
                        }

                        await Task.Delay(1000); //REMARK: Simulate that something happens... :-)
                    });
                }

                return _saveCommand;
            }
        }

        public void Load()
        {
            FirstName = new ValidableProperty<string>(ValidationRuleFactory.StringMandatoryValidationRule, string.Empty);
            LastName = new ValidableProperty<string>(ValidationRuleFactory.StringMandatoryValidationRule, string.Empty);
        }

        private bool Validate()
        {
            FirstName.Validate();
            RaisePropertyChanged(() => FirstName);

            LastName.Validate();
            RaisePropertyChanged(() => LastName);

            return FirstName.IsValid && LastName.IsValid;
        }
    }
}