using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Utilities
{
    public static class ButtonProperties
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(string), typeof(ButtonProperties), new PropertyMetadata(null));

        public static string GetImageSource(DependencyObject obj)
        {
            return (string)obj.GetValue(ImageSourceProperty);
        }

        public static void SetImageSource(DependencyObject obj, string value)
        {
            obj.SetValue(ImageSourceProperty, value);
        }
    }

}
