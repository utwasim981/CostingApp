using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Utils.Frames;

namespace CostingApp.Module.Win.ViewItems {
    [ViewItem(typeof(IModelHintPanel))]
    public class HintPanelViewItem : ViewItem {
        readonly IModelHintPanel _model;

        public HintPanelViewItem(IModelHintPanel modelHintPanel, Type objectType)
            : base(objectType, modelHintPanel.Id) {
            _model = modelHintPanel;
        }
        #region Overrides of ViewItem
        protected override object CreateControlCore() {
            return new NotePanel8_1 { Text = _model.Text };
        }
        #endregion
        public IModelHintPanel Model {
            get { return _model; }
        }
    }

    public interface IModelHintPanel : IModelViewItem {
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        string Text { get; set; }
    }
}
