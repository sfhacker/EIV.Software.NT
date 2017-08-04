
namespace EIV.UI.ServiceContext.Service
{
    using EIV.UI.ServiceContext.Interface;
    using com.eiva.financiero;
    using Microsoft.OData.Client;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public sealed class UserControlService : IUserControlService
    {
        // TODO: ..\\..\\..\\EIV.UI.UserTemplates.Tests\\bin\\Debug\\
        // Se puede especificar mas de un assembly, separado x ';'
        private const string USER_TEMPLATES_ASSEMBLY = @"..\..\..\EIV.UI.Formularios\bin\Debug\EIV.UI.Formularios.dll";

        private const string HEADER_AUTHORIZATION = "Authorization";

        private const string DEFAULT_USERNAME = "admin";
        private const string DEFAULT_PASSWORD = "qwerty";

        private Assembly formulariosAssembly = null;

        // private DataServiceContext container = null;
        private EivaFinanciero container = null;

        private string cultureName = null;
        private string cultureUIName = null;
        private string themeName = null;
        private string lexiconName = null;

        private bool isConnected = false;

        private string userName = null;
        private string password = null;
        private string domainName = null;

        // este tipo de constructor se puede utilizar para testing
        // ya que no requiere conexion a ningun data source
        public UserControlService()
        {
            this.SetDefaults();

            this.LoadFormulariosAssembly();
        }

        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        public string Password
        {
            internal get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public string DomainName
        {
            get
            {
                return this.domainName;
            }
            set
            {
                this.domainName = value;
            }
        }

        public bool Connect(string serviceUri)
        {
            Uri thisUri = null;

            if (string.IsNullOrEmpty(serviceUri))
            {
                return false;
            }

            bool rst = Uri.TryCreate(serviceUri, UriKind.Absolute, out thisUri);
            if (!rst)
            {
                return false;
            }
            if (this.isConnected)
            {
                return true;
            }

            try
            {
                // EIV.OData.Proxy
                // TODO: eifvinanciero
                //this.container = new ODataExample(serviceUri);
                this.container = new EivaFinanciero(thisUri);

                this.container.Format.UseJson();
                this.container.SaveChangesDefaultOptions = SaveChangesOptions.ReplaceOnUpdate;

                this.container.SendingRequest2 += Container_SendingRequest2;

                this.container.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

                this.isConnected = true;

                return true;
            }
            catch (Exception ex)
            {
                // TODO
                //this.errors.Add(ex);
            }

            return false;
        }

        private void Container_SendingRequest2(object sender, SendingRequest2EventArgs e)
        {
            string svcCredentials = string.Format("{0}:{1}", DEFAULT_USERNAME, DEFAULT_PASSWORD);

            byte[] credBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(svcCredentials);

            string headerValue = string.Format("Basic {0}", Convert.ToBase64String(credBytes));

            e.RequestMessage.SetHeader(HEADER_AUTHORIZATION, headerValue);
        }

        public void Disconnect()
        {
            if (!this.isConnected)
            {
                return;
            }

            this.container = null;

            this.isConnected = false;
        }

        // permite realizar queries a cualquier entidad, con cualquier no de params (e.g. id=23) y filtros (e.g. $expand)
        public ObservableCollection<object> LoadAllEntities<T>(string entitySetName, IDictionary<string, object> entityParams, IDictionary<string, object> filters) where T : class
        // public IList<object> LoadAllEntities<T>(string entitySetName, IDictionary<string, object> entityParams, IDictionary<string, object> filters) where T : class
        {
            UriOperationParameter[] myParams = null;

            IEnumerable<object> result = null;

            DataServiceQuery<T> query = null;

            if (this.container == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                return null;
            }
            if (entityParams == null)
            {
                query = this.container.CreateQuery<T>(entitySetName);
            }
            else
            {
                int count = entityParams.Count;
                if (count > 0)
                {
                    myParams = new UriOperationParameter[count];

                    int i = 0;
                    foreach (string paramKey in entityParams.Keys)
                    {
                        // check for null value here
                        myParams[i++] = new UriOperationParameter(paramKey, entityParams[paramKey]);
                    }

                    query = this.container.CreateFunctionQuery<T>("", entitySetName, false, myParams);
                }
            }
            if (query == null)
            {
                return null;
            }
            if (filters != null)
            {
                foreach (string filter in filters.Keys)
                {
                    object filterValue = filters[filter];
                    if (filterValue != null)
                    {
                        // Immutability here ?
                        query = query.AddQueryOption(filter, filterValue);
                    }
                }
            }

            try
            {
                // 1.- Query
                // The service is down or any other network related issue(s)
                result = query.Execute();

                // Synchronous operation ???
                var list = result.ToList();

                // return list;
                return new ObservableCollection<object>(list);
            }
            catch (Exception ex)
            {
                //this.statusInfo.Text = ex.Message;
                // this.errorMessage = ex.Message;
            }

            return null;
        }

        // Se encapsulan todas las llamadas al servicio OData en esta clase
        // Puede ser overkill
        public ObservableCollection<Localidad> LoadAllLocalidades()
        {
            var list = this.container.Localidades.ToList();

            return new ObservableCollection<Localidad>(list);

        }

        public ObservableCollection<object> LoadMenuByUser(string userName)
        {
            return null;
        }

        // Circular dependency here?
        public IServiceResponse Insert(string entitySetName, object entity)
        {
            IServiceResponse queryResponse = null;

            if (string.IsNullOrEmpty(entitySetName))
            {
                return null;
            }
            if (entity == null)
            {
                return null;
            }
            if (this.container == null)
            {
                return null;
            }
            // TODO: Improve
            this.container.AddObject(entitySetName, entity);

            // This should be done by the client?
            queryResponse = this.SaveChanges();

            return queryResponse;
        }

        public IServiceResponse Update(object entity)
        {
            IServiceResponse queryResponse = null;

            if (entity == null)
            {
                return null;
            }
            if (this.container == null)
            {
                return null;
            }
            // TODO: Improve
            this.container.UpdateObject(entity);

            // This should be done by the client?
            queryResponse = this.SaveChanges();

            return queryResponse;
        }

        public IServiceResponse Delete(object entity)
        {
            if (entity == null)
            {
                return null;
            }
            if (this.container == null)
            {
                return null;
            }
            // TODO: Improve
            this.container.DeleteObject(entity);

            // TODO: Use SaveChanges method here
            //this.container.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);

            return null;
        }

        // como manejar esto ?
        // un SaveChanges por cada operacion (insert, update, ...)
        // o un solo SaveChanges por un batch de operaciones?
        public IServiceResponse SaveChanges()
        {
            DataServiceResponse resp = null;
            IServiceResponse queryResponse = null;

            if (this.container == null)
            {
                return null;
            }
            try
            {
                resp = this.container.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);

                // there should be only one!
                foreach (OperationResponse response in resp)
                {
                    string message = "Cambios guardados satisfactoriamente.";
                    int statusCode = response.StatusCode;
                    Exception respEx = response.Error;

                    // response.Headers

                    queryResponse = new ServiceResponse(statusCode, message, respEx);

                    break;
                }
            }
            catch (DataServiceRequestException e)
            {
                resp = e.Response;

                // there should be only one!
                foreach (OperationResponse response in resp)
                {
                    string message = e.Message;
                    int statusCode = response.StatusCode;
                    Exception respEx = response.Error;

                    // response.Headers

                    queryResponse = new ServiceResponse(statusCode, message, respEx);

                    break;
                }
            }
            catch (InvalidOperationException invalidOper)
            {
                queryResponse = new ServiceResponse(invalidOper);
            }
            catch (Exception Ex)
            {
                Type alas = Ex.GetType();

                queryResponse = new ServiceResponse(Ex);
            }

            return queryResponse;
        }

        public void SetCurrentCulture(string cultureName)
        {
            this.cultureName = cultureName;
        }

        public void SetCurrentUICulture(string cultureName)
        {
            this.cultureUIName = cultureName;
        }

        public void SetCurrentTheme(string themeName)
        {
            this.themeName = themeName;
        }

        public void SetCurrentLexico(string lexicoName)
        {
            this.lexiconName = lexicoName;
        }

        // In MainApp ??
        //private void CreateNewWindow(System.Windows.Controls.UserControl winContent)
        //{
        //    Window newWin = new Window();

        //    newWin.Title = "Ventana Dinamica";
        //    newWin.Owner = this;

        //    newWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        //    newWin.Content = winContent;

        //    // Modal
        //    newWin.ShowDialog();

        //    // Non-Modal
        //    // newWin.Show();
        //}

        // Donde poder esto?
        public Type FindFormulario(string formName)
        {
            Type formType = null;

            if (string.IsNullOrEmpty(formName))
            {
                return null;
            }
            if (this.formulariosAssembly == null)
            {
                return null;
            }

            formType = this.formulariosAssembly.GetType(formName);

            return formType;
        }

        public void RenderFormulario(Type formulario)
        {
            if (formulario == null)
            {
                return;
            }

            object[] argValues = new object[] { this };

            UserControlBase.Interface.IUserControlBase formBase = Activator.CreateInstance(formulario, args: argValues) as UserControlBase.Interface.IUserControlBase;

            if (formBase != null)
            {
                formBase.Initialize();
                formBase.ShowDialog();
            }
        }
        // In App.config ???
        private void LoadFormulariosAssembly()
        {
            try
            {
                this.formulariosAssembly = Assembly.LoadFrom(USER_TEMPLATES_ASSEMBLY);
            }
            catch (System.IO.FileNotFoundException notFoundEx)
            {

            }
            catch (Exception ex)
            {

            }
        }

        // TODO: Circular dependency
        //public IUserControlBase FindUserTemplate(string templateName)
        //{
        //    return null;
        //}

        private void SetDefaults()
        {
            // Will this work ok for all Windows versions?
            this.userName = Environment.UserName;
            this.domainName = Environment.UserDomainName;

            this.cultureName = "";
            this.cultureUIName = "";
            this.themeName = "";
            this.lexiconName = "";
        }
    }
}