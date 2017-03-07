using System;
using System.Linq.Expressions;

namespace ManneDoForms.Common.Validation
{
    public class ValidableProperty<T> where T : class
    {
        // Private Members
        private readonly Expression<Func<T, bool>> _validationExpression;


        // ------------------------------------------------------

        // Constructors
        public ValidableProperty(Expression<Func<T, bool>> validationExpression, T value)
        {
            _validationExpression = validationExpression;

            Value = value;
            IsValid = true;
        }


        // ------------------------------------------------------

        // Public Properties
        public T Value { get; set; }
        public bool IsValid { get; private set; }


        // ------------------------------------------------------

        // Public Methods
        public void Validate()
        {
            try
            {
                var f = _validationExpression.Compile();
                IsValid = f.Invoke(Value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ValidableProperty - Error while executing validation expression: {0}", ex.Message);
                IsValid = false;
            }
        }
    }
}