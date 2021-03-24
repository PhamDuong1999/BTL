using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DKKD.MODELS
{
    [Table("NhaCungCap")]
    public class NhaCungCap
    {
        [Key]
        public int ID { get; set; }
        public string TenNCC { get; set; }
        public string Logo { get; set; }
        public int? TrangThai { get; set; }
    }
}
