using Primo.GMCS.CounterpartyScoring.Models.SiteData.KadArbitr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    public class KadArbitrBankrotResult : KadArbitrNotBankrotResult
    {
        public KadArbitrBankrotResult(IEnumerable<TableResult> tableResults) : base()
        {
            this.AddTableResult(tableResults);
        }
        public override bool IsBankrots { get => true;}
        public KadArbitrBankrotResult() : base() { }
    }
}
