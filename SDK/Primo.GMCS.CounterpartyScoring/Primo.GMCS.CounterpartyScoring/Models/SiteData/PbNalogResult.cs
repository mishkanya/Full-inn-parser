using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTools.Common.Model;
using LTools.Common.UIElements;
using LTools.SDK;
using LTools.WebBrowser;
using Primo.GMCS.CounterpartyScoring.Activities.Containers.Back;
using Primo.GMCS.CounterpartyScoring.Activities.Elements.Setters.View;
using Primo.GMCS.CounterpartyScoring.Models.SiteData;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    /// <summary>
    /// https://pb.nalog.ru/
    /// </summary>
    public class PbNalogResult : BaseResult
    {
        /// <summary>
        /// json data
        /// </summary>
        public string Json { get; set; }
    }
}
