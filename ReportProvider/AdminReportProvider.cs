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
    /// Сводный отчет
    /// </summary>
    public class AdminReportProvider : BaseReportProvider
    {
        public AdminReportProvider(string[][] items)
            : base(items)
        {
            
        }

        public override string[] GetHeaders()
        {
            return new string[] { "Наименование", "Группа", "Кол-во вакцинированных", "Кол-во сдающих ПЦР", "Кол-во ПЦР старше 3-х дней", "Кол-во ПЦР старше 7-х дней", "Кол-во противопоказаний" };
        }

        protected override void FormatCells(ExcelRange excelRange, int rangeIdx, int length, string[] columns)
        {
            base.FormatCells(excelRange, rangeIdx, length, columns);

            string headerRange = "B" + rangeIdx + ":" + GetColumnName(length) + rangeIdx;

            excelRange[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }
    }
}
