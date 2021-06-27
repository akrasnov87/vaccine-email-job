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
            return new string[] { "ФИО", "Дата рождения", "Вакцинирован\n(1-Да, 0-Нет)", "Кол-во вакцинаций", "Дата внесения\nинформации\nо вакцинации", "ПЦР\n(1-Да, 0-Нет)", "Кол-во ПЦР", "Дата внесения\nинформации\nо ПЦР", "Противопоказание\n(1-Да, 0-Нет)" };
        }

        protected override void FormatCells(ExcelRange excelRange, int rangeIdx, int length)
        {
            base.FormatCells(excelRange, rangeIdx, length);

            excelRange["B" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            excelRange["C" + rangeIdx + ":D" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            excelRange["E" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            excelRange["F" + rangeIdx + ":G" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            excelRange["H" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            excelRange["I" + rangeIdx].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }
    }
}
