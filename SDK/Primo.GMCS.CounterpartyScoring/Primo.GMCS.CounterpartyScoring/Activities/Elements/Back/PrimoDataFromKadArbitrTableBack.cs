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

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Back
{
    public class PrimoDataFromKadArbitrTableBack : PrimoComponentSimple<PrimoDataFromKadArbitrTable>
    {
        public override string GroupName
        {
            //get => $"GMCS{LTools.Common.UIElements.WFPublishedElementBase.TREE_SEPARATOR}Sites";
            get => $"GMCS/Sites";
            protected set { }
        }

        #region properties

        private const string browserElementPropertie = "Browser";

        private LTools.WebBrowser.BrowserInst browser;

        /// <summary>
        /// Property My Prop 1
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(LTools.WebBrowser.BrowserInst))]
        [System.ComponentModel.Category("Входные данные"), System.ComponentModel.DisplayName(browserElementPropertie)]
        public LTools.WebBrowser.BrowserInst Browser
        {
            get { return this.browser; }
            set { this.browser = value; }
        }

        #endregion

        public PrimoDataFromKadArbitrTableBack(IWFContainer container) : base(container)
        {
            sdkComponentName = "Получить данные с сайта";
            sdkComponentHelp = "Получение списка строк из таблицы на сайте https://kad.arbitr.ru/";
            sdkComponentIcon = "pack://application:,,/Primo.GMCS.CounterpartyScoring;component/Images/sample.png";
            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = browserElementPropertie,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.CUSTOM,
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(LTools.WebBrowser.BrowserInst), ToolTip = "Браузер с таблицей на открытой странице", IsReadOnly = false
                }
            };
            InitClass(container);
        }

        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            string hasTable =  browser.Eval("document.getElementById('table').getElementsByTagName('tr').length");
            if (hasTable == "0") return new ExecutionResult() { IsSuccess = false, ErrorMessage = "На странице браузера нет таблицы" };
            else return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"hasTable: {hasTable}" };
        }

        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();

            if(this.Browser == null) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = browserElementPropertie, Error = "Браузер не выбран" });

            return ret;
        }
    }
}