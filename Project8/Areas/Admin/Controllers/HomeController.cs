using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project8.Models.Data;
using Project8.Models.Process;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;

namespace Project8.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //Trang quản lý

        //Khởi tạo biến dữ liệu : db
        BSDBContext db = new BSDBContext();

        public int isAdmin()
        {
            var model = Session["LoginAdmin"];

            //kiểm tra tính hợp lệ dữ liệu
            if (Session["LoginAdmin"] != null)
            {
                var result = db.Admins.SingleOrDefault(x => x.TaiKhoan == model);
                return 1;
            }
            return 0;
        }

        // GET: Admin/Home : trang chủ Admin
        public ActionResult Index()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        #region Sản phẩm

        //GET : Admin/Home/ShowListBook : Trang quản lý sách
        [HttpGet]
        public ActionResult ShowListBook()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //Gọi hàm ListAllBook và truyền vào model trả về View
            var model = new AdminProcess().ListAllBook();

            return View(model);
        }

        //GET : Admin/Home/AddBook : Trang thêm sách mới
        public ActionResult AddBook()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //lấy mã mà hiển thị tên
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(x => x.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTG = new SelectList(db.TacGias.ToList().OrderBy(x => x.TenTG), "MaTG", "TenTG");

            return View();
        }

        //POST : Admin/Home/AddBook : thực hiện thêm sách
        [HttpPost]
        public ActionResult AddBook(Sach sach, HttpPostedFileBase fileUpload)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //lấy mã mà hiển thị tên
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(x => x.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTG = new SelectList(db.TacGias.ToList().OrderBy(x => x.TenTG), "MaTG", "TenTG");

            //kiểm tra việc upload ảnh
            if (fileUpload == null)
            {
                ViewBag.Alert = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                //kiểm tra dữ liệu db có hợp lệ
                if (ModelState.IsValid)
                {
                    //lấy file đường dẫn
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //chuyển file đường dẫn và biên dịch vào /images
                    var path = Path.Combine(Server.MapPath("/images"), fileName);

                    //kiểm tra đường dẫn ảnh có tồn tại
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Alert = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    //thực hiện việc lưu đường dẫn ảnh vào link ảnh bìa
                    sach.AnhBia = fileName;
                    //thực hiện lưu vào db
                    var result = new AdminProcess().InsertBook(sach);
                    if (result > 0)
                    {
                        ViewBag.Success = "Thêm mới thành công";
                        //xóa trạng thái để thêm mới
                        ModelState.Clear();
                    }
                    else
                    {
                        ModelState.AddModelError("", "thêm không thành công.");
                    }
                }
            }

            return View();
        }

        //GET : Admin/Home/DetailsBook/:id : Trang xem chi tiết 1 cuốn sách
        [HttpGet]
        public ActionResult DetailsBook(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm lấy id sách và truyền vào View
            var sach = new AdminProcess().GetIdBook(id);

            return View(sach);
        }

        public ActionResult UpdateBook(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm lấy mã sách
            var sach = new AdminProcess().GetIdBook(id);

            //thực hiện việc lấy mã nhưng hiển thị tên và đúng tại mã đang chỉ định và gán vào ViewBag
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai", sach.MaLoai);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(x => x.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TacGias.ToList().OrderBy(x => x.TenTG), "MaTG", "TenTG", sach.MaTG);

            return View(sach);
        }

        //POST : /Admin/Home/UpdateBook : thực hiện việc cập nhật sách
        //Tương tự như thêm sách
        [HttpPost]
        public ActionResult UpdateBook(Sach sach, HttpPostedFileBase fileUpload)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //thực hiện việc lấy mã nhưng hiển thị tên ngay đúng mã đã chọn và gán vào ViewBag
            ViewBag.MaLoai = new SelectList(db.TheLoais.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai", sach.MaLoai);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(x => x.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTG = new SelectList(db.TacGias.ToList().OrderBy(x => x.TenTG), "MaTG", "TenTG", sach.MaTG);

            //Nếu không thay đổi ảnh bìa thì làm
            if (fileUpload == null)
            {
                //kiểm tra hợp lệ dữ liệu
                if (ModelState.IsValid)
                {
                    //gọi hàm UpdateBook cho việc cập nhật sách
                    var result = new AdminProcess().UpdateBook(sach);

                    if (result == 1)
                    {
                        ViewBag.Success = "Cập nhật thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật không thành công.");
                    }
                }
            }
            //nếu thay đổi ảnh bìa thì làm
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("/images"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Alert = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    sach.AnhBia = fileName;
                    var result = new AdminProcess().UpdateBook(sach);
                    if (result == 1)
                    {
                        ViewBag.Success = "Cập nhật thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "cập nhật không thành công.");
                    }
                }
            }

            return View(sach);
        }

        //DELETE : Admin/Home/DeleteBook/:id : thực hiện xóa 1 cuốn sách
        [HttpDelete]
        public ActionResult DeleteBook(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm DeleteBook để thực hiện xóa
            new AdminProcess().DeleteBook(id);

            //trả về trang quản lý sách
            return RedirectToAction("ShowListBook");
        }

        //Category

        //GET : /Admin/Home/ShowListCategory : trang quản lý thể loại
        [HttpGet]
        public ActionResult ShowListCategory()
        {

            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm ListAllCategory để hiện những thể loại trong db
            var model = new AdminProcess().ListAllCategory();

            return View(model);
        }

        //GET : Admin/Home/AddCategory : trang thêm thể loại
        [HttpGet]
        public ActionResult AddCategory()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        //POST : Admin/Home/AddCategory/:model : thực hiện việc thêm thể loại vào db
        [HttpPost]
        public ActionResult AddCategory(TheLoai model)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                //khởi tao biến admin trong Project8.Models.Process
                var admin = new AdminProcess();

                //khởi tạo biến thuộc đối tượng thể loại trong db
                var tl = new TheLoai();

                //gán thuộc tính tên thể loại
                tl.TenLoai = model.TenLoai;

                //gọi hàm thêm thể loại (InsertCategory) trong biến admin
                var result = admin.InsertCategory(tl);

                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    //xóa trạng thái
                    ModelState.Clear();

                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }

            return View(model);
        }

        //GET : Admin/Home/UpdateCategory/:id : trang cập nhật thể loại
        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm lấy mã thể loại
            var tl = new AdminProcess().GetIdCategory(id);

            //trả về dữ liệu View tương ứng
            return View(tl);
        }

        //POST : /Admin/Home/UpdateCategory/:id : thực hiện việc cập nhật thể loại
        [HttpPost]
        public ActionResult UpdateCategory(TheLoai tl)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //gọi hàm cập nhật thể loại
                var result = admin.UpdateCategory(tl);

                //thực hiện kiểm tra
                if (result == 1)
                {
                    return RedirectToAction("ShowListCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
            }

            return View(tl);
        }

        //DELETE : /Admin/Home/DeleteCategory:id : thực hiện xóa thể loại
        [HttpDelete]
        public ActionResult DeleteCategory(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            // gọi hàm xóa thể loại
            new AdminProcess().DeleteCategory(id);

            //trả về trang quản lý thể loại
            return RedirectToAction("ShowListCategory");
        }

        //Author

        //GET : /Admin/Home/ShowListAuthor : trang quản lý tác giả
        [HttpGet]
        public ActionResult ShowListAuthor()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm xuất danh sách tác giả trong db
            var model = new AdminProcess().ListAllAuthor();

            //trả về View tương ứng
            return View(model);
        }

        //GET : /Admin/Home/AddAuthor : trang thêm tác giả
        public ActionResult AddAuthor()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        //POST : /Admin/Home/AddAuthor/:model : thực hiện việc thêm tác giả
        [HttpPost]
        public ActionResult AddAuthor(TacGia model)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //khởi tạo đối tượng tg
                var tg = new TacGia();

                //gán dữ liệu
                tg.TenTG = model.TenTG;
                tg.QueQuan = model.QueQuan;
                tg.NgaySinh = model.NgaySinh;
                tg.NgayMat = model.NgayMat;
                tg.TieuSu = model.TieuSu;

                //gọi hàm thêm tác giả
                var result = admin.InsertAuthor(tg);

                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }

            return View(model);
        }

        //GET : /Admin/Home/UpdateAuthor/:id : trang thêm tác giả 
        [HttpGet]
        public ActionResult UpdateAuthor(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm lấy mã tác giả
            var tg = new AdminProcess().GetIdAuthor(id);

            return View(tg);
        }

        //POST : /Admin/Home/UpdateAuthor/:id : thực hiện việc thêm tác giả
        [HttpPost]
        public ActionResult UpdateAuthor(TacGia tg)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //kiểm tra hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //gọi hàm cập nhật tác giả
                var result = admin.UpdateAuthor(tg);
                //thực hiển kiểm tra
                if (result == 1)
                {
                    ViewBag.Success = "Cập nhật thành công";
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
            }

            return View(tg);
        }

        //DELETE : /Admin/Home/DeleteAuthor/:id : thực hiện xóa tác giả
        [HttpDelete]
        public ActionResult DeleteAuthor(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm xóa tác giả
            new AdminProcess().DeleteAuthor(id);

            return RedirectToAction("ShowListAuthor");
        }

        //Publish

        //GET : /Admin/Home/ShowListPublish : trang quản lý nhà xuất bản
        [HttpGet]
        public ActionResult ShowListPublish()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm xuất danh sách nhà xuất bản
            var model = new AdminProcess().ListAllPublish();

            return View(model);
        }

        //GET : /Admin/Home/AddPublish : trang quản lý nhà xuất bản
        public ActionResult AddPublish()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        //POST : /Admin/Home/AddPublish/:model : thực hiện việc thêm nhà xuất bản
        [HttpPost]
        public ActionResult AddPublish(NhaXuatBan model)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //khởi tạo object(đối tượng) nhà xuất bản
                var nxb = new NhaXuatBan();

                //gán dữ liệu
                nxb.TenNXB = model.TenNXB;
                nxb.DiaChi = model.DiaChi;
                nxb.DienThoai = model.DienThoai;

                //gọi hàm thêm nhà xuất bản
                var result = admin.InsertPublish(nxb);
                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }

            return View(model);
        }

        //GET : /Admin/Home/UpdatePublish/:id : trang thêm nhà xuất bản
        [HttpGet]
        public ActionResult UpdatePublish(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm lấy mã nhà xuất bản
            var nxb = new AdminProcess().GetIdPublish(id);

            return View(nxb);
        }

        //GET : /Admin/Home/UpdatePublish/:id : thực hiện thêm nhà xuất bản
        [HttpPost]
        public ActionResult UpdatePublish(NhaXuatBan nxb)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //gọi hàm cập nhật nhà xuất bản
                var result = admin.UpdatePublish(nxb);
                //kiểm tra hàm
                if (result == 1)
                {
                    ViewBag.Success = "Cập nhật nhật thành công";
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
            }

            return View(nxb);
        }

        //DELETE : Admin/Home/DeletePublish/:id : thực hiện xóa nhà xuất bản
        [HttpDelete]
        public ActionResult DeletePublish(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //gọi hàm xóa hàm xuất bản
            new AdminProcess().DeletePublish(id);

            //trả về trang quản lý nhà xuất bản
            return RedirectToAction("ShowListPublish");
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        OleDbConnection Econ;

        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        }
        private void InsertExceldata(string fileepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();

            oda.Fill(ds);

            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "Sach";
            objbulk.ColumnMappings.Add("MaLoai", "MaLoai");
            objbulk.ColumnMappings.Add("MaNXB", "MaNXB");
            objbulk.ColumnMappings.Add("MaTG", "MaTG");
            objbulk.ColumnMappings.Add("TenSach", "TenSach");
            objbulk.ColumnMappings.Add("GiaNhap", "GiaNhap");
            objbulk.ColumnMappings.Add("GiaBan", "GiaBan");
            objbulk.ColumnMappings.Add("Mota", "Mota");
            objbulk.ColumnMappings.Add("NguoiDich", "NguoiDich");
            objbulk.ColumnMappings.Add("AnhBia", "AnhBia");
            objbulk.ColumnMappings.Add("NgayCapNhat", "NgayCapNhat");
            objbulk.ColumnMappings.Add("SoLuongTon", "SoLuongTon");
            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
        }

        [HttpPost]
        public ActionResult AddBookFromFile(HttpPostedFileBase file)

        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/excelfolder/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
            InsertExceldata(filepath, filename);

            var model = new AdminProcess().ListAllBook();
            return View("ShowListBook", model);
        }

        #endregion

        #region Phản hồi

        //Contact/Feedback : Liên hệ / phản hồi khách hàng

        [HttpGet]
        //GET : Admin/Home/FeedBack : xem danh sách thông báo phản hồi
        public ActionResult FeedBack()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = new AdminProcess().ShowListContact();

            return View(result);
        }

        //GET : Admin/Home/FeedDetail/:id : xem nội dung phản hồi khách hàng
        public ActionResult FeedDetail(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = new AdminProcess().GetIdContact(id);

            return View(result);
        }

        //DELETE : Admin/Home/DeleteFeedBack/:id : xóa thông tin phản hồi khách hàng
        [HttpDelete]
        public ActionResult DeleteFeedBack(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            new AdminProcess().deleteContact(id);

            return RedirectToAction("FeedBack");
        }

        #endregion

        #region Người dùng

        //GET : /Admin/Home/ShowUser : trang quản lý người dùng
        public ActionResult ShowUser()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = new AdminProcess().ListUser();

            return View(result);
        }

        //GET : /Admin/Home/DetailsUser/:id : trang xem chi tiết người dùng
        public ActionResult DetailsUser(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = new AdminProcess().GetIdCustomer(id);

            return View(result);
        }

        //DELETE : Admin/Home/DeleteUser/:id : xóa thông tin người dùng
        [HttpDelete]
        public ActionResult DeleteUser(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            new AdminProcess().DeleteUser(id);

            return RedirectToAction("ShowUser");
        }

        #endregion

        #region Đơn đặt hàng

        //GET : Admin/Home/Order : trang quản lý đơn đặt hàng
        public ActionResult Order()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = new OrderProcess().ListOrder();

            return View(result);
        }

        //GET : /Admin/Home/DetailsOrder : trang xem chi tiết đơn hàng
        public ActionResult DetailsOrder(int id)
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = new OderDetailProcess().ListDetail(id);

            return View(result);
        }

        #endregion

        #region Báo cáo
        [HttpGet]
        //GET : /Admin/Home/Report/ : Xem báo cáo sản phẩm
        public ActionResult Report()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = new BookProcess().getNumberBookkSold();
            return View(model);
        }
        [HttpGet]
        //GET : /Admin/Home/ExportReport/ : Xem báo cáo sản phẩm
        public ActionResult ExportReport()
        {
            if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = new BookProcess().getExcelExportData();
            return View(model);
        }
        [HttpPost]
        public ActionResult ExportToExcel()
        {if (isAdmin() == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = new BookProcess().getExcelExportData();
            var gv = new GridView();
            gv.DataSource = model;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            int length = DateTime.Now.Date.ToString().IndexOf(' ');
            string date = DateTime.Now.Date.ToString().Substring(0, length);
            Response.AddHeader("content-disposition", "attachment; filename=DoanhThu"+ date + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("ExportReport", model);
        }
        #endregion

    }
}