using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SendKeys {

    public class StringToBoolConverter :
        IValueConverter {

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        ) {
            return !String.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        ) {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }
}
