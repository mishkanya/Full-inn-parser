using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    [Serializable]
    public abstract class BaseResult
    {
        public bool IsFind { get; set; }
        public bool HasError { get; set; }
        public string Message{ get; set; }

        public DateTime CheckTime { get; private set; }
        public BaseResult()
        {
            CheckTime = DateTime.Now;
        }
    }
}
