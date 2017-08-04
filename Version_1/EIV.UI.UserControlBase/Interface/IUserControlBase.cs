
namespace EIV.UI.UserControlBase.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Telerik.Windows.Controls;
    public interface IUserControlBase
    {
        System.Windows.Controls.UserControl MainInterface { get; }

        // Rows
        ObservableCollection<object> GridContent { get; }

        // Telerik controls
        // GridViewColumn
        IList<GridViewBoundColumnBase> GridColumns { get; }

        // E.g. en-GB, es-AR, it, etc
        // '' means neutral culture
        string CurrentCulture { get; set; }
        string CurrentUICulture { get; set; }
        string CurrentTheme { get; set; }

        // E.g. Mutual SanCor, Banco Coignag, etc
        // The value here should be the full 'path' (Rootnamespace.Folder name.Lexicon name)
        string CurrentLexicon { get; set; }

        void Initialize();

        void Dispose();

        // Non-modal
        void Show();

        // Modal
        void ShowDialog();

        // ABM form
        //void ShowDataForm();

        void Refresh();

        event EventHandler SaveEvent;
        void Insert(object entity);
        void Update(object entity);
        void Delete(object entity);

        void SaveChanges(IEnumerable<object> entities);

        // Three main methods
        // Circular dependency here?
        // IServiceResponse Insert(object entity);
        //void Insert(object entity);
        //void Update(object entity);
        //void Delete(object entity);

        //void SaveChanges(IEnumerable<object> entities);
    }
}