using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
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
            get => ContainersData.SitesGroupName;
            protected set { }
        }

        #region properties
        /// <summary>
        /// Название перменной браузера
        /// </summary>
        private const string browserElementPropertie = "Browser";

        private string _browser;

        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(string))]
        [System.ComponentModel.Category(ContainersData.InputCategoryName), System.ComponentModel.DisplayName(browserElementPropertie)] //ok
        public string Browser
        {
            get { return this._browser; }
            set { this._browser = value; this.InvokePropertyChanged(this, browserElementPropertie); }
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
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.VARIABLE, // ok
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string), 
                    ToolTip = "Браузер с таблицей на открытой странице", IsReadOnly = false
                }
            };
            InitClass(container);
        }

        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            string hasTable = GetPropertyValue<LTools.WebBrowser.BrowserInst>(this.Browser,browserElementPropertie,sd).Eval("document.getElementById('table').getElementsByTagName('tr').length");
            if (hasTable == "0") return new ExecutionResult() { IsSuccess = false, ErrorMessage = "На странице браузера нет таблицы" };
            else return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"hasTable: {hasTable}" };
        }

        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            //if(this.Browser == null) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = browserElementPropertie, Error = "Браузер не выбран" });

            return ret;
        }
    }
}