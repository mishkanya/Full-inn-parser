using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData.EgrulNalog
{
    public class Person
    {
        public Person(string name, string surName, string patronymic, string inn = "", string position = "")
        {
            Name = name.Trim();
            SurName = surName.Trim();
            Patronymic = patronymic.Trim();
            Inn = inn.Trim();
            Position = position.Trim();
        }
        public Person()
        {

        }
        public Person(string FIO)
        {
            string[] fio = FIO.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            this.Name = fio[0];
            this.SurName = fio[1];
            this.Patronymic = fio.Count() == 3 ? fio[2] : String.Empty;
        }

        /// <summary>
        /// ABJ
        /// </summary>
        /// <param name="FIO"></param>
        /// <param name="b"></param>
        public Person(string FIO, bool b)//когда фамилия и имя наоборот
        {

            string[] fio = FIO.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            this.Name = fio[1];
            this.SurName = fio[0];
            this.Patronymic = fio.Count() == 3 ? fio[2] : String.Empty;
        }


        public static List<Person> PersonsList(List<string> persons)
        {
            List<Person> _persons = new List<Person>(0);
            foreach (string p in persons)
            {
                _persons.Add(new Person(p));
            }
            return _persons;
        }

        public string FIO => $"{SurName} {Name} {Patronymic}";
        public string Name { get; set; }
        public int CompanyCount { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public string Inn { get; set; }
        public string Position { get; set; }
        public bool isEntity { get; set; }


        public double? Procent { get; set; }

        public Int64? ProcentCoast { get; set; }
    }
}
