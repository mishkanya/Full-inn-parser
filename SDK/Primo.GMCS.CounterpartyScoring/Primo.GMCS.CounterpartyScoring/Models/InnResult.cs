using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models
{
    [Serializable]
    public class InnResult
    {
        public InnResult(string inn)
        {
            Inn = inn;
        }
        public string Inn { get; private set; }

        #region parse results

        public SiteData.BankrotFedresurResult BankrotFedresurResult { get; private set; }
        public SiteData.EgrulNalogResult EgrulNalogResult { get; private set; }
        public SiteData.EppGenprocGovResult EppGenprocGovResult { get; private set; }
        public SiteData.KadArbitrResult KadArbitrResultBankrot { get; private set; }
        public SiteData.KadArbitrResult KadArbitrResultNotBankrot { get; private set; }
        public SiteData.NalogGovResult NalogGovResult { get; private set; }
        public SiteData.PbNalogResult PbNalogResult { get; private set; }
        public SiteData.ZakupkiGovResult ZakupkiGovResult { get; private set; }
        
        #endregion



        public string GetJson()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return jsonString;
        }
    }
}
