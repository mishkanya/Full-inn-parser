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
    public class KadArbitrNotBankrotResult : BaseResult
    {

        public KadArbitrNotBankrotResult(IEnumerable<TableResult> tableResults) : base()
        {
            this.AddTableResult(tableResults);
        }
        public bool HasData { get; set; }
        public virtual bool IsBankrots { get => false; }
        public KadArbitrNotBankrotResult() : base()
        {
        }
        protected void InitList()
        {
            if (_tableResults == null) _tableResults = new List<TableResult>();
        }
        protected List<TableResult> _tableResults { get; set; } = new List<TableResult>();

        /// <summary>
        /// Данные из таблицы
        /// </summary>
        public IEnumerable<TableResult> TableResults { get => _tableResults; }

        public int TableRowsCount
        {
            get
            {
                InitList();
                return _tableResults.Count;
            }
        }
        public void AddTableResult(TableResult tableResult)
        {
            InitList();
            _tableResults.Add(tableResult);
            this.HasData = true;
        }
        public void AddTableResult(IEnumerable<TableResult> tableResult)
        {
            InitList();
            _tableResults.AddRange(tableResult);
            this.HasData = tableResult.Count() != 0;
        }
    }
}
