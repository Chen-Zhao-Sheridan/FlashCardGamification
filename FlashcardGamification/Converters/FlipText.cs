using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.Converters
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/FlipTextConverter"/>
    public class FlipTextConverter : IValueConverter
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/FlipTextConverter_Convert"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isAnswerVisible)
            {
                return isAnswerVisible ? "Hide Answer" : "Show Answer";
            }
            return "Show Answer";
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/FlipTextConverter_ConvertBack"/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
