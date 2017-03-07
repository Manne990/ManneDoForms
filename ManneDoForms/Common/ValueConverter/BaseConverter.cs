using System;
using Xamarin.Forms.Xaml;

namespace ManneDoForms.Common.ValueConverter
{
    public class BaseConverter : IMarkupExtension
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}