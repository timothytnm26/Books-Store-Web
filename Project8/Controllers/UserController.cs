using Project8.Areas.Admin.Models;
using Project8.Models.Data;
using Project8.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace Project8.Controllers
{
    public class userController : Controller
    {
        //Khởi tạo biến dữ liệu : db
        BSDBContext db = new BSDBContext();

        
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(KhachHang model)
        {
            
            if (ModelState.IsValid)
            {
                HttpClient hc = new HttpClient();
                hc.BaseAddress = new Uri("http://localhost:54921/api/users");


                var insertrec = hc.PostAsJsonAsync<KhachHang>("users", model);
                insertrec.Wait();

                var saverec = insertrec.Result;
                if (saverec.IsSuccessStatusCode)
                {
                    //Session["User"] = result;
                    ModelState.Clear();
                    //return Redirect("/Home/");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký không thành công.");                       
                }
            }
            return PartialView();
        }


    
        //GET : /User/LoginPage : trang đăng nhập
        [HttpGet]
        public ActionResult LoginPage()
        {
            return View();
        }

        //POST : /User/LoginPage : thực hiện đăng nhập
        [HttpPost]
        public ActionResult LoginPage(LoginModel model)
        {
            //kiểm tra hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //gọi hàm đăng nhập trong AdminProcess và gán dữ liệu trong biến model
                var result = new UserProcess().Login(model.TaiKhoan, model.MatKhau);
                //Nếu đúng
                if (result == 1)
                {
                    //gán Session["LoginAdmin"] bằng dữ liệu đã đăng nhập
                    Session["User"] = model.TaiKhoan;
                    //trả về trang chủ
                    return RedirectToAction("Index", "Home");
                }
                //nếu tài khoản không tồn tại
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                    //return RedirectToAction("LoginPage", "User");
                }
                //nếu nhập sai tài khoản hoặc mật khẩu
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                    //return RedirectToAction("LoginPage", "User");
                }
            }

            return View();
        }

        //GET : /User/Login : đăng nhập tài khoản
        //Parital View : Login
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Login()
        {
            return PartialView();
        }

        //POST : /User/Login : thực hiện đăng nhập
        [HttpPost]
        [ChildActionOnly]
        public ActionResult Login(LoginModel model)
        {
            //kiểm tra hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //gọi hàm đăng nhập trong AdminProcess và gán dữ liệu trong biến model
                var result = new UserProcess().Login(model.TaiKhoan, model.MatKhau);

                //Nếu đúng
                if (result == 1)
                {
                    //gán Session["LoginAdmin"] bằng dữ liệu đã đăng nhập
                    Session["User"] = model.TaiKhoan;
                    //trả về trang chủ
                    return RedirectToAction("Index", "Home");
                }
                //nếu tài khoản không tồn tại
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                    //return RedirectToAction("LoginPage", "User");
                }
                //nếu nhập sai tài khoản hoặc mật khẩu
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                    //return RedirectToAction("LoginPage", "User");
                }
            }

            return PartialView();
        }

        //GET : /User/Logout : đăng xuất tài khoản khách hàng
        [HttpGet]
        public ActionResult Logout()
        {
            Session["User"] = null;

            return RedirectToAction("Index", "Home");
        }

        //GET : /User/EditUser : cập nhật thông tin khách hàng
        [HttpGet]
        public ActionResult EditUser()
        {
            //lấy dữ liệu từ session
            var model = Session["User"];

            if (ModelState.IsValid)
            {
                //tìm tên tài khoản
                var result = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan == model);

                //trả về dữ liệu tương ứng
                return View(result);
            }

            return View();
        }

        //POST : /User/EditUser : thực hiện việc cập nhật thông tin khách hàng
        [HttpPost]
        public ActionResult EditUser(KhachHang model)
        {
            //gọi hàm cập nhật thông tin khách hàng
            var result = new UserProcess().UpdateUser(model);

            //thực hiện kiểm tra
            if (result == 1)
            {
                return RedirectToAction("EditUser");                  
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật không thành công.");
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult EditPassword(FormCollection model)
        {
            var userName = Session["User"];
            var user = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan == userName);

            string oldPass = model["OldPass"];
            string newPass = model["NewPass"];
            string rePass = model["RePass"];

            if(oldPass!= user.MatKhau)
            {
                ModelState.AddModelError("", "Sai mật khẩu cũ.");
                return View("EditUser", user);
            }
            else if (newPass!=rePass)
            {
                ModelState.AddModelError("", "Mật khẩu mới không khớp.");
                return View("EditUser", user);
            }
            else
            {
                user.MatKhau = newPass;
                var r = new UserProcess().UpdateUser(user);
            }
            return View("EditUser", user);
        }

    }
}