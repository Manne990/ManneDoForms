using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using GalaSoft.MvvmLight;

namespace ManneDoForms.Common.Validation
{
    public class ValidableProperty<T> : ViewModelBase where T : class
    {
        // Private Members
        private readonly List<ValidationRule<T>> _rules;


        // ------------------------------------------------------

        // Public Properties
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        private string _hint;
        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
                RaisePropertyChanged(() => Hint);
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            private set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        private ObservableCollection<string> _errorMessages;
        public ObservableCollection<string> ErrorMessages
        {
            get { return _errorMessages; }
            private set
            {
                _errorMessages = value;
                RaisePropertyChanged(() => ErrorMessages);
            }
        }


        // ------------------------------------------------------

        // Constructors
        public ValidableProperty()
        {
            ErrorMessages = new ObservableCollection<string>();
            _rules = new List<ValidationRule<T>>();
            IsValid = true;
        }


        // ------------------------------------------------------

        // Public Methods
        public void AddRule(Expression<Func<T, bool>> expression, string errorMessage)
        {
            _rules?.Add(new ValidationRule<T> { Expression = expression, ErrorMessage = errorMessage });
        }

        public void ClearRules()
        {
            _rules?.Clear();
        }

        public void Validate()
        {
            ErrorMessages = new ObservableCollection<string>();

            try
            {
                foreach (var rule in _rules)
                {
                    var f = rule.Expression.Compile();

                    if (f.Invoke(Value) == false)
                    {
                        ErrorMessages.Add(rule.ErrorMessage);
                    }
                }

                IsValid = ErrorMessages.Count == 0;
            }
            catch
            {
                IsValid = false;
            }
        }
    }

    public class ValidationRule<T> where T : class
    { 
        public Expression<Func<T, bool>> Expression { get; set; }
        public string ErrorMessage { get; set; }
    }
}