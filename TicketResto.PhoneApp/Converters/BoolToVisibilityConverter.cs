using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TicketResto.PhoneApp.Converters
{
    class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is bool)
            {
                var visible = (bool)value;
                return visible ? Visibility.Visible : Visibility.Collapsed;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is Visibility)
            {
                var visible = (Visibility)value;
                return visible == Visibility.Visible ? true : false;
            }
            return value;
        }

        #endregion
    }
}
