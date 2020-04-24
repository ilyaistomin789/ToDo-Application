using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using OOP_LAB_7_8.Resources.Languages;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages => m_Languages;

        public App()
        {
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo(LanguageString.language[0]));
            m_Languages.Add(new CultureInfo(LanguageString.language[1]));
            m_Languages.Add(new CultureInfo(LanguageString.language[2]));
            App.LanguageChanged += App_LanguageChanged;
            
        }
        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("Resources/Languages/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    case "en-BY":
                        dict.Source = new Uri(String.Format("Resources/Languages/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/Languages/lang.xaml", UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                    where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Languages/lang.")
                    select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }
        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            OOP_LAB_7_8.Properties.Settings.Default.DefaultLanguage = Language;
            OOP_LAB_7_8.Properties.Settings.Default.Save();
        }
    }
}
