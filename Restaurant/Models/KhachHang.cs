using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Restaurant.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            Gopies = new HashSet<GopY>();
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaKhachHang { get; set; }
        [Required(ErrorMessage = "Tên khách hàng không được để trống!")]
        [DisplayName("Tên khách hàng")]
        public string TenKhachHang { get; set; }
        [StringLength(10)]
        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại chưa đúng định dạng!")]
        public string Sdt { get; set; }
        [Required(ErrorMessage = "Email không được để trống!")]
        [RegularExpression(@"\b[A-Za-z0-9._%-]+@+[A-Za-z0-9._%-]+(.com)\b", ErrorMessage = ("Email phải đúng định dạng @gmail.com"))]
        [EmailAddress(ErrorMessage = "Email chưa đúng định dạng!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password không được để trống!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }
        public int? MaChucVu { get; set; }

        public virtual ChucVu MaChucVuNavigation { get; set; }
        public virtual ICollection<GopY> Gopies { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
