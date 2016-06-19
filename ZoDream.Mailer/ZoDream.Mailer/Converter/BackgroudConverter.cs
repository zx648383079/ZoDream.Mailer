using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ZoDream.Mailer.Model;

namespace ZoDream.Mailer.Converter
{
    public class BackgroudConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EmailStatus)value)
            {
                case EmailStatus.Waiting:
                    return Brushes.Yellow;
                case EmailStatus.Failure:
                    return Brushes.Red;
                case EmailStatus.Success:
                    return Brushes.ForestGreen;
                case EmailStatus.None:
                default:
                    return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
