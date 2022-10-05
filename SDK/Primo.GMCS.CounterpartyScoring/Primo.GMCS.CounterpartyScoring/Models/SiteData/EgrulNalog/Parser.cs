
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using word = Microsoft.Office.Interop.Word;

namespace Primo.GMCS.CounterpartyScoring.Models.SiteData.EgrulNalog
{
    public class Parser
    {
        private EgrulNalogResult _result;
        public EgrulNalogResult Result => _result;

        string[] ChaptersNames =
        {
            "Наименование",
            "Место нахождения и адрес юридического лица",
            "Сведения о регистрации",
            "Сведения о регистрирующем органе по месту нахождения юридического лица",
            "Сведения о лице, имеющем право без доверенности действовать от имени юридического лица",
            "Сведения об участниках / учредителях юридического лица",
            "Сведения об уставном капитале / складочном капитале / уставном фонде / паевом фонде",
            "Сведения о держателе реестра акционеров акционерного общества",
            "Сведения об учете в налоговом органе",
            "Сведения о регистрации в качестве страхователя в территориальном органе Пенсионного фонда Российской Федерации",
            "Сведения о регистрации в качестве страхователя в исполнительном органе Фонда социального страхования Российской Федерации",
            "Сведения о видах экономической деятельности по Общероссийскому классификатору видов экономической деятельности",
            "Сведения о филиалах и представительствах",
            "Сведения о записях, внесенных в Единый государственный реестр юридических лиц",
            "Представительства",
            "Сведения о правопредшественнике"
        };

        string[] _noAddressStrings = { "ОГРН:", "ИНН:", "КПП:", "ГЕНЕРАЛЬНЫЙ ДИРЕКТОР" };
        public DataTable dataTable = new DataTable();


        public Parser(string path)
        {
            _result = new EgrulNalogResult();
            ParseDocument(path);
            ParceName();
            ParceRegistration();
            ParceFounders();
            ParceVicariousAuthorityPersonOrOrganization();
            ParceKapital();
            ParceClosing();

            ParceAdress();
            ParseResidents();

            _result.HasError = false;
            _result.IsFind = true;

        }



        public void ParseDocument(string filePath, bool saveFile = false, string savePath = "")
        {
            var app = new word.Application();
            object MissingObj = System.Reflection.Missing.Value;

            var document = app.Documents.Open(filePath, false, true, ref MissingObj,
            ref MissingObj, ref MissingObj, ref MissingObj, ref MissingObj, ref MissingObj, word.WdOpenFormat.wdOpenFormatAuto, ref MissingObj, ref MissingObj, ref MissingObj,
            ref MissingObj, ref MissingObj, ref MissingObj);

            var maxColumnCount = 0;
            var tables = document.Tables;
            foreach (word.Table table in tables)
            {
                var columnsCount = table.Columns.Count;
                if (maxColumnCount < columnsCount)
                    maxColumnCount = columnsCount;
            }
            for (int i = 0; i < maxColumnCount; i++)
            {
                dataTable.Columns.Add(new DataColumn($"Column{i}"));
            }
            foreach (word.Table table in tables)
            {
                var tablerows = table.Rows;
                try
                {
                    foreach (word.Row row in tablerows)
                    {
                        var dtRow = dataTable.NewRow();

                        int j = 0;
                        var cells = row.Cells;
                        foreach (word.Cell cell in cells)
                        {
                            var value = cell.Range.Text;


                            value = value.ToString().Replace("\r", " ");
                            value = value.ToString().Replace("\u0007", "");
                            dtRow[j] = value.Trim();
                            j++;
                            releaseObject(cell);
                        }
                        releaseObject(cells);
                        if (!string.IsNullOrEmpty(dtRow[0]?.ToString()) | !string.IsNullOrEmpty(dtRow[1]?.ToString()) | !string.IsNullOrEmpty(dtRow[2]?.ToString()))
                            dataTable.Rows.Add(dtRow);
                        releaseObject(tablerows);
                    }
                    releaseObject(tablerows);
                }
                catch
                {
                    releaseObject(tablerows);
                }



            }
            releaseObject(tables);
            if (saveFile)
            {
                document.SaveAs2(savePath);
            }
            document.Close(false);
            releaseObject(document);
            app.Quit();
            releaseObject(app);
            GC.Collect();


        }







