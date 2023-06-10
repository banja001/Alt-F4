using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Utilities
{
    public static class ThemesController
    {
        public static ThemeTypes CurrentTheme { get; set; }

        public enum ThemeTypes
        {
            Light, Dark
        }

        public static ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources.MergedDictionaries[0]; }
            set { Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        private static void ChangeTheme(Uri uri)
        {
            ThemeDictionary = new ResourceDictionary() { Source = uri };
        }

        public static void SetTheme(ThemeTypes theme)
        {
            string themeName = null;
            CurrentTheme = theme;
            switch (theme)
            {
                case ThemeTypes.Dark: themeName = "Guest2DarkTheme"; break;
                case ThemeTypes.Light: themeName = "Guest2Themes"; break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                    ChangeTheme(new Uri($"/Utilities/Themes/{themeName}.xaml", UriKind.Relative));
            }
            catch { }
        }
    }
}
