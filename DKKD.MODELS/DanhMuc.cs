using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DKKD.MODELS
{
    [Table("DanhMuc")]
    public class DanhMuc
    {
        public int ID { get; set; }
        public string TenDM { get; set; }
        public int? TrangThai { get; set; }
    }
}
