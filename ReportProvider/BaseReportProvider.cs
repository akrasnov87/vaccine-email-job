using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using OfficeOpenXml;
using Vaccine.Models;

namespace Vaccine.ReportProvider
{
    public abstract class BaseReportProvider: IDisposable
    {
        protected string[][] items;

        public BaseReportProvider(string[][] items)
        {
            this.items = items;
        }

        public void Dispose()
        {
            Stream stream = GetSteam();
            if (stream != null)
            {
                stream.Dispose();
            }
        }
        public abstract string[] GetHeaders();

        public Stream GetSteam()
        {
            MemoryStream memoryStream = new MemoryStream();

            string[] headers = GetHeaders();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                var excelWorksheet = excel.Workbook.Worksheets["Worksheet1"];

                string headerRange = "A1:" + GetColumnName(headers.Length) + "1";
                excelWorksheet.Cells[headerRange].LoadFromArrays(new string[][] { headers.ToArray() });

                int range = 2;

                for (int j = 0; j < items.Length; j++)
                {
                    var columns = items[j];

                    headerRange = "A" + range + ":" + GetColumnName(headers.Length) + range;
                    excelWorksheet.Cells[headerRange].LoadFromArrays(new string[][] { columns });

                    range++;
                }

                excel.SaveAs(memoryStream);
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        protected string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }
    }
}
