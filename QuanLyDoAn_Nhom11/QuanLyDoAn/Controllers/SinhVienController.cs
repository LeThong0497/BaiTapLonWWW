using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyDoAn.Models;

namespace QuanLyDoAn.Controllers
{
    public class SinhVienController : Controller
    {
        QLDoAnSinhVienEntities db = new QLDoAnSinhVienEntities();
        // GET: SinhVien
        public ActionResult DanhSachDeTai()
        {
            var detais = db.DeTais.ToList();
            return View(detais);
        }
        [HttpPost]
        public JsonResult DangKyDeTai(string MaSinhVien, string MaDeTai)
        {

            int count = 0;
            var sv = db.SinhViens.Where(x => x.maSinhVien == MaSinhVien).FirstOrDefault();
            if (sv.maDeTai != null)
                return Json("false", JsonRequestBehavior.AllowGet);
            var s = db.DeTais.Where(x => x.maDeTai == MaDeTai).First();
            if (s.soSVDaDangKy < s.soSVThamGia)
            {
                var dtai = db.DeTais.Where(x => x.maDeTai == MaDeTai).FirstOrDefault();
                dtai.soSVDaDangKy += 1;
                count = (int)dtai.soSVDaDangKy;

                sv.maDeTai = MaDeTai;
                db.SaveChanges();
            }
            else
            {
                return Json("Đã đủ số lượng", JsonRequestBehavior.AllowGet);
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Index(string id)
        {
            var sv = db.SinhViens.Where(x => x.maSinhVien == id).FirstOrDefault();
            if (sv.maDeTai == null)
            {
                ViewBag.MaDeTai = "null";
            }
            return View(sv);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var sv = db.SinhViens.Where(x => x.maSinhVien == id).FirstOrDefault();
            if (sv != null)
            {
                return View("Edit", sv);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit(SinhVien sv)
        {
            var stev = db.SinhViens.Where(x => x.maSinhVien == sv.maSinhVien).FirstOrDefault<SinhVien>();
            stev.tenSinhVien = sv.tenSinhVien;
            stev.email = sv.email;
            stev.soDienThoai = sv.soDienThoai;
            stev.ngaySinh = sv.ngaySinh;
            stev.gioiTinh = sv.gioiTinh;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = stev.maSinhVien });
        }

        [HttpGet] 
        public ActionResult ThongTinDeTai(string id)
        {
            if (id == null)
            {
                return RedirectToAction("DanhSachDeTai");
            }
            
            var dsdt = from dt in db.DeTais
                       join hd in db.HuongDans on dt.maDeTai equals hd.maDeTai
                       join gv in db.GiangViens on hd.maGiangVien equals gv.maGiangVien
                       where dt.maDeTai == id
                       select new
                       {
                           MaDeTai = dt.maDeTai,
                           TenDeTai = dt.tenDeTai,
                           SLSVThamGia = dt.soSVThamGia.ToString(),
                           GhiChu = dt.ghiChu,
                           SLSVDangKy = dt.soSVDaDangKy.ToString(),
                           NhanXet = hd.nhanXet,
                           Diem = hd.diemDeTai.ToString(),
                           TenGV = gv.tenGiangVien,
                           email = gv.email,
                           soDienThoai = gv.soDienThoai
                       };

            
                ChiTietDeTaiModel chiTietDeTaiModel = new ChiTietDeTaiModel();
                foreach (var item in dsdt)
                {
                    chiTietDeTaiModel.MaDeTai = item.MaDeTai;
                    chiTietDeTaiModel.TenDeTai = item.TenDeTai;
                    chiTietDeTaiModel.SoSinhVienThamGia = item.SLSVThamGia;
                    chiTietDeTaiModel.GhiChu = item.GhiChu;
                    chiTietDeTaiModel.SoSinhVienDangKy = item.SLSVDangKy;
                    chiTietDeTaiModel.NhanXet = item.NhanXet;
                    chiTietDeTaiModel.DiemDeTai = item.Diem;
                    chiTietDeTaiModel.TenGiangVien = item.TenGV;
                    chiTietDeTaiModel.Email = item.email;
                    chiTietDeTaiModel.SoDienThoai = item.soDienThoai;
                }
            
                
                return View(chiTietDeTaiModel);

            //var tblDeTai = db.DeTais.Where(x => x.maDeTai == id).FirstOrDefault();
            //var tblHuongDan = db.HuongDans.Where(x => x.maDeTai == tblDeTai.maDeTai).FirstOrDefault();
            //var tblGiangVien = db.GiangViens.Where(x => x.maGiangVien == tblHuongDan.maGiangVien).FirstOrDefault();

            //ChiTietDeTaiModel chiTietDeTaiModel = new ChiTietDeTaiModel();
            //chiTietDeTaiModel.MaDeTai = tblHuongDan.maDeTai;
            //chiTietDeTaiModel.TenDeTai = tblDeTai.tenDeTai;
            //chiTietDeTaiModel.SoSinhVienThamGia = tblDeTai.soSVThamGia.ToString();
            //chiTietDeTaiModel.GhiChu = tblDeTai.ghiChu;
            //chiTietDeTaiModel.SoSinhVienDangKy = tblDeTai.soSVDaDangKy.ToString();
            //chiTietDeTaiModel.NhanXet = tblHuongDan.nhanXet;
            //chiTietDeTaiModel.DiemDeTai = tblHuongDan.diemDeTai.ToString();
            //chiTietDeTaiModel.TenGiangVien = tblGiangVien.tenGiangVien;
            //chiTietDeTaiModel.Email = tblGiangVien.email;
            //chiTietDeTaiModel.SoDienThoai = tblGiangVien.soDienThoai;
            //return View(chiTietDeTaiModel);
        }
    }
}