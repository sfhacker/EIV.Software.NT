/// <summary>
/// 
/// </summary>
namespace EIV.UI.UserControlBase.ViewModel
{
    using Interface;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using Telerik.Windows.Controls;

    public sealed class UserControlViewModel : System.Windows.Controls.ViewBase
    {
        private readonly IUserControlBase viewModelConcreteForm = null;

        private ObservableCollection<object> testContent = null;

        public event System.EventHandler SaveEvent = null;

        // Parece rebuscado!
        public UserControlViewModel(IUserControlBase concreteForm)
        {
            this.viewModelConcreteForm = concreteForm;

            this.testContent = new ObservableCollection<object>(this.viewModelConcreteForm.GridContent);
        }

        public IList<GridViewBoundColumnBase> GridColumns
        {
            get
            {
                return this.viewModelConcreteForm.GridColumns;
            }
        }

        public ObservableCollection<object> GridContent
        {
            get
            {
                return this.testContent;
            }
        }
        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        public void Insert(object entity)
        {
            this.viewModelConcreteForm.Insert(entity);
            //OnSave(new System.EventArgs());
        }

        public void Update(object entity)
        {
            this.viewModelConcreteForm.Update(entity);
            //OnSave(new System.EventArgs());
        }

        public void Delte(object entity)
        {
            this.viewModelConcreteForm.Delete(entity);
            //OnSave(new System.EventArgs());
        }

        public void SaveChanges(object entity)
        {
            this.viewModelConcreteForm.SaveChanges(null);
            //OnSave(new System.EventArgs());
        }

        private void OnSave(System.EventArgs eventArgs)
        {
            if (SaveEvent != null)
            {
                SaveEvent(this, eventArgs);
            }
        }
    }
}