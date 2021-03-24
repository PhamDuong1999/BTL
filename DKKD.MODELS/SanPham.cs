using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DKKD.MODELS
{
    [Table("SanPham")]
    public class SanPham
    {
        public int ID { get; set; }
        public string TenSP { get; set; }
        public string GioiThieu { get; set; }
        public string MoTa { get; set; }
        public string Avatar { get; set; }
        public int? SoLuong { get; set; }
        public int? DonGia { get; set; }
        public int? Sale { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public int? MaNCC { get; set; }
        public int? MaDM { get; set; }
        public int? TrangThai { get; set; }
        [NotMapped]
        public string TenNCC { get; set; }
        [NotMapped]
        public string TenDM { get; set; }
        [NotMapped]
        public int sldaban { get; set; }
    }
}
