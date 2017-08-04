
namespace EIV.UI.UserControlBase.View
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Data;
    using Telerik.Windows.Controls;
    using Telerik.Windows.Controls.GridView;
    using ViewModel;

    /// <summary>
    /// Lógica de interacción para UserWindowView.xaml
    /// </summary>
    /// UserControl
    public partial class UserWindowView : RadWindow
    {
        private UserControlViewModel myViewModel = null;
        public UserWindowView()
        {
            InitializeComponent();
        }

        public UserWindowView(UserControlViewModel viewModel)    // : this()
        {
            InitializeComponent();

            this.myViewModel = viewModel;
        }

        #region Grid Events
        private void gridView_DataLoading(object sender, Telerik.Windows.Controls.GridView.GridViewDataLoadingEventArgs e)
        {
            //this.gridStatusInfo = "Cargando datos ...";

            GridViewDataControl dataControl = (GridViewDataControl) sender;
            if (dataControl != null)
            {
                dataControl.IsBusy = true;
            }
        }

        private void gridView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (this.genericTemplate == null)
            //{
            //    return;
            //}
            GridViewDataControl dataControl = (GridViewDataControl) sender;
            if (dataControl != null)
            {
                //dataControl.ItemsSource = genericLocalidades.
                // how often does it come here?
                //dataControl.Columns.Clear();
                dataControl.Columns.Add(new MyButtonColumn(new RoutedEventHandler(this.ButtonTest_Click))
                {
                    Header = "Editar"
                });

                // TODO: 2017-07-31 + 2017-08-02
                dataControl.Columns.AddRange(this.myViewModel.GridColumns);    // this.gridCols

                // Stack Overflow!
                dataControl.ItemsSource = this.myViewModel.GridContent;  // this.gridContent;

                this.dataPager.SetBinding(RadDataPager.SourceProperty, new Binding("Items") { Source = this.gridView });

                // TODO:
                /*
                // CreateDataPager(dataControl);
                this.BindDataPager(this.gridPager, dataControl);

                ObservableCollection<Pais> items = this.LoadPaises();

                SetGridSource(dataControl, items);
                */
            }
        }

        private void gridView_Deleting(object sender, Telerik.Windows.Controls.GridViewDeletingEventArgs e)
        {
            if (e.Items != null)
            {
                //this.genericLocalidades.Delete(e.Items);

                // TODO:
                /* User can delete more than one row at the same time */
                /*List<Pais> paises = e.Items.Cast<Pais>().ToList();

                if (paises != null)
                {
                    if (paises.Count > 0)
                    {
                        /* this.customViewModel.RemoveEntities<Pais>(paises); */
                /*  }
              }*/
            }
        }

        private void gridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            // TODO:
            //Pais newPais = null;

            if (e.EditAction == GridViewEditAction.Cancel)
            {
                e.UserDefinedErrors.Clear();

                return;
            }

            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                GridViewRow row = e.Row;
                if (e.Row is GridViewNewRow)
                {
                    object cellValue = e.OldValues;

                    //this.genericLocalidades.Insert(e.NewData);
                }

                return;
            }

            if (e.EditOperationType == GridViewEditOperationType.Edit)
            {
                GridViewRow row = e.Row;
                if (e.Source is GridViewComboBoxColumn)
                {
                    var one = "alas";
                }
                IDictionary<string, object> cellOldValues = e.OldValues;
                object cellNewValues = e.NewData;

                // Issue when changing an item in any Combo Box
                //this.genericLocalidades.Update(e.NewData);
            }
        }

        private void gridView_DataLoaded(object sender, System.EventArgs e)
        {
            //this.gridStatusInfo = "Carga de datos completa!";

            GridViewDataControl dataControl = (GridViewDataControl) sender;
            if (dataControl != null)
            {
                dataControl.IsBusy = false;
            }
        }

        private void gridView_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (this.gridView.CurrentCell != null)
            {
                GridViewCell cellIndex = e.Cell;
                GridViewRowItem rowItem = e.Row;
                //object initialCellValue = e.Row.Cells[cellIndex].Value;
                //if (!initialValues.ContainsKey(cellIndex))
                //{
                //    initialValues.Add(cellIndex, initialCellValue);
                //}

                //this.gridView.rebi
            }
        }

        private void gridView_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
        }

        private void gridView_CurrentCellChanged(object sender, GridViewCurrentCellChangedEventArgs e)
        {

        }

        private void gridView_CellValidated(object sender, GridViewCellValidatedEventArgs e)
        {
        }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            RadWindow.Alert("here new 2017");

            var senderElement = e.OriginalSource as FrameworkElement;
            var row = senderElement.ParentOfType<GridViewRow>();

            if (row != null)
            {
                row.IsSelected = true;
                this.gridView.CurrentItem = row.DataContext;

                RadWindow dataFormPopUp = new UserControlDataForm(this.gridView);
                dataFormPopUp.Owner = this;
                dataFormPopUp.Header = "Localidades";

                dataFormPopUp.DataContext = this.myViewModel;
                dataFormPopUp.ShowDialog();
            }
        }
        #endregion
    }

    public sealed class MyButtonColumn : Telerik.Windows.Controls.GridViewColumn
    {
        private readonly EventHandler btnClickEvent = null;
        private readonly RoutedEventHandler btnRoutedClickEvent = null;

        public MyButtonColumn()
        {

        }

        //public MyButtonColumn(EventHandler clickEvent)
        //{
        //    this.btnClickEvent = clickEvent;
        //}

        public MyButtonColumn(RoutedEventHandler routedClickEvent)
        {
            this.btnRoutedClickEvent = routedClickEvent;
        }

        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            RadButton button = cell.Content as RadButton;
            if (button == null)
            {
                button = new RadButton();
                button.Content = "Editar";
                //button.Command = RadGridViewCommands.Search;
                if (this.btnRoutedClickEvent != null)
                {
                    button.Click += this.btnRoutedClickEvent;  // new RoutedEventHandler(btnClickEvent);             // Button_Click;
                }
                
            }

            button.CommandParameter = dataItem;

            return button;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    RadWindow.Alert("here");

        //    var senderElement = e.OriginalSource as FrameworkElement;
        //    var row = senderElement.ParentOfType<GridViewRow>();

        //    if (row != null)
        //        row.IsSelected = true;
        //}
    }
}