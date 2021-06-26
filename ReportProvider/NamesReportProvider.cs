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
            return new string[] { "ФИО", "Дата рождения", "Вакцинирован", "Дата сохранения информации о вакцинации", "ПЦР", "Дата внесения информации о ПЦР" };
        }
    }
}
