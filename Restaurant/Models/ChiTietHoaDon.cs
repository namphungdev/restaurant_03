using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Restaurant.Models
{
    public partial class ChiTietHoaDon
    {
        [DisplayName("Mã hóa đơn")]
        public int MaHoaDon { get; set; }
        [DisplayName("Mã sản phẩm")]
        public int MaSanPham { get; set; }
        [DisplayName("Tổng tiền")]
        public int? TongTien { get; set; }
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }

        public virtual HoaDon MaHoaDonNavigation { get; set; }
        public virtual SanPham MaSanPhamNavigation { get; set; }
    }
}
