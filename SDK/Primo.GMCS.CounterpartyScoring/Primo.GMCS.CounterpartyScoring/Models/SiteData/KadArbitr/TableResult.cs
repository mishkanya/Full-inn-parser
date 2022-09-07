using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData.KadArbitr
{
    [Serializable]
    public class TableResult
    {
        /// <summary>
        /// Дата обращения
        /// </summary>
        public DateTime Date { get; set; }  
        /// <summary>
        /// Номер дела
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Судья
        /// </summary>
        public string Judge { get; set; }
        /// <summary>
        /// Текущая инстанция 
        /// </summary>
        public string Instance { get; set; }
        /// <summary>
        /// Истец
        /// </summary>
        public string Plaintiff { get; set; }
        /// <summary>
        /// Ответчик
        /// </summary>
        public string Defendant { get; set; } 
    }
}
