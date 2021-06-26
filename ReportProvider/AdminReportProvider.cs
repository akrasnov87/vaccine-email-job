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
            return new string[] { "Наименование", "Количество вакцинированных", "Количество ПЦР" };
        }
    }
}
