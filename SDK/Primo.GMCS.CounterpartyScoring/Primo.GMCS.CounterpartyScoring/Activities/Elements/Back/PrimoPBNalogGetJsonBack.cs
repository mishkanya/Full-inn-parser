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

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Back
{
    public class PrimoPBNalogGetJsonBack : PrimoComponentSimple<PrimoDataFromKadArbitrTable>
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
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(PbNalogResult))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(OUT_LIST_RESULT)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, OUT_LIST_RESULT); }
        }
        private string _result;
        private const string OUT_LIST_RESULT = "Result";

        #endregion


        public PrimoPBNalogGetJsonBack(IWFContainer container) : base(container)
        {
            this._container = container;
            sdkComponentName = "Получить данные с pb.nalog.ru";
            sdkComponentHelp = "Получение json строки с сайта pb.nalog.ru";
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

            string siteToken = url.Replace("https://pb.nalog.ru/company.html?token=", "");
            LTools.Workflow.Elements.WFAddToLog.AddToLog(sd.WorkflowVar, $"SiteToken: {siteToken}", LTools.Enums.LogMessageType.Info);

            var tokenData = this.GetTokenData(siteToken);
            LTools.Workflow.Elements.WFAddToLog.AddToLog(sd.WorkflowVar, $"TokenData: {tokenData.ToString()}", LTools.Enums.LogMessageType.Info);

            if (tokenData == null) return new ExecutionResult() { IsSuccess = false, SuccessMessage = $"tokenData is null" };
            if(tokenData.captchaRequired == true) return new ExecutionResult() { IsSuccess = false, SuccessMessage = $"captchaRequired" };

            var json = GetSiteJson(siteToken, tokenData);
            if(string.IsNullOrEmpty(json) || json == "null") return new ExecutionResult() { IsSuccess = false, SuccessMessage = $"josn is null" };
            
            var res = new PbNalogResult() { Json = json, IsFind = true };
            SetVariableValue<PbNalogResult>(Result, res, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"PbNalogResult: {!string.IsNullOrEmpty(res.Json)}" };

        }


        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            return ret;
        }


        private TokenData GetTokenData(string siteToken)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            handler.AutomaticDecompression = ~DecompressionMethods.None;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://pb.nalog.ru/company-proc.json"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                    request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    request.Headers.TryAddWithoutValidation("Origin", "https://pb.nalog.ru");
                    request.Headers.TryAddWithoutValidation("Referer", "https://pb.nalog.ru/company.html?token={siteToken}");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");

                    request.Content = new StringContent($"token={siteToken}&method=get-request");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

                    var response = httpClient.SendAsync(request).Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<TokenData>(json);
                }
            }
        }


        private string GetSiteJson(string sitedata,TokenData tokenData )
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            handler.AutomaticDecompression = ~DecompressionMethods.None;
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://pb.nalog.ru/company-proc.json"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                    request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    request.Headers.TryAddWithoutValidation("Origin", "https://pb.nalog.ru");
                    request.Headers.TryAddWithoutValidation("Referer", $"https://pb.nalog.ru/company.html?token={sitedata}");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");

                    request.Content = new StringContent($"token={tokenData.token}&id={tokenData.id}&method=get-response");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

                    var response = httpClient.SendAsync(request).Result;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    var json = response.Content.ReadAsStringAsync().Result;
                    return json;
                }
            }
        }
    }
}
