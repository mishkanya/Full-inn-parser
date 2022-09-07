using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

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
        public SiteData.KadArbitrResult KadArbitrResult { get; private set; }
        public SiteData.NalogGovResult NalogGovResult { get; private set; }
        public SiteData.PbNalogResult PbNalogResult { get; private set; }
        public SiteData.ZakupkiGovResult ZakupkiGovResult { get; private set; }
        #endregion



        public string GetJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(this, options);
            return jsonString;
        }
    }
}
