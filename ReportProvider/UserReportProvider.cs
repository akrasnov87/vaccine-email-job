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
    /// Сводный отчет уровня «Ответственного»
    /// </summary>
    public class UserReportProvider : BaseReportProvider
    {
        public UserReportProvider(string[][] items)
            : base(items)
        {
            
        }

        public override string[] GetHeaders()
        {
            return new string[] { "Количество вакцинированных", "Количество ПЦР" };
        }
    }
}
