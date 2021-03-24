using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DKKD.MODELS
{
    [Table("ChiTietDH")]
    public class CartItem
    {
        public int ID { get; set; }
        public int MaDH { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }

        [NotMapped]
        public string TenSP { get; set; }
        [NotMapped]
        public string Avatar { get; set; }
        [NotMapped]
        public double ThanhTien => SoLuong * DonGia;
    }
}
