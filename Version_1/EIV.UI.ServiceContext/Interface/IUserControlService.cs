/// <summary>
/// 
/// </summary>
namespace EIV.UI.ServiceContext.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IUserControlService
    {
        bool IsConnected { get; }

        string DomainName { get; set; }

        string UserName { get; set; }

        string Password { set; }

        Type FindFormulario(string formName);

        // Testing ...
        // Este method puede estar dentro de otro
        // SRP ???
        void RenderFormulario(Type formulario);

        bool Connect(string serviceUri);
        void Disconnect();

        // mmmm ????
        //com.eiva.financiero.EivaFinanciero ODataServiceContext { get; }

        // Generic method applicable to any entity
        ObservableCollection<object> LoadAllEntities<T>(string entitySetName, IDictionary<string, object> entityParams, IDictionary<string, object> filters) where T : class;

        IServiceResponse Insert(string entitySetName, object entity);
        IServiceResponse Update(object entity);
        IServiceResponse Delete(object entity);

        IServiceResponse SaveChanges();
    }
}