using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        RelayCommand SaveCommand { get; }
    }

    public class FormWithValidationViewModel : ViewModelBase, IFormWithValidationViewModel
    {
        public FormWithValidationViewModel()
        {
            ValidationErrors = new ObservableCollection<string>();

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
                    _saveCommand = new RelayCommand(async () =>
                    {
                        if (ValidateForm() == false)
                        {
                            return;
                        }

                        System.Diagnostics.Debug.WriteLine("Valid -> Save!");

                        await Task.Delay(200); //REMARK: Simulate that something happens... :-)
                    });
                }

                return _saveCommand;
            }
        }

        private bool ValidateForm()
        {
            // Init
            ValidationErrors = new ObservableCollection<string>();

            // First Name
            FirstName.Validate();
            RaisePropertyChanged(() => FirstName);

            foreach (var message in FirstName.ErrorMessages)
            {
                ValidationErrors.Add(message);
            }

            // Last Name
            LastName.Validate();
            RaisePropertyChanged(() => LastName);

            foreach (var message in LastName.ErrorMessages)
            {
                ValidationErrors.Add(message);
            }

            // Date of Birth
            DateOfBirth.Validate();
            RaisePropertyChanged(() => DateOfBirth);

            foreach (var message in DateOfBirth.ErrorMessages)
            {
                ValidationErrors.Add(message);
            }

            // Return...
            return FirstName.IsValid && LastName.IsValid && DateOfBirth.IsValid;
        }
    }
}