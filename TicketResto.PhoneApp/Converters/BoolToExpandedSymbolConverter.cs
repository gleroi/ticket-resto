using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TicketResto.PhoneApp.Converters
{
    class BoolToExpandedSymbolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is bool)
            {
                var expanded = (bool)value;
                return expanded ? Symbol.HideBcc : Symbol.ShowBcc;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is Symbol)
            {
                var symbol = (Symbol)value;
                switch (symbol)
                {
                    case Symbol.HideBcc:
                        return true;
                    case Symbol.ShowBcc:
                        return false;
                    default:
                        return false;
                }
            }
            return value;
        }

        #endregion
    }
}
