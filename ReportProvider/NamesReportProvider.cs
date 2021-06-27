using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using OfficeOpenXml;
using Vaccine.Models;

namespace Vaccine.ReportProvider
{
    /// <summary>
    /// Отчет уровня «Ответственного»
    /// </summary>
    public class NamesReportProvider : BaseReportProvider
    {
        public NamesReportProvider(string[][] items)
            : base(items)
        {
            
        }

        public override string[] GetHeaders()
        {
            return new string[] { "ФИО", "Дата рождения", "Вакцинирован (1-Да, 0-Нет)", "Кол-во вакцинаций", "Дата внесения информации о вакцинации", "ПЦР (1-Да, 0-Нет)", "Кол-во ПЦР", "Дата внесения информации о ПЦР", "Противопоказание (1-Да, 0-Нет)" };
        }

        protected override void FormatCells(ExcelRange excelRange, int rangeIdx, int length, string[] columns)
        {
            base.FormatCells(excelRange, rangeIdx, length, columns);

            string dateStr = columns[columns.Length - 2];
            if (!string.IsNullOrEmpty(dateStr))
            {
                try
                {
                    DateTime dt = DateTime.Parse(dateStr, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
                    string headerRange = "A"+ rangeIdx + ":" + GetColumnName(columns.Length) + rangeIdx;

                    int day = (DateTime.Now.Date - dt).Days;

                    if (day >= 3 && day < 7)
                    {
                        excelRange[headerRange].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
                    } else if(day >= 7)
                    {
                        excelRange[headerRange].Style.Fill.SetBackground(System.Drawing.Color.Red);
                    }
                } catch(Exception e)
                {
                    Console.WriteLine("[ERR]: " + e.ToString());
                }
            }
 
            excelRange["B" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            excelRange["C" + rangeIdx + ":D" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            excelRange["E" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            excelRange["F" + rangeIdx + ":G" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            excelRange["H" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            excelRange["I" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }
    }
}
