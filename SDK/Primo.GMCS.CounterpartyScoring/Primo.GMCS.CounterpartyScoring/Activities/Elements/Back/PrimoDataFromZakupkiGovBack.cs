using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
using LTools.WebBrowser.Elements;
using LTools.WebBrowser.InternetExplorer;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using ValidationResult = LTools.Common.Model.ValidationResult;

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Back
{
    public class PrimoDataFromZakupkiGovBack : PrimoComponentSimple<PrimoDataFromZakupkiGov>
    {
        public override string GroupName
        {
            get => ContainersData.SitesGroupName;
            protected set { }
        }

        private IWFContainer _container;

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
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(ZakupkiGovResult))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(OUT_LIST_RESULT)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, OUT_LIST_RESULT); }
        }
        private string _result;

        #endregion

        public PrimoDataFromZakupkiGovBack(IWFContainer container) : base(container)
        {
            this._container = container;
            sdkComponentName = "Получить данные zakupki.gov.ru";
            sdkComponentHelp = "Получить данные zakupki.gov.ru";
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
                    ToolTip = "Результат", IsReadOnly = false
                },
            };
            InitClass(container);
        }


        public override ExecutionResult SimpleAction(ScriptingData sd)
        {

            BrowserInst browser = null;

            if (string.IsNullOrEmpty(this.Browser) == false)
                browser = GetPropertyValue<LTools.WebBrowser.BrowserInst>(this.Browser, BROWSER_ELEMENT_PROPERTIE, sd);

            if (browser == null)
            {
                browser = GetContainerOfType<WFOpenBrowser>(_container)?.Browser;
                if (browser == null)
                    browser = GetContainerOfType<WFAttachBrowser>(_container)?.Browser;
                if (browser == null)
                    return new ExecutionResult() { IsSuccess = false, ErrorMessage = "Браузер не обнаружен!" };

                SetVariableValue<BrowserInst>(Browser, browser, sd);
            }

            string url = browser.Eval("window.location.href");
            LTools.Workflow.Elements.WFAddToLog.AddToLog(sd.WorkflowVar, $"URL: {url}", LTools.Enums.LogMessageType.Info);

            Thread.Sleep(1000);

            string jsRequestStart = "document.getElementsByClassName('blockInfo__table tableBlock')[0].getElementsByTagName('td')[%id%].innerText;";

            string court = browser.Eval("document.getElementsByClassName('blockInfo__table tableBlock')[0].getElementsByTagName('td')[0].innerText;");
            string number = browser.Eval(jsRequestStart.Replace("%id%", "1"));
            string startDate = browser.Eval(jsRequestStart.Replace("%id%", "2"));
            string executeDate = browser.Eval(jsRequestStart.Replace("%id%", "3"));

            var cultureInfo = new CultureInfo("de-DE");
            var result = new ZakupkiGovResult(true)
            {
                Court = court,
                Number = number, 
                StartDate = DateTime.Parse(startDate, cultureInfo),
                ExecuteDate = DateTime.Parse(executeDate, cultureInfo)
            };
            SetVariableValue<Models.SiteData.ZakupkiGovResult>(Result, result, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"result: " };

        }


        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            return ret;
        }
    }
}