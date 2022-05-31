using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Project8.Models.Data
{
    
    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        [Display(Name ="ID ADMIN")]
        public int IDAdmin { get; set; }

        [StringLength(50)]
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage ="Nhập tài khoản")]
        public string TaiKhoan { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }
    }
}
