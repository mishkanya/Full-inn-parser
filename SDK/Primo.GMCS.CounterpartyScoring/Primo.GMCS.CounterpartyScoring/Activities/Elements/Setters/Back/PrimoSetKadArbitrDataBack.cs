using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
using Primo.GMCS.CounterpartyScoring.Activities.Containers.Back;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.Back
{
    public class PrimoSetKadArbitrDataBack : PrimoDataSetterBaseBack<KadArbitrNotBankrotResult, PrimoSetKadArbitrData>
    {
        protected override string _sdkComponentName=> "Вставить данные с сайта https://kad.arbitr.ru/";
        public PrimoSetKadArbitrDataBack(IWFContainer container) : base(container)
        {

        }
        public override ExecutionResult SimpleAction(ScriptingData sd)
        {   
            if(TryGetMainPropertie(sd, out KadArbitrNotBankrotResult data) == false) 
                return new ExecutionResult() { IsSuccess = false, ErrorMessage = "Объект не должен равнятья null!" };

            _resultContainer.SetData<Models.SiteData.KadArbitrNotBankrotResult>(data, sd);

            var bankrot = new Models.SiteData.KadArbitrBankrotResult() { IsFind = true, HasError = false, HasData = true };
            bankrot.AddTableResult(data.TableResults.Where(t=> t.Type == Models.Enums.KadArbitrTableElementType.Bankrot));
            _resultContainer.SetData<Models.SiteData.KadArbitrBankrotResult>(bankrot, sd);

            return new ExecutionResult() { IsSuccess = true, SuccessMessage = $"result" };
        }
        public override ValidationResult Validate()
        {
            ValidationResult ret = new ValidationResult();
            return ret;
        }

        protected override PrimoDataSetterBaseBack<KadArbitrNotBankrotResult, PrimoSetKadArbitrData> CreateNewObject(IWFContainer container)
        {
            return new PrimoSetKadArbitrDataBack(container);
        }
    }
}