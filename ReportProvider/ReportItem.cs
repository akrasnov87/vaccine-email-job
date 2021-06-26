using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Vaccine.ReportProvider
{
    public class ReportItem
    {
        public Stream stream { get; set; }
        public string name { get; set; }
    }
}
