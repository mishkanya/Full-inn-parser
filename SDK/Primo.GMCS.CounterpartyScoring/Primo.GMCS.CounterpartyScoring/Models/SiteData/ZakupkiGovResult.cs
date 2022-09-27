using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    /// <summary>
    /// https://zakupki.gov.ru/epz/eruz/search/results.html
    /// https://zakupki.gov.ru/epz/eruz/search/results.html?morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&pageNumber=1&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&sortBy=BY_REGISTRY_DATE&participantType_0=on&participantType_1=on&participantType_2=on&participantType_3=on&participantType_4=on&participantType_5=on&participantType_6=on&participantType_7=on&participantType=0%2C1%2C2%2C3%2C4%2C5%2C6%2C7&registered=on&excluded=on&rejectReasonIdNameHidden={}&inn=%inn%&countryRegIdNameHidden={}&violations=on
    /// </summary>
    public class ZakupkiGovResult : BaseResult
    {
        public ZakupkiGovResult() { }
        public ZakupkiGovResult(bool hasData)
        {
            if(hasData == false)
            {
                this.IsFind = false;
                this.Message = "Отрицательный результат поиска по реестру юридических лиц, привлеченных к административной ответственности по статье 19.28 Кодекса Российской Федерации об административных правонарушениях";
            }
            else
            {
                this.IsFind = true;
                this.Message = "Сведения о привлечении участника закупок к административной ответственности за совершение правонарушения, предусмотренного ст. 19.28 КоАП РФ";
            }

        }
        public string Court { get; set; }
        public string  Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExecuteDate { get; set; }

    }
}
