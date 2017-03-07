using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ManneDoForms.Common.Validation;
using Xamarin.Forms;
using XLabs.Ioc;

namespace ManneDoForms.Samples
{
    public partial class FormWithValidationPage : ContentPage
    {
        public FormWithValidationPage()
        {
            InitializeComponent();

            BindingContext = Resolver.Resolve<IFormWithValidationViewModel>();
        }
    }

    public interface IFormWithValidationViewModel
    {
        ValidableProperty<string> FirstName { get; set; }
        ValidableProperty<string> LastName { get; set; }
        ValidableProperty<string> DateOfBirth { get; set; }

        ObservableCollection<string> ValidationErrors { get; set; }

        RelayCommand SaveCommand { get; }
    }

    public class FormWithValidationViewModel : ViewModelBase, IFormWithValidationViewModel
    {
        public FormWithValidationViewModel()
        {
            // Init
            ValidationErrors = new ObservableCollection<string>();

            // Create Properties and Add Validation Rules
            FirstName = new ValidableProperty<string>();
            FirstName.AddExpression(ValidationRuleFactory.StringMandatoryValidationRule, "First name is mandatory");

            LastName = new ValidableProperty<string> { Hint = "optional" };

            DateOfBirth = new ValidableProperty<string> { Hint = "yyyy-mm-dd" };
            DateOfBirth.AddExpression(ValidationRuleFactory.StringMandatoryValidationRule, "Date of birth is mandatory");
            DateOfBirth.AddExpression(ValidationRuleFactory.DateOfBirthValidationRule, "Date of birth must be in format: yyyy-mm-dd");
        }

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

        private ValidableProperty<string> _dateOfBirth;
        public ValidableProperty<string> DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                RaisePropertyChanged(() => DateOfBirth);
            }
        }

        private ObservableCollection<string> _validationErrors;
        public ObservableCollection<string> ValidationErrors
        {
            get { return _validationErrors; }
            set
            {
                _validationErrors = value;
                RaisePropertyChanged(() => ValidationErrors);
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(SaveForm);
                }

                return _saveCommand;
            }
        }

        private void SaveForm()
        {
            if (ValidateForm() == false)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine($"First name: {FirstName.Value}");
            System.Diagnostics.Debug.WriteLine($"Last name: {LastName.Value}");
            System.Diagnostics.Debug.WriteLine($"Date of birth: {DateOfBirth.Value}");
        }

        private bool ValidateForm()
        {
            var validationErrors = new List<string>();

            FirstName.Validate();
            validationErrors.AddRange(FirstName.ErrorMessages);

            LastName.Validate();
            validationErrors.AddRange(LastName.ErrorMessages);

            DateOfBirth.Validate();
            validationErrors.AddRange(DateOfBirth.ErrorMessages);

            ValidationErrors = new ObservableCollection<string>(validationErrors);

            return FirstName.IsValid && LastName.IsValid && DateOfBirth.IsValid;
        }
    }
}