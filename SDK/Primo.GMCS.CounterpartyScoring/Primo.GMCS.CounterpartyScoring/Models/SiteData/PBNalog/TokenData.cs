using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData.PBNalog
{
    public class TokenData
    {
        public TokenData()
        {

        }
        public string token { get; set; }
        public string id { get; set; }
        public bool captchaRequired { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
