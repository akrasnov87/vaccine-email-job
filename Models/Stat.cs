using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vaccine.Models
{
    [Table("vw_stat", Schema = "rpt")]
    public class Stat
    {
        [Key]
        public Guid f_document { get; set; }
        public int f_user { get; set; }
        public long n_jpg { get; set; }
        public long n_pdf { get; set; }
        public DateTime? dx_created { get; set; }
        public bool b_ignore { get; set; }
    }
}
