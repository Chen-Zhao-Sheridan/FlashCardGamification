using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.Converters
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/InverseBoolConverter"/>
    public class InverseBoolConverter : IValueConverter
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/InverseBoolConverter_Convert"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false; 
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/InverseBoolConverter_ConvertBack"/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}
