using System;
using System.Collections.Generic;
using System.Text;

namespace Vaccine.ReportProvider
{
    public class NameItem
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime? birthDay { get; set; }
        public int vaccine { get; set; }
        public DateTime? vaccineDate { get; set; }
        public int pcr { get; set; }
        public DateTime? pcrDate { get; set; }
    }

    public static class NameItemExtension
    {
        public static string[][] ItemsToString(this NameItem[] items)
        {
            string[][] array = new string[items.Length][];
            for(int i = 0; i < items.Length; i++)
            {
                array[i] = new string[] { items[i].name, GetDateString(items[i].birthDay), items[i].vaccine.ToString(), GetDateString(items[i].vaccineDate), items[i].pcr.ToString(), GetDateString(items[i].pcrDate) };
            }

            return array;
        }

        public static string GetDateString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return "";
            } else
            {
                return date.Value.ToString("dd.MM.yyyy");
            }
        }
    }
}
