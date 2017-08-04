
namespace EIV.UI.UserControlBase.Base
{
    using EIV.UI.UserControlBase.Interface;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Telerik.Windows.Controls;
    using System;

    public abstract class UserControlBase<T> : IUserControlBase where T : class
    {
        private readonly T obj;

        public abstract event EventHandler SaveEvent;

        // Testing ... 2017-07-20
        // public abstract event EventHandler LanguageChangedEvent;

        //public abstract ObservableCollection<T> GridContent { get; }

        public abstract string CurrentCulture { get; set; }
        public abstract string CurrentUICulture { get; set; }
        public abstract string CurrentTheme { get; set; }
        public abstract string CurrentLexicon { get; set; }

        public abstract IList<GridViewBoundColumnBase> GridColumns { get; }
        public abstract ObservableCollection<object> GridContent { get; }

        public abstract void Initialize();

        public abstract void Dispose();

        public abstract void Show();

        public abstract void ShowDialog();
        public abstract void Refresh();

        public virtual void ChangeCurrentTheme()
        {
        }

        //public abstract void Save(object entity);

        // IODataResponse
        public abstract void Update(object entity);
        public abstract void Insert(object entity);
        public abstract void Delete(object entity);
        public abstract void SaveChanges(IEnumerable<object> entities);

        public abstract System.Windows.Controls.UserControl MainInterface { get; }
    }
}