using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Contracts
{
    public class SourceFile
    {
        [Key]
        public int id { get; set; }

        public Guid userid { get; set; }

        [Required]
        public string FileName { get; set; }

        public string FileBody { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

    }
}
