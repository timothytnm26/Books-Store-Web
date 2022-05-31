using Project8.Areas.Admin.Models;
using Project8.Models.Data;
using Project8.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Project8.Controllers
{
    public class usersController : ApiController
    {
        public IHttpActionResult userform(KhachHang kh)
        {
            
            BSDBContext db = new BSDBContext();
            var user = new UserProcess();
            if (user.CheckUsername(kh.TaiKhoan, kh.MatKhau) == 1)
            {
                ModelState.AddModelError("", "Tài khoản đã tồn tại");
                return Conflict();
            }
            else if (user.CheckUsername(kh.TaiKhoan, kh.MatKhau) == -1)
            {
                ModelState.AddModelError("", "Tài khoản đã tồn tại");
                return Conflict();
            }
            else
            {
                db.KhachHangs.Add(new KhachHang()
                {
                    MaKH = kh.MaKH,
                    TenKH = kh.TenKH,
                    Email = kh.Email,
                    DiaChi = kh.DiaChi,
                    DienThoai = kh.DienThoai,
                    NgaySinh = kh.NgaySinh,
                    TaiKhoan = kh.TaiKhoan,
                    MatKhau = kh.MatKhau,
                    NgayTao = DateTime.Now.Date,
                    TrangThai = true
                });
                db.SaveChanges();
                return Ok();
            }
           // return Conflict();
            

        }
        //Khởi tạo biến dữ liệu : db
        

    }
}
