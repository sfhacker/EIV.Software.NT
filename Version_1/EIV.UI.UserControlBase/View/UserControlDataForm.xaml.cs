/// <summary>
/// http://docs.telerik.com/devtools/wpf/controls/radwindow/how-to/use-radwindow-as-user-control
/// </summary>
namespace EIV.UI.UserControlBase
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Data;
    using Telerik.Windows.Controls;

    /// <summary>
    /// Lógica de interacción para UserControlDataForm.xaml
    /// </summary>
    public partial class UserControlDataForm : RadWindow
    {
        private RadGridView gridView = null;

        private object tempEntity = null;
        private IList<object> itemsToDelete = null;

        public event System.EventHandler saveEvent = null;

        public UserControlDataForm()
        {
            InitializeComponent();
        }

        public UserControlDataForm(RadGridView grid) : this()
        {
            if (grid == null)
            {
                return;
            }

            this.gridView = grid;

            //InitializeComponent();
        }

        public event System.EventHandler OnSaveEvent
        {
            add
            {

            }
            remove
            {

            }
        }

        //public void ShowDialog(Window container)
        //{
        //    if (container == null)
        //    {
        //        return;
        //    }
        //    if (this.gridView == null)
        //    {
        //        return;
        //    }

        //    //this.popUpDataForm.ItemsSource = this.gridView.Items;

        //    //this.popUpDataForm.CurrentItem = this.gridView.CurrentItem;

        //    var newContent = new RadWindow();               // UserControl();
        //    //newContent.Content = new DetailsPresenter()
        //    //{
        //    //    DetailsProvider = this.gridView.RowDetailsProvider
        //    //};

        //    newContent.Content = this;

        //    container.Content = newContent;      // this.Content;

        //    container.ShowDialog();
        //}

        private void radDataForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.gridView != null)
            {
                this.radDataForm.ItemsSource = this.gridView.Items;
                this.radDataForm.CurrentItem = this.gridView.CurrentItem;
            }
        }

        private void radDataForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            ViewModel.UserControlViewModel myViewModel = this.DataContext as ViewModel.UserControlViewModel;

            var entity = this.radDataForm.CurrentItem as object;
            if (entity != null)
            {
                if (tempEntity != null)
                {
                    if (entity.Equals(tempEntity))
                    {
                        if (myViewModel != null)
                        {
                            myViewModel.Insert(entity);
                        }

                        //if (this.genericTemplate != null)
                        //{
                        //    // TODO:
                        //    IODataResponse resp = this.genericTemplate.Insert(entity);
                        //    if (resp != null)
                        //    {
                        //        string respMessage = string.Format("{0}: {1}\r\n\r\n{2}", resp.StatusCode, resp.Message, resp.Error);

                        //        int errorValue = (int) MessageBoxImage.Information;
                        //        if (resp.StatusCode > 299 || resp.StatusCode < 200)
                        //        {
                        //            errorValue = (int) MessageBoxImage.Error;
                        //        }
                        //        MessageBox.Show(respMessage, "Nuevo Registro", MessageBoxButton.OK, (MessageBoxImage) errorValue);
                        //    }
                        //}
                    }
                    tempEntity = null;

                    return;
                }


                //MessageBox.Show("Se procedera a guardar los cambios", "Guardar cambios");

                if (myViewModel != null)
                {
                    myViewModel.Update(entity);
                }

                //if (this.genericTemplate != null)
                //{
                //    // Refactor
                //    IODataResponse resp = this.genericTemplate.Update(entity);
                //    if (resp != null)
                //    {
                //        string respMessage = string.Format("{0}: {1}\r\n\r\n{2}", resp.StatusCode, resp.Message, resp.Error);

                //        int errorValue = (int) MessageBoxImage.Information;
                //        if (resp.StatusCode > 299 || resp.StatusCode < 200)
                //        {
                //            errorValue = (int) MessageBoxImage.Error;
                //        }
                //        MessageBox.Show(respMessage, "Actualizar Registro", MessageBoxButton.OK, (MessageBoxImage) errorValue);
                //    }
                //}
            }
        }

        private void radDataForm_DeletedItem(object sender, Telerik.Windows.Controls.Data.DataForm.ItemDeletedEventArgs e)
        {
            var entity = e.DeletedItem;
            if (entity != null)
            {
                this.itemsToDelete.Add(entity);
            }
        }

        private void radDataForm_AddedNewItem(object sender, Telerik.Windows.Controls.Data.DataForm.AddedNewItemEventArgs e)
        {
            var entity = e.NewItem;
            if (entity != null)
            {
                tempEntity = entity;
                // this.itemsToDelete.Add(entity);
            }
        }

        // Esto no deberia estar aqui!
        private void radDataForm_AutoGeneratingField(object sender, Telerik.Windows.Controls.Data.DataForm.AutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName == "Provincia")
            {
                // Funca OK
                e.DataField.DataMemberBinding = new Binding("Provincia.Descripcion");
            }
        }
    }
}