using Primo.GMCS.CounterpartyScoring.Models.SiteData.KadArbitr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    /// <summary>
    /// https://kad.arbitr.ru/
    /// </summary>
    public class KadArbitrResult : BaseResult
    {
        public KadArbitrResult() : base()
        {
            _tableResults = new List<TableResult>();
        }

        private List<TableResult> _tableResults { get; set; }

        /// <summary>
        /// Данные из таблицы
        /// </summary>
        public IEnumerable<TableResult> TableResults { get => _tableResults; }

        public int TableRowsCount { get => _tableResults.Count; }
        public void SetTableResult(IEnumerable<TableResult> tableResults)
        {
            _tableResults = tableResults.ToList();
        }
        public void AddTableResult(TableResult tableResult)
        {
            _tableResults.Add(tableResult);
        }
    }
}
