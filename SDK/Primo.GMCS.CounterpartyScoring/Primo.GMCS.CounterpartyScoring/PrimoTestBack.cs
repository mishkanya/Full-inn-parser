using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ValidationResult = LTools.Common.Model.ValidationResult;

namespace Primo.GMCS.CounterpartyScoring
{
    public class PrimoTestBack : PrimoComponentSimple<PrimoTest>
    {
        public override string GroupName
        {
            //get => $"GMCS{LTools.Common.UIElements.WFPublishedElementBase.TREE_SEPARATOR}Sites";
            get => $"GMCS/Sites";
            protected set { }
        }

        private string prop1;
        /// <summary>
        /// Property My Prop 1
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(string))]
        [System.ComponentModel.Category("SDK"), System.ComponentModel.DisplayName("My Prop 1")]
        public string Prop1
        {
            get { return this.prop1; }
            set { this.prop1 = value; this.InvokePropertyChanged(this, "Prop1"); }
        }

        public PrimoTestBack(IWFContainer container) : base(container)
        {
            sdkComponentName = "My sync element";
            sdkComponentHelp = "This is my first sync element";
            sdkComponentIcon = "pack://application:,,/Primo.GMCS.CounterpartyScoring;component/Images/sample.png";
            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = "Prop1",
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string), ToolTip = "SDK Tooltip1", IsReadOnly = false
                }
            };
            InitClass(container);
            this.Prop1 = this.IsNoCode("Prop1") ? "test text" : "\"test text\"";
        }

        /// <summary>
        /// Main action
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            try
            {
                string p1 = GetPropertyValue<string>(this.Prop1, "Prop1", sd);
                System.Windows.MessageBox.Show(p1);
                return new ExecutionResult() { IsSuccess = true, SuccessMessage = "Done" };
            }
            catch (Exception ex)
            {
                return new ExecutionResult() { IsSuccess = false, ErrorMessage = ex?.Message };
            }
        }

        /// <summary>
        /// Syntax check
        /// </summary>
        /// <returns></returns>
        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            if (String.IsNullOrEmpty(this.Prop1)) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = "My Prop 1", Error = "Text not specified" });
            return ret;
        }
    }
}