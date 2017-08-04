
namespace EIV.UI.MainApp.View
{
    using System.Windows;
    using Telerik.Windows.Controls;
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : RadWindow
    {
        private const string DEFAULT_USERNAME = "admin";
        private const string DEFAULT_PASSWORD = "qwerty";

        public LoginView()
        {
            InitializeComponent();

            this.txtUserName.Text = "";
            this.txtUserName.Password = "";
            this.txtUserName.MaxLength = 32;
            this.txtUserName.ShowPasswordButtonVisibility = ShowPasswordButtonVisibilityMode.Never;
            this.txtUserName.WatermarkBehavior = WatermarkBehavior.HideOnTextEntered;

            this.txtPassword.Text = "";
            this.txtPassword.Password = "";
            this.txtPassword.MaxLength = 32;
            this.txtPassword.ShowPasswordButtonVisibility = ShowPasswordButtonVisibilityMode.Never;
            this.txtPassword.WatermarkBehavior = WatermarkBehavior.HideOnTextEntered;

            this.cboSeguridad.SelectedIndex = 0;
            this.cboSucursal.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string strUserName = this.txtUserName.Password;
            string strPassword = this.txtPassword.Password;
            string strSeguridad = null;

            if (string.IsNullOrEmpty(strUserName))
            {
                RadWindow.Alert("Nombre de Usuario invalido");

                return;
            }
            if (string.IsNullOrEmpty(strPassword))
            {
                RadWindow.Alert("Password invalido");

                return;
            }

            if (this.cboSeguridad.SelectedItem != null)
            {
                RadComboBoxItem cboItem = this.cboSeguridad.SelectedItem as RadComboBoxItem;
                strSeguridad = cboItem.Content as string;
            }

            string txtMsg = string.Format("{0};{1};{2}", strUserName, strPassword, strSeguridad);

            MessageBox.Show(txtMsg);
            if (strUserName.Equals(DEFAULT_USERNAME) && strPassword.Equals(DEFAULT_PASSWORD))
            {
                RadWindow.Alert("Login Ok!");

                return;
            }

            RadWindow.Alert("You shall not pass");
        }

        private void cboSeguridad_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int cboIndex = this.cboSeguridad.SelectedIndex;

            if (cboIndex > 0)
            {
                this.txtUserName.Password = "";
                this.txtUserName.IsEnabled = false;

                this.txtPassword.Password = "";
                this.txtPassword.IsEnabled = false;

                // this.stackSucursal.Visibility = Visibility.Visible;
                //this.lblSucursal.Visibility = Visibility.Visible;
                //this.cboSucursal.Visibility = Visibility.Visible;

                return;
            }

            this.txtUserName.IsEnabled = true;
            this.txtPassword.IsEnabled = true;

            //this.stackSucursal.Visibility = Visibility.Collapsed;

            //this.lblSucursal.Visibility = Visibility.Hidden;
            //this.cboSucursal.Visibility = Visibility.Hidden;
        }

        private void cboSucursal_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}