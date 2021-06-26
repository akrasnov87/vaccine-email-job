using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Vaccine.Models
{
    [Table("dd_documents", Schema = "core")]
    public class File
    {
        [Key]
        public Guid id { get; set; }

        public byte[] ba_jpg { get; set; }

        public byte[] ba_pdf { get; set; }

        public Guid f_document { get; set; }
        public DateTime dx_created { get; set; }
    }
}
