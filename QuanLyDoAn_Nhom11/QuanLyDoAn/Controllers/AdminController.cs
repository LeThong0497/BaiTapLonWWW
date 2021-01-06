using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Linq;
using System.Data.Entity;
using System.Net;

namespace QuanLyDoAn.Controllers
{
   
    public class AdminController : Controller
    {

        private QLDoAnSinhVienEntities db = new QLDoAnSinhVienEntities();
        // GET: Admin/Admin
        public ActionResult TrangChu()
        {
            string id = Session["TaiKhoanAdmin"].ToString();
            var ad= db.QuanTris.Where(p => p.maQuanLy == id).FirstOrDefault();
            return View(ad);
        }
        public ActionResult TaiKhoan(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;

            return View(db.TaiKhoans.ToList().OrderBy(n=> n.loaiTaiKhoan).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult XoaTaiKhoan(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan tk = db.TaiKhoans.Find(id);
            if (tk == null)
            {
                return HttpNotFound();
            }
            return View(tk);
        }
        [HttpPost, ActionName("XoaTaiKhoan")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {

            TaiKhoan tk = db.TaiKhoans.Find(id);
            if (tk != null)
            {
                db.TaiKhoans.Remove(tk);
                db.SaveChanges();
                return RedirectToAction("TaiKhoan");
            }

            return View(tk);

        }
        public ActionResult UpdateTaiKhoan(string id)
        {
            TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = tk.ID;
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.ID = new SelectList(db.TaiKhoans.ToList().OrderBy(n => n.loaiTaiKhoan), "ID", "loaiTaiKhoan");
            return View(tk);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateTaiKhoan(TaiKhoan tk)
        {
           
            if (ModelState.IsValid)
            {
              
                db.Entry(tk).State = EntityState.Modified;
               
                db.SaveChanges();
                return RedirectToAction("TaiKhoan");
            }
            
            return View(tk);
        }

        //-----------------------------------

        public ActionResult DeTai()
        {
           
            var detais = db.DeTais.ToList();
            return View(detais);
        }
        public ActionResult ChiTietDeTai(string id)
        {
            DeTai d = db.DeTais.FirstOrDefault(n => n.maDeTai == id);
            if (d == null)
            {
                return HttpNotFound();
            }
            return View(d);
        }


        public ActionResult ThemDeTai()
        {
            return View();
        }

        // POST: SinhVien/Create
        [HttpPost]
        public ActionResult ThemDeTai(DeTai d)
        {
            if (ModelState.IsValid)
            {
                db.DeTais.Add(d);
                db.SaveChanges();
                return RedirectToAction("DeTai");
            }
           
            return View(d);
        }
        public ActionResult SuaDeTai(string id)
        {

            var dt = db.DeTais.Where(p => p.maDeTai == id).FirstOrDefault<DeTai>();
            if (dt == null)
                return HttpNotFound();
            return View(dt);
        }
        [HttpPost]
        public ActionResult SuaDeTai(DeTai dt)
        {
            string id = Session["TaiKhoanAdmin"].ToString();
            using (var d = new QLDoAnSinhVienEntities())
            {
                var detai = d.DeTais.Where(s => s.maDeTai == dt.maDeTai)
                                                    .FirstOrDefault<DeTai>();

                if (detai != null)
                {
                    detai.tenDeTai = dt.tenDeTai;
                    detai.ghiChu = dt.ghiChu;
                    detai.soSVThamGia = dt.soSVThamGia;
                    detai.soSVDaDangKy = dt.soSVDaDangKy;

                    d.SaveChanges();
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return RedirectToAction("DeTai", new { magv = id });
        }
        public ActionResult XoaDeTai(string id)
        {
            DeTai detai = db.DeTais.Single(d => d.maDeTai == id);
            return View(detai);
        }
        [HttpPost, ActionName("XoaDeTai")]
        public ActionResult DeleteConfirm(string id)
        {
            string ma = Session["TaiKhoanAdmin"].ToString();
            var sv = from s in db.SinhViens
                     where s.maDeTai == id
                     select s.maSinhVien;


            if (sv != null)
            {
                return View(RedirectToAction("DeTai", ma));
            }
            else
            {
                DeTai detai = db.DeTais.Single(d => d.maDeTai == id);
                HuongDan huongdan = db.HuongDans.Single(h => h.maDeTai == id);
                db.HuongDans.Remove(huongdan);
                db.DeTais.Remove(detai);
                db.SaveChanges();
                return View(RedirectToAction("DeTai", ma));
            }



        }
    }
}