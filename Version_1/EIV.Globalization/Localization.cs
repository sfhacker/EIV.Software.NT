/// <summary>
/// Si el cliente NO especifica ningun idioma, se tomara
/// el idioma de la PC donde se ejecuta el programa de usuario
/// Si el idioma especificado es 'neutral' or invariant,
/// se tomara el idioma x defecto (espanol)
/// </summary>
namespace EIV.Globalization
{
    using System;

    using System.Configuration;
    using System.Globalization;

    using System.Threading;
    using System.Windows;

    public sealed class Localization
    {
        // TODO: Review
        private const string DEFAULT_CULTURE = "es";

        public enum LexicaEnum
        {
            None = -1,
            Banco,
            Cooperativa,
            Mutual
        }

        public enum CultureEnum
        {
            Default = -1,
            English,
            Italian
        }

        private CultureInfo initialComputerCulture = null;
        private CultureInfo initialComputerUICulture = null;

        //private string computerCulture = null;
        //private string computerUICulture = null;

        private LexicaEnum currentLexico = LexicaEnum.None;

        // PresentationFramework
        private ResourceDictionary defaultResourceDictionary = null;

        private string errorMessage = null;

        public Localization()
        {
            this.initialComputerCulture = Thread.CurrentThread.CurrentCulture;
            this.initialComputerUICulture = Thread.CurrentThread.CurrentUICulture;
        }

        public string ComputerCulture
        {
            get
            {
                if (this.initialComputerCulture != null)
                {
                    return this.initialComputerCulture.DisplayName;
                }
                return null;
            }
        }

        public string ComputerUICulture
        {
            get
            {
                if (this.initialComputerUICulture != null)
                {
                    return this.initialComputerUICulture.DisplayName;
                }
                return null;
            }
        }

        public LexicaEnum CurrentLexico
        {
            get
            {
                return this.currentLexico;
            }
        }

        // Used by other layers
        public ResourceDictionary CurrentDictionary
        {
            get
            {
                return this.defaultResourceDictionary;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        public void SetCurrentLexico(LexicaEnum lexico)
        {
            this.currentLexico = lexico;

            // Test
            string lexicoPath = GetLexicoRelativePath();
            this.LoadLexico(lexicoPath);
        }
        public object GetLexicoEntryValue(string key)
        {
            if (this.currentLexico == LexicaEnum.None)
            {
                return null;
            }
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (this.defaultResourceDictionary == null)
            {
                return null;
            }

            return this.defaultResourceDictionary[key];
        }

        private void LoadLexico(string lexicoUrl)
        {
            if (string.IsNullOrEmpty(lexicoUrl))
            {
                return;
            }

            this.errorMessage = null;

            Uri skinDictionaryUri = new Uri(lexicoUrl, UriKind.Relative);

            try
            {
                this.defaultResourceDictionary = Application.LoadComponent(skinDictionaryUri) as ResourceDictionary;
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;

                this.defaultResourceDictionary = null;
            }
        }

        // just testing
        // it should be private
        private string GetLexicoRelativePath()
        {
            //private const string BANCO_ES_DICT = @".\Lexica\Banco\Banco.xaml";
            string basePath = "Lexica";
            string lexicoName = null;
            string cultureName = null;
            string lexicoRelativePath = null;

            if (this.currentLexico == LexicaEnum.None)
            {
                return null;
            }

            lexicoName = this.currentLexico.ToString();
            cultureName = this.GetCultureUITwoLetterName();
            if (string.IsNullOrEmpty(cultureName))
            {
                lexicoRelativePath = string.Format(@".\{0}\{1}\{2}.xaml", basePath, lexicoName, lexicoName);
            } else
            {
                lexicoRelativePath = string.Format(@".\{0}\{1}\{2}_{3}.xaml", basePath, lexicoName, lexicoName, cultureName);
            }

            return lexicoRelativePath;
        }

        // just testing
        // it should be private
        // if CultureInfo(""), then 'iv' (Invariant Culture)
        private string GetCultureUITwoLetterName()
        {
            if (this.initialComputerUICulture == null)
            {
                return string.Empty;
            }
            string twoLetterLangName = this.initialComputerUICulture.TwoLetterISOLanguageName;
            if (twoLetterLangName.Equals("iv") || twoLetterLangName.Equals(DEFAULT_CULTURE))
            {
                return string.Empty;
            }
            return this.initialComputerUICulture.TwoLetterISOLanguageName;
        }

        // System.Configuration
        private string ReadAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];

                return result;
            }
            catch (ConfigurationErrorsException)
            {
                /* Console.WriteLine("Error reading app settings"); */
                //this.statusInfo.Text = "Invalid App.config file.";
            }

            return string.Empty;
        }
    }
}