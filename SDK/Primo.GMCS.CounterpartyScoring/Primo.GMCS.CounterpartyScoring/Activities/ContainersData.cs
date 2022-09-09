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
        public const string INPUT_CATEGORY_NAME = "Входные данные:";
        public const string OUTPUT_CATEGORY_NAME = "Вывод:";
        public const string ICON_PATH = "pack://application:,,/Primo.GMCS.CounterpartyScoring;component/Images/sample.png";


    }
}
