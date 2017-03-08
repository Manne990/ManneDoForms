using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ManneDoForms.Common.Validation
{
    public static class ValidationExpressions
    {
        public static readonly Expression<Func<string, bool>> StringMandatory = value => string.IsNullOrWhiteSpace(value) == false;

        public static readonly Expression<Func<string, bool>> StringMinLength2 = value => string.IsNullOrWhiteSpace(value) || value.Length >= 2;

        public static readonly Expression<Func<string, bool>> SwedishDateFormat = value => string.IsNullOrWhiteSpace(value) || Regex.IsMatch(value ?? string.Empty, "^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$");
    }
}