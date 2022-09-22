using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using Primo.GMCS.CounterpartyScoring.Activities.Containers.Back;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ValidationResult = LTools.Common.Model.ValidationResult;

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.Back
{
    public abstract class PrimoDataSetterBaseBack<T, U> : PrimoComponentSimple<U> where U : UserControl, new() where T : BaseResult
    {
        public override string GroupName
        {
            get => ContainersData.SettersSiteDataGroupName;
            protected set { }
        }

        protected PrimoInnResultContainerBack _resultContainer;

        #region properties



        private const string SITE_DATA = "Result";
        private string _result;
        [LTools.Common.Model.Serialization.StoringProperty]
        [LTools.Common.Model.Studio.ValidateReturnScript(DataType = typeof(BaseResult))]
        [System.ComponentModel.Category(ContainersData.OUTPUT_CATEGORY_NAME), System.ComponentModel.DisplayName(SITE_DATA)] //ok
        public string Result
        {
            get { return this._result; }
            set { this._result = value; this.InvokePropertyChanged(this, SITE_DATA); }
        }

        #endregion

        protected virtual string _sdkComponentName { get => "Base Name"; }
        protected virtual string _sdkComponentHelp { get => "Должен распологаться в контейнере PrimoInnResultContainer"; }

       
        public PrimoDataSetterBaseBack(IWFContainer container) : base(container)
        {
            sdkComponentName = _sdkComponentName;
            sdkComponentHelp = _sdkComponentHelp;
            sdkComponentIcon = ContainersData.ICON_PATH;

            sdkProperties = new List<LTools.Common.Helpers.WFHelper.PropertiesItem>()
            {
                new LTools.Common.Helpers.WFHelper.PropertiesItem()
                {
                    PropName = SITE_DATA,
                    PropertyType = LTools.Common.Helpers.WFHelper.PropertiesItem.PropertyTypes.SCRIPT, // ok
                    EditorType = ScriptEditorTypes.NONE,
                    DataType = typeof(string),
                    ToolTip = $"{typeof(T)}Данные с сайта", IsReadOnly = false
                },
            };
            _resultContainer = GetContainerOfType<PrimoInnResultContainerBack>(container);
            InitClass(container);
        }

        protected bool TryGetMainPropertie(ScriptingData sd, out T propertie)
        {
            propertie = GetPropertyValue<T>(this.Result, SITE_DATA, sd);
            return propertie != null;
        }

        public override ExecutionResult SimpleAction(ScriptingData sd) => new ExecutionResult() { IsSuccess = false, SuccessMessage = "Метод не перегружен" };

        protected abstract PrimoDataSetterBaseBack<T, U> CreateNewObject(IWFContainer container);

        public override IWFComponent ProduceControl(IWFContainer container)
        {
            if (GetContainerOfType<PrimoInnResultContainerBack>(container) == null)
            {
                PrimoInnResultContainerBack db = new PrimoInnResultContainerBack(container);
                db.AddComponent(CreateNewObject(db), null);
                return db;
            }
            return CreateNewObject(container);
        }

        public override bool IsAllowedMoveToContainer(IWFContainer container)
        {
            if (GetContainerOfType<PrimoInnResultContainerBack>(container) != null)
                return true;
            return false;
        }
    }
}