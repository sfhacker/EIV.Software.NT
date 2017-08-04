/// <summary>
/// 
/// </summary>
namespace EIV.TelerikThemes
{
    using System;
    using System.Collections.ObjectModel;

    public sealed class TelerikTheme
    {
        // pack://application:,,,/EIV.UI.TelerikThemes;component/ThemesDictionary.xaml
        // Build action should be: 'Resource'
        // 'Telerik.Windows.Themes.<theme name>.dll' debe ser agregado en la app que llama a este DLL
        private const string THEMES_FOLDER = @"/EIV.UI.TelerikThemes;component/ThemesDictionary.xaml";
        private const string THEMES_FORMAT = "/Telerik.Windows.Themes.{0};component/Themes/{1}";

        // 15 themes
        public enum ThemeEnum
        {
            None = -1,
            Expression_Dark,
            Green,
            Office2013,
            Office2016,
            Office2016Touch,
            Office_Black,       // Default
            Office_Blue,
            Office_Silver,
            Summer,
            Transparent,
            Vista,
            VisualStudio2013,
            Windows7,
            Windows8,
            Windows8Touch
        }

        // Default Theme is Office_Black
        private ThemeEnum currentTheme = ThemeEnum.Office_Black;

        public TelerikTheme()
        {

        }
        public ThemeEnum CurrentTheme
        {
            get
            {
                return this.currentTheme;
            }
            set
            {
                this.currentTheme = value;
            }
        }

        // this can be done using .xaml but .....
        // 'Telerik.Windows.Themes.<theme name>.dll' debe ser agregado en la app que llama a este DLL
        // Este method deberia ser private y utilizar solo 'GetThemeByUser()'
        public Collection<System.Windows.ResourceDictionary> GetThemeDictionary()
        {
            string themeName = null;
            string themeFormat = null;
            System.Windows.ResourceDictionary myResourceDictionary = null;

            if (this.currentTheme == ThemeEnum.None)
            {
                return null;
            }

            // it may not work for all themes!
            themeName = this.currentTheme.ToString();

            Collection<System.Windows.ResourceDictionary> rst = new Collection<System.Windows.ResourceDictionary>();

            string[] themeControls = { "System.Windows.xaml", "Telerik.Windows.Controls.xaml",
                                       "Telerik.Windows.Controls.Data.xaml", "Telerik.Windows.Controls.GridView.xaml",
                                       "Telerik.Windows.Controls.Input.xaml", "Telerik.Windows.Controls.Navigation.xaml"
                                     };

            foreach (string themeControl in themeControls)
            {
                themeFormat = string.Format(THEMES_FORMAT, themeName, themeControl);

                myResourceDictionary = new System.Windows.ResourceDictionary();
                myResourceDictionary.Source = new Uri(themeFormat, UriKind.RelativeOrAbsolute);

                rst.Add(myResourceDictionary);
            }

            return rst;
        }

        // TODO:
        // Esta info quedara guardada en la DB?
        // Se toma el usuario logeado en la PC? AD? o el usuario que se logea a nuestra app?
        // En el registro de Windows o en un XML
        public Collection<System.Windows.ResourceDictionary> GetThemeByUser() // User
        {
            return null;
        }
    }
}