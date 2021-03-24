using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DKKD.MODELS
{
    [Table("DonHang")]
    public class DonHang
    {
        public int ID { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TongTienTT { get; set; }
        public int? TrangThai { get; set; }
        [NotMapped]
        public List<CartItem> ListCTDH { get; set; }
    }
}
