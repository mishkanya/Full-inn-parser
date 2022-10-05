using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData.EgrulNalog
{
    public class Organization
    {
        public string Name { get; set; }
        public double? Procent { get; set; }

        public Int64? ProcentCoast { get; set; }
        public string INN { get; set; }
    }
}
