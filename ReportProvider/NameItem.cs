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
        public long vaccineCount { get; set; }
        public DateTime? vaccineDate { get; set; }
        public int pcr { get; set; }
        public long pcrCount { get; set; }
        public DateTime? pcrDate { get; set; }
        public bool b_ignore { get; set; }
    }

    public static class NameItemExtension
    {
        public static string[][] ItemsToString(this NameItem[] items)
        {
            string[][] array = new string[items.Length][];
            for(int i = 0; i < items.Length; i++)
            {
                array[i] = new string[] { items[i].name, GetDateString(
                    items[i].birthDay),
                    items[i].b_ignore ? "" : items[i].vaccine.ToString(),
                    items[i].b_ignore ? "" : items[i].vaccineCount.ToString(),
                    items[i].b_ignore ? "" : GetDateString(items[i].vaccineDate),
                    items[i].b_ignore ? "" : items[i].pcr.ToString(),
                    items[i].b_ignore ? "" : items[i].pcrCount.ToString(),
                    items[i].b_ignore ? "" : GetDateString(items[i].pcrDate),
                    items[i].b_ignore ? "1" : "0" };
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
