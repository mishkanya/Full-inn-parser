using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Activities
{
    internal static class ContainersData
    {
        public static readonly string MainGroupName = "GMCS";
        public static readonly string SitesGroupName = $"{MainGroupName}{LTools.Common.UIElements.WFPublishedElementBase.TREE_SEPARATOR}Sites";
        public const string InputCategoryName = "Входные данные:";
        public const string OutputCategoryName = "Вывод:";
        public const string Icon = "pack://application:,,/Primo.GMCS.CounterpartyScoring;component/Images/sample.png";

        public const string ssad = "";

    }
}
