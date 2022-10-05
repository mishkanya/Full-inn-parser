using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Back
{
    public class PrimoParseEgrulNalogPDFBack : PrimoComponentSimple<PrimoParseEgrulNalogPDF>
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
        private const string PDF_PATH = "Path";
        private const string OUT_LIST_RESULT = "Result";


        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(string))]
        [System.ComponentModel.Category(ContainersData.INPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(PDF_PATH)] //ok
        public string Path
        {
            get { return this._path; }
            set { this._path = value; this.InvokePropertyChanged(this, PDF_PATH); }
        }
        private string _path;

        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(EgrulNalogResult))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(OUT_LIST_RESULT)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, OUT_LIST_RESULT); }
        }
        private string _result;

        #endregion

        public PrimoParseEgrulNalogPDFBack(IWFContainer container) : base(container)
        {
            this._container = container;
            sdkComponentName = "Получить данные Egrul.Nalog.ru";
            sdkComponentHelp = "Получить данные Egrul.Nalog.ru";
            sdkComponentIcon = ContainersData.ICON_PATH;

            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = PDF_PATH,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT, // ok
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
            var path = GetPropertyValue<string>(this.Path, PDF_PATH, sd);
            var parser = new Models.SiteData.EgrulNalog.Parser(path);

            var result = parser.Result;
            SetVariableValue<Models.SiteData.EgrulNalogResult>(Result, result, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"EgrulNalogResult" };
        }


        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            if (String.IsNullOrEmpty(this.Path)) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = PDF_PATH, Error = "Путь не указан" });
            return ret;
        }

    }
}

