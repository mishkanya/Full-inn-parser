using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using Primo.GMCS.CounterpartyScoring.Models;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Activities.Containers.Back
{
    public class PrimoInnResultContainerBack : PrimoContainer
    {
        public override string GroupName
        {
            get => ContainersData.MainGroupName;
            protected set { }
        }


        #region properties
        /// <summary>
        /// Название перменной браузера
        /// </summary>
        private const string INN_STRING = "Inn";
        private const string OUT_INN_Result = "InnResult";


        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(string))]
        [System.ComponentModel.Category(ContainersData.INPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(INN_STRING)] //ok
        public string InnString
        {
            get { return this._innString; }
            set { this._innString = value; this.InvokePropertyChanged(this, INN_STRING); }
        }
        private string _innString;


        /// <summary>
        /// Отображение переменной в студии
        /// </summary>
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(InnResult))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(OUT_INN_Result)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, OUT_INN_Result); }
        }
        private string _result;
        #endregion


        public PrimoInnResultContainerBack(IWFContainer container) : base(container)
        {
            sdkComponentName = "Спаршенные данные InnResult";
            sdkComponentHelp = "Спаршенные данные для последукющего перевода в Json";
            sdkComponentIcon = ContainersData.ICON_PATH;

            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {  
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = InnString,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT, // ok
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = "Входная строка ИНН", IsReadOnly = false
                },
                  new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = Result,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.VARIABLE, // ok
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = "Полученная переменная InnResult", IsReadOnly = false
                },
            };
            InitClass(container);
        }

        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            string inn = GetPropertyValue<string>(this.InnString, INN_STRING, sd);
            var innResult = new Models.InnResult(inn);

            SetVariableValue<Models.InnResult>(this.Result, innResult, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = "Done" };
        }

        public void SetData<T>(T siteData,ScriptingData sd) where T: BaseResult
        {
            var res = GetPropertyValue<InnResult>(Result, OUT_INN_Result, sd);
            res.SetSiteData<T>(siteData);
        }


        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            if (String.IsNullOrEmpty(this.InnString)) ret.Items.Add(new ValidationResult.ValidationItem() { PropertyName = InnString, Error = "Воходной параментр не передан" });

            return ret;
        }
    }
}
