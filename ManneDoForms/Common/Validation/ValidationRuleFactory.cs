using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ManneDoForms.Common.Validation
{
    public static class ValidationRuleFactory
    {
        public static readonly Expression<Func<string, bool>> AnythingGoesValidationRule = value => true;

        public static readonly Expression<Func<string, bool>> StringMandatoryValidationRule = value => string.IsNullOrWhiteSpace(value) == false;

        public static readonly Expression<Func<string, bool>> SwedishSsnNotMandatoryValidationRule = value => string.IsNullOrWhiteSpace(value) || Regex.IsMatch(value, "^[0-9]{12}$");
        public static readonly Expression<Func<string, bool>> NorwayDateOfBirthNotMandatoryValidationRule = value => string.IsNullOrWhiteSpace(value) || Regex.IsMatch(value, "^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$");
    }
}