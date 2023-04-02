using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jobsafe.utils
{
    public class TypeNameConverter : IValueConverter
    {

        public TypeNameConverter() { }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type type = (Type)value;
            Debug.WriteLine("O nome é:"+type.Name);
            return $"{type.Name}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
