using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.Back
{
    public   class PrimoSetZakupkiGovDataBack : PrimoDataSetterBaseBack<ZakupkiGovResult, PrimoSetZakupkiGovData>
    {
        protected override string _sdkComponentName => "Вставить данные с сайта zakupki.gov.ru";
        public PrimoSetZakupkiGovDataBack(IWFContainer container) : base(container)
        {

        }

        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            if (TryGetMainPropertie(sd, out ZakupkiGovResult data) == false)
                return new ExecutionResult() { IsSuccess = false, ErrorMessage = "Объект не должен равнятья null!" };

            _resultContainer.SetData<Models.SiteData.ZakupkiGovResult>(data, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"result" };
        }
        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            return ret;
        }

        protected override PrimoDataSetterBaseBack<ZakupkiGovResult, PrimoSetZakupkiGovData> CreateNewObject(IWFContainer container)
        {
            return new PrimoSetZakupkiGovDataBack(container);
        }
    }
}