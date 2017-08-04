/// <summary>
/// 
/// </summary>
namespace EIV.UI.Formularios.Localidad
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EIV.UI.UserControlBase.Base;
    using Telerik.Windows.Controls;
    using ServiceContext.Interface;
    using UserControlBase.View;

    // com.cairone.odataexample.Localidad
    public sealed class LocalidadABM : UserControlBase<com.eiva.financiero.Localidad>
    {
        private const string ENTITY_SET_NAME = "Localidades";

        private readonly IUserControlService templateService = null;

        private IList<GridViewBoundColumnBase> gridColumns = null;
        private ObservableCollection<object> gridContent = null;

        private UserControlView userForm = null;

        private System.Windows.Controls.UserControl myMainInterface = null;

        // Testing events here ... 2017-08-03
        public event EventHandler MyEvent = null;
        public override event EventHandler SaveEvent;

        // Required by 'Activator.CreateInstance'
        // Use Initialize
        public LocalidadABM()
        {
        }
        public LocalidadABM(IUserControlService serviceContext)
        {
            if (serviceContext != null)
            {
                this.templateService = serviceContext;
            }
        }

        public override string CurrentCulture
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string CurrentLexicon
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string CurrentTheme
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string CurrentUICulture
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override IList<GridViewBoundColumnBase> GridColumns
        {
            get
            {
                return this.gridColumns;
            }
        }

        public override ObservableCollection<object> GridContent
        {
            get
            {
                return this.gridContent;
            }
        }

        public override System.Windows.Controls.UserControl MainInterface
        {
            get
            {
                // TODO:
                return this.myMainInterface;
            }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            // TODO:
            this.gridColumns = this.GenerateColumnsLocal();

            this.gridContent = this.LoadEntities();

            // Testing ...2017-08-02
            // TODO:
            // this.userForm = new UserControlBase(this);
            // this.userForm = new UserControlView(this.templateService);
            //this.userForm = new UserControlView();               // new UserControlBaseTab(this);

            //this.myMainInterface = new System.Windows.Controls.UserControl();

            //this.myMainInterface.Content = userForm;  //    stackPanel;
        }

        public override void Refresh()
        {
            // TODO:
        }

        public override void Show()
        {
            // TODO: 
        }

        public override void ShowDialog()
        {
            // Test this ....
            // Deberia tomar el culture del main app
            // Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");   // en
            // Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");   // en-US

            // System.Windows.Window
            //RadWindow thisWindow = new UserControlDataForm();
            //thisWindow.Header = "Localidades ABM";
            //thisWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //thisWindow.ResizeMode = System.Windows.ResizeMode.CanMinimize;

            // Test this .....
            //thisWindow.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

            // 2017-08-02
            var viewModel = new UI.UserControlBase.ViewModel.UserControlViewModel(this);

            // TODO: 
            viewModel.SaveEvent += ViewModel_SaveEvent;

            // var testOne = new UserControlView(viewModel);
            RadWindow testOne = new UserWindowView(viewModel);
            //testOne.Owner = this;   // TODO: MainApp here!
            testOne.Header = "Localidades ABM";
            testOne.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            testOne.Height = 400;
            testOne.Width = 300;
            //testOne.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            //testOne.DataContext = viewModel;

            testOne.ShowDialog();
            //this.userForm.ShowDialog(thisWindow);
        }

        private void ViewModel_SaveEvent(object sender, EventArgs e)
        {
            var testEvent = "·";

            // TODO:
            //this.templateService.Insert(ENTITY_SET_NAME, null);
        }

        #region Private Methods

        // Testing events ... 2017-08-03
        protected void OnMyEvent()
        {
            if (this.MyEvent != null)
                this.MyEvent(this, EventArgs.Empty);
        }

        // TODO:
        private ObservableCollection<object> LoadEntities()
        {
            IDictionary<string, object> filters = null;
            if (this.templateService != null)
            {
                filters = new Dictionary<string, object>();
                filters.Add("$expand", "Provincia");
                //filters.Add("$orderby", "Descripcion");

                return this.templateService.LoadAllEntities<com.eiva.financiero.Localidad>(ENTITY_SET_NAME, null, filters);
            }

            var rst = new ObservableCollection<object>();

            return rst;
        }
        private ObservableCollection<GridViewBoundColumnBase> GenerateColumnsLocal()
        {
            ObservableCollection<GridViewBoundColumnBase> rst = new ObservableCollection<GridViewBoundColumnBase>();

            // rst.Add(new GridViewDataColumn() { Header = "{Pais Id}", UniqueName = "localidad_paisId", DataMemberBinding = new System.Windows.Data.Binding("paisId") });
            // rst.Add(new GridViewDataColumn() { Header = "Provincia Id", UniqueName = "localidad_provinciaId", DataMemberBinding = new System.Windows.Data.Binding("provinciaId") });

            //rst.Add(new GridViewDataColumn() { Header = "Id", UniqueName = "localidad_id", DataMemberBinding = new System.Windows.Data.Binding("Id") });
            rst.Add(new GridViewDataColumn() { Header = "Nombre", UniqueName = "localidad_nombre", DataMemberBinding = new System.Windows.Data.Binding("Descripcion") });
            rst.Add(new GridViewDataColumn() { Header = "Cod. Postal", UniqueName = "localidad_cp", DataMemberBinding = new System.Windows.Data.Binding("CodigoPostal") });
            rst.Add(new GridViewDataColumn() { Header = "Prefijo", UniqueName = "localidad_prefijo", DataMemberBinding = new System.Windows.Data.Binding("Prefijo") });

            // ItemsSourceBinding = new System.Windows.Data.Binding("provinciaId")
            // ItemsSourceBinding = new System.Windows.Data.Binding("pais")
            // ItemsSourceBinding = new System.Windows.Data.Binding("paisId")
            // ItemsSourceBinding = new System.Windows.Data.Binding("AvailableProvincias")

            // OK
            rst.Add(new GridViewComboBoxColumn() { Header = "Provincia", UniqueName = "localidad_provincia", DataMemberBinding = new System.Windows.Data.Binding("ProvinciaId") { Mode = System.Windows.Data.BindingMode.TwoWay }, SelectedValueMemberPath = "Id", DisplayMemberPath = "Provincia.Descripcion" });

            // rst.Add(new GridViewComboBoxColumn() { Name = "cboPais", Header = "Pais", UniqueName = "localidad_pais", DataMemberBinding = new System.Windows.Data.Binding("paisId"), SelectedValueMemberPath = "id", DisplayMemberPath = "nombre" });
            // rst.Add(new GridViewCheckBoxColumn() { Header = "Read Only", UniqueName = "localidad_readonly", DataMemberBinding = new System.Windows.Data.Binding("ReadOnly") });
            // rst.Add(new GridViewHyperlinkColumn() { Header = "Wikipedia", UniqueName = "localidad_wiki", DataMemberBinding = new System.Windows.Data.Binding("Wikipedia") });
            // rst.Add(new GridViewImageColumn() { Header = "Bandera", UniqueName = "localidad_bandera", DataMemberBinding = new System.Windows.Data.Binding("Bandera") });

            return rst;
        }

        public override void Insert(object entity)
        {
            var alas = "here";

            //this.templateService.Insert(ENTITY_SET_NAME, null);
            //throw new NotImplementedException();
        }

        public override void Update(object entity)
        {
            IServiceResponse resp = null;

            if (this.templateService != null)
            {
                resp = this.templateService.Update(entity);
            }
        }

        public override void Delete(object entity)
        {
            
        }

        public override void SaveChanges(IEnumerable<object> entities)
        {
            
        }
        #endregion
    }

    public sealed class LocalidadLocal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}