        //Парсинг даты регистрации и ОГРН
        private void ParceRegistration()
        {
            var rows = GetChapterRows("Сведения о регистрации");
            _result.Ogrn = rows.FirstOrDefault(row => row[1].ToString().Contains("ОГРН"))[2]?.ToString();
            try
            {
                var registration = rows.FirstOrDefault(row => row[1].ToString().Contains("Дата регистрации"))[2]?.ToString();
                if (!string.IsNullOrEmpty(registration))
                    if (DateTime.TryParseExact(registration, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime date))
                        _result.RegistrationDate = date;
            }
            catch
            {

            }

        }


        //Парсинг Участников
        private void ParceFounders()
        {
            var rows = GetChapterRows("Сведения об участниках / учредителях юридического лица");
            if (rows.Count == 0)
                return;

            var foundersRows = new List<List<DataRow>>();





            //несколько учередителей
            if (rows.Where(x => int.TryParse(String.Join(" ", x.ItemArray).Trim(), out int a)).Count() > 0)
                foundersRows = SplitRowsRegex(rows, @"\d");
            else
                foundersRows.Add(rows);

            foreach (var founderRows in foundersRows)
            {
                string inn = FindValue(founderRows, "ИНН");
                string coast = FindValue(founderRows, "Номинальная стоимость доли");
                string procent = FindValue(founderRows, "Размер доли (в процентах)");
                if (!string.IsNullOrEmpty(inn))
                {

                    switch (inn.Length)
                    {
                        case 10:
                            {
                                string name = FindValue(founderRows, "Полное наименование");
                                var org = new Organization();
                                org.INN = inn;
                                org.Name = name;
                                (org.Procent, org.ProcentCoast) = Foundersprocents(procent, coast);
                                _result.FoundersOrganizations.Add(org);
                                break;
                            }
                        case 12:
                            {
                                string fio = FindValue(founderRows, "Фамилия");
                                var person = new Person(fio, true);
                                (person.Procent, person.ProcentCoast) = Foundersprocents(procent, coast);
                                _result.Founders.Add(person);
                                person.Inn = inn;
                                break;
                            }
                    }
                }
                else
                {
                    var name = FindValue(founderRows, "Полное наименование");
                    var org = new Organization();
                    org.Name = name;
                    (org.Procent, org.ProcentCoast) = Foundersprocents(procent, coast);
                    _result.FoundersOrganizations.Add(org);
                }
            }
        }

        private (double?, Int64?) Foundersprocents(string procentStr, string procentCoastStr)
        {
            Int64? valint = null;
            double? valdouble = null;
            if (Int64.TryParse(procentCoastStr, out Int64 _valint))
                valint = _valint;
            if (double.TryParse(procentStr, out double _valdouble))
                valdouble = _valdouble;
            return (valdouble, valint);
        }


        private string FindValue(List<DataRow> rows, string columnText)
        {
            return rows.FirstOrDefault(row => row[1].ToString().Contains(columnText))?[2]?.ToString();
        }

        //Парсинг Наименования
        private void ParceName()
        {
            var rows = GetChapterRows("Наименование");
            _result.Name = rows.FirstOrDefault(row => row[1].ToString().Contains("Сокращенное наименование"))[2]?.ToString();
            _result.FullName = rows.FirstOrDefault(row => row[1].ToString().Contains("Полное наименование"))[2]?.ToString();
            DataRow dataRow = rows.FirstOrDefault(row => row[1].ToString().Contains("Полное наименование на английском языке"));
            if (dataRow != null)
            {
                _result.EnglishFullName = dataRow[2]?.ToString();
            }
        }


        private List<DataRow> GetChapterRows(string ChapterName)
        {
            var findRows = new List<DataRow>();
            var findChapter = false;
            foreach (DataRow row in dataTable.Rows)
            {
                if (findChapter)
                {
                    if (ChaptersNames.Contains(row[0].ToString()) | ChaptersNames.Contains(row[1].ToString()))
                        break;
                    findRows.Add(row);
                }
                if (row[0].ToString() == ChapterName | row[1].ToString() == ChapterName)
                {
                    findChapter = true;
                    continue;
                }
            }
            return findRows;
        }

