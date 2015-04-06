using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TicketResto.PhoneApp.Converters
{

	class DecimalConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is decimal)
			{
				var dec = (decimal)value;
				return dec.ToString(CultureInfo.CurrentUICulture);
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is string)
			{
				var str = value as string;
                CultureInfo currentCulture = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
				decimal result;
                if (decimal.TryParse(str, NumberStyles.Float, currentCulture, out result))
				{
					return result;
				}
			}
			return value;
		}
	}
}
