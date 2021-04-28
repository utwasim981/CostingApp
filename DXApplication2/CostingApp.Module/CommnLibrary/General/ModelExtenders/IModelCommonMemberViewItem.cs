using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.CommonLibrary.General.ModelExtenders {
    public interface IModelListViewColumnOptions {
        [Category("WXaf"), DefaultValue(true)]
        bool AllowSort { get; set; }
        [Category("WXaf"), DefaultValue(true)]
        bool AllowGroup { get; set; }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ListViewColumnOptionsAttribute : ModelExportedValuesAttribute {
        public ListViewColumnOptionsAttribute(bool allowSort, bool allowGroup) {
            this.AllowSort = allowSort;
            this.AllowGroup = allowGroup;
        }
        public bool AllowSort { get; set; }
        public bool AllowGroup { get; set; }
        public override void FillValues(Dictionary<string, object> values) {
            values.Add("AllowSort", AllowSort);
            values.Add("AllowGroup", AllowGroup);
        }
    }

    public class ListViewColumnOptionsController : ViewController<ListView>, IModelExtender {
        private void ListView_ModelChanged(object sender, EventArgs e) {
            UpdateListViewColumnOptions();
        }
        protected override void OnActivated() {
            base.OnActivated();
            View.ModelChanged += ListView_ModelChanged;
        }
        protected override void OnDeactivated() {
            View.ModelChanged -= ListView_ModelChanged;
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            UpdateListViewColumnOptions();
        }
        protected virtual void UpdateListViewColumnOptions() {
            ColumnsListEditor columnsListEditor = View.Editor as ColumnsListEditor;
            if (columnsListEditor != null) {
                foreach (ColumnWrapper columnWrapper in columnsListEditor.Columns) {
                    IModelListViewColumnOptions options = columnsListEditor.Model.Columns[columnWrapper.PropertyName] as IModelListViewColumnOptions;
                    if (options != null) {
                        if (columnWrapper.AllowSortingChange && !options.AllowSort) {
                            columnWrapper.AllowSortingChange = false;
                            columnWrapper.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                            columnWrapper.SortIndex = -1;
                        }
                        if (columnWrapper.AllowGroupingChange && !options.AllowGroup) {
                            columnWrapper.AllowGroupingChange = false;
                            columnWrapper.GroupIndex = -1;
                        }
                    }
                }
            }
        }
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelCommonMemberViewItem, IModelListViewColumnOptions>();
        }
    }
}