        private List<List<DataRow>> SplitRowsRegex(List<DataRow> rows, string regex)
        {
            var outData = new List<List<DataRow>>();
            List<DataRow> rowsArray = new List<DataRow>();
            foreach (DataRow row in rows)
            {
                var splitrow = true;
                if (row.ItemArray.Where(x => Regex.IsMatch(x?.ToString(), regex)).Count() == 1 && row.ItemArray.Where(x => string.IsNullOrEmpty(x?.ToString())).Count() == 2)
                {
                    splitrow = true;
                }
                else
                {
                    splitrow = false;
                }
                if (splitrow)
                {
                    if (rowsArray.Count > 0)
                        outData.Add(rowsArray);
                    rowsArray = new List<DataRow>();
                    continue;
                }
                rowsArray.Add(row);
            }
            outData.Add(rowsArray);
            return outData;
        }

        //Парсинг Директора
        private void ParceVicariousAuthorityPersonOrOrganization()
        {
            var rows = GetChapterRows("Сведения о лице, имеющем право без доверенности действовать от имени юридического лица");
            if (rows.Count == 0)
                return;
            string inn = FindValue(rows, "ИНН");
            switch (inn?.Length)
            {
                case 10:
                    {
                        string name = FindValue(rows, "Полное наименование");
                        var org = new Organization();
                        org.INN = inn;
                        org.Name = name;
                        _result.VicariousAuthorityOrganization = org;
                        break;
                    }
                case 12:
                    {
                        string fio = FindValue(rows, "Фамилия");
                        string position = FindValue(rows, "Должность");
                        var person = new Person(fio, true);
                        person.Inn = inn;
                        person.Position = position;
                        _result.VicariousAuthorityPerson = person;
                        break;

                    }
            }

        }

        //Парсинг уставного капитала
        private void ParceKapital()
        {
            var rows = GetChapterRows("Сведения об уставном капитале / складочном капитале / уставном фонде / паевом фонде");
            var coast = rows.FirstOrDefault(row => row[1].ToString().Contains("Размер"))?[2]?.ToString();
            if (coast != null)
                coast = coast.Replace('.', ',');
            if (double.TryParse(coast, out double c))
            {
                _result.KapitalStr = coast;
                _result.Kapital = c;
            }
        }

        private void ParseResidents()
        {
            var rows = GetChapterRows("Сведения о филиалах и представительствах");
            if (rows.Count() == 0)
                return;

            var rowsArray = SplitRowsRegex(rows, @"\d+");


            foreach (var resident in rowsArray)
            {

                var grnAndDate = FindValue(resident, "ГРН и дата внесения в ЕГРЮЛ сведений о данном филиале")?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (grnAndDate?.Length == 2)
                {
                    var fillial = new Fillial();
                    fillial.EgrulDate = DateTime.ParseExact(grnAndDate[1], "dd.MM.yyyy", null);
                    fillial.Grn = grnAndDate[0];
                    fillial.FullName = FindValue(resident, "Наименовании филиала");
                    fillial.Address = FindValue(resident, "Адрес места нахождения филиала на территории Российской Федерации");
                    Result.Fillials.Add(fillial);
                }
            }
        }


        private void ParceClosing()
        {
            var rows = GetChapterRows("Сведения о прекращении юридического лица");
            if (rows.Count == 0)
                return;
            string closing = FindValue(rows, "Способ прекращения");
            string dates = FindValue(rows, "Дата прекращения");
            if (DateTime.TryParseExact(dates, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime date))
            {
                _result.TerminationDate = date;
            }
            _result.TerminationMethod = closing;
        }

        private void ParceAdress()
        {

            var rows = GetChapterRows("Место нахождения и адрес юридического лица");
            if (rows.Count == 0)
                return;

            string address = FindValue(rows, "Адрес юридического лица");
            string dop = FindValue(rows, "Дополнительные сведения");

            _result.Address = address;
            _result.AddressOther = dop;
        }

        private bool isExist(string str)
        {
            foreach (string a in _noAddressStrings)
            {
                if (str.Contains(a))
                {
                    return true;
                }
            }
            return false;
        }
        private void releaseObject(params object[] objects)
        {
            foreach (object obj in objects)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);

                }
                finally
                {
                    //GC.Collect();
                }
            }
        }
    }
}
