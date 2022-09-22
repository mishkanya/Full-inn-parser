using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
using LTools.WebBrowser.Elements;
using LTools.WebBrowser.InternetExplorer;
using Newtonsoft.Json;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ValidationResult = LTools.Common.Model.ValidationResult;
using Primo.GMCS.CounterpartyScoring.Models.SiteData.PBNalog;
using LTools.WebBrowser.Model;

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Back
{
    public class PrimoFedresursGetJsonBack : PrimoComponentSimple<PrimoPBNalogGetJson>
    {
        public override string GroupName
        {
            get => ContainersData.SitesGroupName;
            protected set { }
        }

        private IWFContainer _container;

        #region properties


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
        /// Название перменной браузера
        /// </summary>
        private const string BROWSER_ELEMENT_PROPERTIE = "Browser";

        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(BankrotFedresurResult))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(OUT_LIST_RESULT)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, OUT_LIST_RESULT); }
        }
        private string _result;
        private const string OUT_LIST_RESULT = "Result";

        #endregion


        public PrimoFedresursGetJsonBack(IWFContainer container) : base(container)
        {
            this._container = container;
            sdkComponentName = "Получить данные с bankrot.fedresurs.ru";
            sdkComponentHelp = "Получение json строки с сайта bankrot.fedresurs.ru";
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
                    ToolTip = "Json строка", IsReadOnly = false
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


            string guid = url.Replace("https://fedresurs.ru/company/", "");
            string json;


            browser.Eval("fetch('https://fedresurs.ru/backend/companies/"+ guid + "', { 'headers': { 'accept': 'application/json, text/plain, */*', 'accept-language': 'ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7', 'cache-control': 'no-cache', 'pragma': 'no-cache', 'sec-ch-ua': '\\\"Google Chrome\\\";v=\\\"105\\\", \\\"Not)A;Brand\\\";v=\\\"8\\\", \\\"Chromium\\\";v=\\\"105\\\"', 'sec-ch-ua-mobile': '?0', 'sec-ch-ua-platform': '\\\"Windows\\\"', 'sec-fetch-dest': 'empty', 'sec-fetch-mode': 'cors', 'sec-fetch-site': 'same-origin'}, 'referrer': '"+url+"', 'referrerPolicy': 'strict-origin-when-cross-origin', 'body': null, 'method': 'GET', 'mode': 'cors', 'credentials': 'include' }).then(function(bite){ return bite.json(); }).then(function(data){ const newDiv = document.createElement('div'); \r\nconst newContent = document.createTextNode(JSON.stringify(data)); newDiv.id = 'datajson'; newDiv.appendChild(newContent); const currentDiv = document.getElementById('div1'); document.body.insertBefore(newDiv, currentDiv); }); ");
            System.Threading.Thread.Sleep(1000);
            LTools.Workflow.Elements.WFAddToLog.AddToLog(sd.WorkflowVar, $"Ожидание: 1000", LTools.Enums.LogMessageType.Debug);
            json = browser.Eval("document.getElementById('datajson').childNodes[0].data");
            LTools.Workflow.Elements.WFAddToLog.AddToLog(sd.WorkflowVar, $"json: {json}", LTools.Enums.LogMessageType.Info);
            if (string.IsNullOrEmpty(json) || json == "null"|| json == "{}") return new ExecutionResult() { IsSuccess = false, SuccessMessage = $"josn is null" };

            var res = new BankrotFedresurResult() { Json = json, IsFind = true };
            SetVariableValue<BankrotFedresurResult>(Result, res, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"PbNalogResult: {!string.IsNullOrEmpty(res.Json)}" };

        }


        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            return ret;
        }
    }
}