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
    public class PrimoSetEgrulNalogDataBack : PrimoDataSetterBaseBack<EgrulNalogResult, PrimoSetEgrulNalogData>
    {
        protected override string _sdkComponentName => "Вставить данные с сайта egrul.nalog.ru";
        public PrimoSetEgrulNalogDataBack(IWFContainer container) : base(container)
        {

        }

        public override ExecutionResult SimpleAction(ScriptingData sd)
        {
            if (TryGetMainPropertie(sd, out EgrulNalogResult data) == false)
                return new ExecutionResult() { IsSuccess = false, ErrorMessage = "Объект не должен равнятья null!" };

            _resultContainer.SetData<Models.SiteData.EgrulNalogResult>(data, sd);
            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"result" };
        }
        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            return ret;
        }

        protected override PrimoDataSetterBaseBack<EgrulNalogResult, PrimoSetEgrulNalogData> CreateNewObject(IWFContainer container)
        {
            return new PrimoSetEgrulNalogDataBack(container);
        }
    }
}
