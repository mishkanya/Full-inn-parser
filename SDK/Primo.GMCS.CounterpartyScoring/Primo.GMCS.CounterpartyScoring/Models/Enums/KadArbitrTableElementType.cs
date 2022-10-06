using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.Enums
{
    /// <summary>
    /// 0 - банкротные, 1 - административные, 2 - гражданские
    /// </summary>
    public enum KadArbitrTableElementType
    {
        Bankrot,
        BankrotSimple,
        Administrative,
        AdministrativeSimple,
        Civil,
        CivilSimple,
        Default
    }
}
