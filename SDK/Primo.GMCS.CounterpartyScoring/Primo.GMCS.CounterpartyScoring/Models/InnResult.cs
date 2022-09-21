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
        public string Inn { get; private set; }

        public Dictionary<Type, SiteData.BaseResult> SitesData { get; private set; } = new Dictionary<Type, SiteData.BaseResult>()
        {
            {
                typeof(SiteData.BankrotFedresurResult), new SiteData.BankrotFedresurResult()
            },
            {
                typeof(SiteData.EgrulNalogResult), new SiteData.EgrulNalogResult()
            },
            {
                typeof(SiteData.EppGenprocGovResult), new SiteData.EppGenprocGovResult()
            },
            {
                typeof(SiteData.KadArbitrNotBankrotResult), new SiteData.KadArbitrNotBankrotResult()
            },
            {
                typeof(SiteData.KadArbitrBankrotResult), new SiteData.KadArbitrBankrotResult()
            },
            {
                typeof(SiteData.NalogGovResult), new SiteData.NalogGovResult()
            },
            {
                typeof(SiteData.PbNalogResult), new SiteData.PbNalogResult()
            },
            {
                typeof(SiteData.ZakupkiGovResult), new SiteData.ZakupkiGovResult()
            },
        };

        public InnResult(string inn) => Inn = inn;

        public T GetSiteData<T>() where T : SiteData.BaseResult => (T)SitesData[typeof(T)];
        public void SetSiteData<T>(T siteData) where T : SiteData.BaseResult => SitesData[typeof(T)] = siteData;
        public string GetJson() => JsonConvert.SerializeObject(this);
        
    }
}
