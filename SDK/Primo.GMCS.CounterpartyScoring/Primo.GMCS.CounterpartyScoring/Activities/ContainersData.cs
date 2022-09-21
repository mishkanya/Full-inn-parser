using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTools.Common.UIElements;

namespace Primo.GMCS.CounterpartyScoring.Activities
{
    internal static class ContainersData
    {
        public static readonly string MainGroupName = $"GMCS{WFPublishedElementBase.TREE_SEPARATOR}InnParser";

        public static readonly string SitesGroupName = $"{MainGroupName}{WFPublishedElementBase.TREE_SEPARATOR}Sites";
        public static readonly string SettersSiteDataGroupName = $"{MainGroupName}{WFPublishedElementBase.TREE_SEPARATOR}Sites{WFPublishedElementBase.TREE_SEPARATOR}Setters";
        public const string INPUT_CATEGORY_NAME = "Входные данные:";
        public const string OUTPUT_CATEGORY_NAME = "Вывод:";
        public const string ICON_PATH = "pack://application:,,/Primo.GMCS.CounterpartyScoring;component/Images/sample.png";


    }
}
