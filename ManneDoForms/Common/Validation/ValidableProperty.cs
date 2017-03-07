using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ManneDoForms.Common.Validation
{
    public class ValidableProperty<T> where T : class
    {
        // Private Members
        private readonly List<ValidationExpression<T>> _expressions;


        // ------------------------------------------------------

        // Public Properties
        public T Value { get; set; }
        public string Hint { get; set; }
        public bool IsValid { get; private set; }
        public List<string> ErrorMessages { get; private set; }


        // ------------------------------------------------------

        // Constructors
        public ValidableProperty()
        {
            ErrorMessages = new List<string>();
            _expressions = new List<ValidationExpression<T>>();
            IsValid = true;
        }


        // ------------------------------------------------------

        // Public Methods
        public void AddExpression(Expression<Func<T, bool>> expression, string errorMessage)
        {
            _expressions.Add(new ValidationExpression<T> { Expression = expression, ErrorMessage = errorMessage });
        }

        public void Validate()
        {
            ErrorMessages = new List<string>();

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