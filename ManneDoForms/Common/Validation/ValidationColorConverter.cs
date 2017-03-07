using System;
using System.Globalization;
using ManneDoForms.Common.ValueConverter;
using Xamarin.Forms;

namespace ManneDoForms.Common.Validation
{
    public class ValidationBackgroundColorConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new Exception("ValidationBackgroundColorConverter: Parameter must be bool!");
            }

            var isValid = (bool)value;

            return isValid ? Color.White : Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValidationTextColorConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new Exception("ValidationTextColorConverter: Parameter must be bool!");
            }

            var isValid = (bool)value;

            return isValid ? Color.Black : Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValidationPlaceholderColorConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new Exception("ValidationPlaceholderColorConverter: Parameter must be bool!");
            }

            var isValid = (bool)value;

            return isValid ? Color.Gray : Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}