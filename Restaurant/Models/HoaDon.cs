using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace Restaurant.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [DisplayName("Mã hóa đơn")]
        public int MaHoaDon { get; set; }
        [DisplayName("Mã khách hàng")]
        public int? MaKhachHang { get; set; }
        [DisplayName("Ngày lập")]
        public DateTime? NgayLap { get; set; }
        [DisplayName("Tổng tiền")]
        public decimal? TongTien { get; set; }
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }
        [DisplayName("Số điện thoại")]
        public string Sdt { get; set; }
        [DisplayName("Thanh toán")]
        public int? MaThanhToan { get; set; }
        [DisplayName("Vận chuyển")]
        public int? MaVanChuyen { get; set; }

        public virtual KhachHang MaKhachHangNavigation { get; set; }
        public virtual ThanhToan MaThanhToanNavigation { get; set; }
        public virtual VanChuyen MaVanChuyenNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
