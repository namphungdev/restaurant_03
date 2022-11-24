using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Restaurant.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }
        [DisplayName("Mã loại sản phẩm")]
        public int MaLoaiSanPham { get; set; }
        [Required(ErrorMessage = "Không được để trống!")]
        [DisplayName("Tên loại sản phẩm")]
        public string TenLoaiSanPham { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
