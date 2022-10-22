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
        [Required]
        [DisplayName("Tên khách hàng")]
        public string TenKhachHang { get; set; }
        [StringLength(10)]
        [Required]
        public string Sdt { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DiaChi { get; set; }
        public int? MaChucVu { get; set; }

        public virtual ChucVu MaChucVuNavigation { get; set; }
        public virtual ICollection<GopY> Gopies { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
