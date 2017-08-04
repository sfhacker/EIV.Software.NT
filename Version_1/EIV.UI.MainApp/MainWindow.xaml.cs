namespace EIV.UI.MainApp
{
    using SlideMenu;
    using System.Configuration;
    using System.Windows;
    using System.Windows.Input;

    using Telerik.Windows.Controls;
    using ServiceContext.Interface;
    using ServiceContext.Service;
    using System.Windows.Threading;
    using System;
    using UserControlBase.Interface;
    using View;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SERVICE_URL = "ServiceURL";
        private const string SERVER_NAME = "ServerName";

        private IUserControlService formService = null;

        private string serviceURL = null;
        private string serverName = null;

        public MainWindow()
        {
            InitializeComponent();
            IconSources.ChangeIconsSet(IconsSet.Modern);

            this.serviceURL = this.ReadAppSetting(SERVICE_URL);
            this.serverName = this.ReadAppSetting(SERVER_NAME);

            this.formService = new UserControlService();

            this.lblBranchName.Text = this.serverName;
            this.lblUserName.Text = this.GetFullUserName();
            this.SetDateTimeTimer();

            // Can connect later on
            bool rst = this.formService.Connect(this.serviceURL);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Menu.Toggle();
        }

        private void Custom_OnClick(object sender, RoutedEventArgs e)
        {
            CustomMenu.Toggle();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You Clicked Packing!");
        }

        private void DefaultClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Default;
        }

        private void PrimaryClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Primary;
        }

        private void SuccessClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Success;
        }

        private void WarningClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Warning;
        }

        private void DangerClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Danger;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Menu.Hide();
        }

        private void PaisesClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("You Clicked Paises!");
            //var winPaises = new PaisesView();
            //winPaises.ShowDialog();
        }

        private void ToggleClosingTypeClick(object sender, RoutedEventArgs e)
        {
            Menu.ClosingType = Menu.ClosingType == ClosingType.Auto
                ? ClosingType.Manual
                : ClosingType.Auto;
        }

        private SideMenu MapMenuToTheme(SideMenuTheme theme)
        {
            //this should not be necesray but colors are not changing correctly
            //when changing theme porperty... maybe its needed to implement INotifyPropertyChanged
            return new SideMenu
            {
                MenuWidth = Menu.MenuWidth,
                Theme = theme,
                Menu = Menu.Menu
            };
        }

        private string GetFullUserName()
        {
            string fullUserName = null;

            if (this.formService == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(this.formService.DomainName))
            {
                fullUserName = this.formService.UserName;
            } else
            {
                fullUserName = string.Format("{0}\\{1}", this.formService.DomainName, this.formService.UserName);
            }

            return fullUserName;
        }

        private void SetDateTimeTimer()
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            //  HH:mm tt
            this.lblDateTime.Text = DateTime.Now.ToString("G");

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        // This could go in EIV.Helpers
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

        private void mnuPais_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Proximamente", "Pais", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void mnuProvincia_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.Hide();

            MessageBox.Show("Proximamente", "Provincia", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Dependency: EIV.UI.UserControlBase
        private void mnuLocalidad_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.Hide();

            // Este nombre debe ser univoco (por lo menos dentro del mismo assembly)
            Type formType = this.formService.FindFormulario("EIV.UI.Formularios.Localidad.LocalidadABM");
            if (formType == null)
            {
                MessageBox.Show("Recurso no encontrado.", "Localidad", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            // Testing ... 2017-08-02
            this.formService.RenderFormulario(formType);

            return;

            object[] argValues = new object[] { this.formService };

            // "No hay constructor sin parámetros definido para este objeto."
            IUserControlBase userControl = Activator.CreateInstance(formType, args: argValues) as IUserControlBase;

            userControl.Initialize();

            // this.CreateNewWindow(userControl.MainInterface);
            this.CreateNewRadWindow(userControl.MainInterface);
        }

        // In MainApp ??
        private void CreateNewWindow(System.Windows.Controls.UserControl winContent)
        {
            Window newWin = new Window();

            newWin.Title = "Ventana Dinamica";
            newWin.Owner = this;

            newWin.Width = 300;
            newWin.Height = 300;
            newWin.WindowStyle = WindowStyle.ToolWindow;
            newWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            newWin.Content = winContent;

            // Modal
            newWin.ShowDialog();

            // Non-Modal
            // newWin.Show();
        }

        private void CreateNewRadWindow(System.Windows.Controls.UserControl winContent)
        {
            RadWindow newWin = new RadWindow();

            newWin.Header = "Ventana Dinamica (Rad)";
            newWin.Owner = this;

            newWin.Width = 300;
            newWin.Height = 300;
            newWin.ResizeMode = ResizeMode.CanMinimize;
            newWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            newWin.Content = winContent;

            // Modal
            newWin.ShowDialog();

            // Non-Modal
            // newWin.Show();
        }

        private void mnuLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.Hide();

            RadWindow loginWin = new LoginView();
            loginWin.Owner = this;
            loginWin.ResizeMode = ResizeMode.CanMinimize;
            loginWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            loginWin.ShowDialog();
        }
    }
}