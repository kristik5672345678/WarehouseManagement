using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WarehouseManagement.Controls.Helpers
{
    public class DiscountBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool hasDiscount && hasDiscount)
            {
                return Brushes.LightGreen; // Зеленый фон для скидки
            }
            return Brushes.White; // Белый фон по умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}