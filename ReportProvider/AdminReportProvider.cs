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
            return new string[] { "Наименование", "Количество\nвакцинированных", "Количество\nсдающих ПЦР", "Количество\nпротивопоказаний" };
        }

        protected override void FormatCells(ExcelRange excelRange, int rangeIdx, int length)
        {
            base.FormatCells(excelRange, rangeIdx, length);

            string headerRange = "B" + rangeIdx + ":" + GetColumnName(length) + rangeIdx;

            excelRange[headerRange].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }
    }
}
