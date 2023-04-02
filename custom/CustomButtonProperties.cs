using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Jobsafe.custom
{
    public static class CustomButtonProperties
    {
        public enum Teste
        {
            Primary, Secondary, Success, Danger, None
        }
        //cuidado especial nisso
        //https://learn.microsoft.com/en-us/dotnet/desktop/wpf/properties/xaml-loading-and-dependency-properties?view=netdesktop-7.0
        public static readonly DependencyProperty VariantProperty =
        DependencyProperty.RegisterAttached("Variant", typeof(Teste), typeof(CustomButtonProperties), new PropertyMetadata(Teste.None));
      

        public static Teste GetVariant(DependencyObject obj)
        {
            return (Teste)obj.GetValue(VariantProperty);
        }

        public static void SetVariant(DependencyObject obj, string value)
        {
            obj.SetValue(VariantProperty, value);
        }
    }
}
