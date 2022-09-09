using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
using LTools.WebBrowser.InternetExplorer;
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
        private const string BROWSER_ELEMENT_PROPERTIE = "Browser";
        private const string OUT_LIST_RESULT = "Result";


        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(LTools.WebBrowser.BrowserInst))]
        [System.ComponentModel.Category(ContainersData.INPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(BROWSER_ELEMENT_PROPERTIE)] //ok
        public string Browser
        {
            get { return this._browser; }
            set { this._browser = value; this.InvokePropertyChanged(this, BROWSER_ELEMENT_PROPERTIE); }
        }
        private string _browser;

        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(List<Models.SiteData.KadArbitr.TableResult>))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(OUT_LIST_RESULT)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, OUT_LIST_RESULT); }
        }
        private string _result;

        #endregion

        private Dictionary<string, Models.Enums.KadArbitrTableElementType> _kadArbitrTypesDict = new Dictionary<string, Models.Enums.KadArbitrTableElementType>
        {
            {"civil", Models.Enums.KadArbitrTableElementType.Civil},
            {"administrative", Models.Enums.KadArbitrTableElementType.Administrative},
            {"bankruptcy", Models.Enums.KadArbitrTableElementType.Bankrot},
        };
        
        public PrimoDataFromKadArbitrTableBack(IWFContainer container) : base(container)
        {
            sdkComponentName = "Получить данные с сайта";
            sdkComponentHelp = "Получение списка строк из таблицы на сайте https://kad.arbitr.ru/";
            sdkComponentIcon = ContainersData.ICON_PATH;
            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = BROWSER_ELEMENT_PROPERTIE,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.VARIABLE, // ok
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = "Браузер с таблицей на открытой странице", IsReadOnly = false
                },
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = OUT_LIST_RESULT,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.VARIABLE, // ok
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = "Список строк таблицы", IsReadOnly = false
                },
            };
            InitClass(container);
        }


        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            var browser = GetPropertyValue<LTools.WebBrowser.BrowserInst>(this.Browser, BROWSER_ELEMENT_PROPERTIE, sd);

            if (browser == null) return new ExecutionResult() { IsSuccess = false, ErrorMessage = "На странице браузера нет таблицы" };

            string rowCountStr = browser.Eval("document.getElementById('table').getElementsByTagName('tr').length");

            if (rowCountStr == "0") return new ExecutionResult() { IsSuccess = false, ErrorMessage = "На странице браузера нет таблицы" };

            int rowCount = int.Parse(rowCountStr);

            List<Models.SiteData.KadArbitr.TableResult> result = new List<Models.SiteData.KadArbitr.TableResult>();

            for (int i = 0; i < rowCount; i++)
            {
                string dateStr = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('num')[0].getElementsByTagName('span')[0].innerText");

                string number = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('num')[0].getElementsByTagName('a')[0].innerText");
                string judge;
                try
                {
                    judge = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('court')[0].getElementsByClassName('judge')[0].innerText");

                }
                catch
                {
                    judge = null;
                }
                string instance;
                if (judge == null)
                {
                    instance = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('court')[0].getElementsByClassName('b-container')[0].getElementsByTagName('div')[0].innerText");

                }
                else
                {
                    instance = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('court')[0].getElementsByClassName('b-container')[0].getElementsByTagName('div')[1].innerText");


                }
                string plaintiff = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('plaintiff')[0].innerText");
                string defendant = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('respondent')[0].innerText");
                string typeStr = browser.Eval($"document.getElementById('table').getElementsByTagName('tr')[{i}].getElementsByClassName('num')[0].getElementsByTagName('div')[1].className");


                var row = new Models.SiteData.KadArbitr.TableResult()
                {
                    Id = number,
                    Date = DateTime.Parse(dateStr),
                    Judge = string.IsNullOrEmpty(judge) ? null : judge,
                    Instance = instance,
                    Plaintiff = plaintiff,
                    Defendant = defendant,
                    Type = _kadArbitrTypesDict[typeStr]
                };

                result.Add(row);
            }
            SetVariableValue<List<Models.SiteData.KadArbitr.TableResult>>(Result, result, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"result: {result.Count}" };
        }

        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();

            if (String.IsNullOrEmpty(this.Browser)) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = BROWSER_ELEMENT_PROPERTIE, Error = "Браузер не выбран" });

            return ret;
        }
    }
}