using Primo.GMCS.CounterpartyScoring.Models.SiteData.EgrulNalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData
{
    /// <summary>
    /// https://egrul.nalog.ru/index.html
    /// </summary>
    public class EgrulNalogResult : BaseResult
    {
        public EgrulNalogResult()
        {
            Founders = new List<Person>();
            FoundersOrganizations = new List<Organization>();
            Fillials = new List<Fillial>();
        }
        public string Ogrn { get; set; } //ОГРН + ОГРН\s[0-9]*
        public string Name { get; set; } //Сокращенное наименование +  Сокращенное\sнаименование\sна\sрусском\sязыке\s.*
        public string FullName { get; set; } //Полное наименование + (Полное\sнаименование\sна\sрусском\sязыке)+(.*?)2
        public string EnglishFullName { get; set; } //Полное наименование на английском +   (7\sПолное\sнаименование\sна\sанглийском\sязыке)+(.*?)8

        public DateTime RegistrationDate { get; set; } //Дата регистрации +  /(17\sДата\sрегистрации)+(.*?)18/g
        public string TerminationMethod { get; set; } //Способ прекращения ?
        public DateTime TerminationDate { get; set; }
        public string AddressOther { get; set; } //Адрес доп сведения
        public double Kapital { get; set; } //капитал
        public string KapitalStr { get; set; } //капитал
        public Person VicariousAuthorityPerson { get; set; } //лицо без доверенности+
        public Organization VicariousAuthorityOrganization { get; set; } //организация без доверенности+
        public List<Person> Founders { get; set; } //Учредители +
        public Person GenDir { get; set; }// генеральный директор +
        public string Address { get; set; }// адрес +

        public List<Organization> FoundersOrganizations { get; set; }
        public List<string> FoundersFioList => Founders.Select(a => a.FIO).ToList();

        public List<Fillial> Fillials { get; set; }
    }
}

