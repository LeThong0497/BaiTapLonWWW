using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDoAn.Controllers
{
    public class HomeController : Controller
    {
        private QLDoAnSinhVienEntities db = new QLDoAnSinhVienEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["error1"] = " Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["error2"] = "Phải nhập mật khẩu";

            }
            else
            {
                TaiKhoan ad = db.TaiKhoans.SingleOrDefault(n => n.ID == tendn && n.passWord == matkhau);
                if (ad != null)
                {
                    Session["TaiKhoanAdmin"] = ad.ID.ToString();
                    if (ad.loaiTaiKhoan == "GiangVien")
                    {
                        return RedirectToAction("Index", "GiangVien");
                    }
                    else if (ad.loaiTaiKhoan == "SinhVien")
                    {
                        return RedirectToAction("Index", "SinhVien", new { id = Session["TaiKhoanAdmin"] });
                        
                    }
                    else
                        return RedirectToAction("TrangChu", "Admin");

                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";

            }
            return View();
        }
    }
}