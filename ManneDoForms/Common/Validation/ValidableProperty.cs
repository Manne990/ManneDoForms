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
        private readonly List<ValidationExpression<T>> _expressions;


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
            _expressions = new List<ValidationExpression<T>>();
            IsValid = true;
        }


        // ------------------------------------------------------

        // Public Methods
        public void AddExpression(Expression<Func<T, bool>> expression, string errorMessage)
        {
            _expressions.Add(new ValidationExpression<T> { Expression = expression, ErrorMessage = errorMessage });
        }

        public void ClearExpressions()
        {
            _expressions.Clear();
        }

        public void Validate()
        {
            ErrorMessages = new ObservableCollection<string>();

            try
            {
                foreach (var expression in _expressions)
                {
                    var f = expression.Expression.Compile();

                    if (f.Invoke(Value) == false)
                    {
                        ErrorMessages.Add(expression.ErrorMessage);
                    }
                }

                IsValid = ErrorMessages.Count == 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ValidableProperty - Error while executing validation expression: {0}", ex.Message);
                IsValid = false;
            }
        }
    }

    public class ValidationExpression<T> where T : class
    { 
        public Expression<Func<T, bool>> Expression { get; set; }
        public string ErrorMessage { get; set; }
    }
}