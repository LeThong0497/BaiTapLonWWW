using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDoAn.Controllers
{
    [HandleError]
    public class GiangVienController : Controller
    {
        QLDoAnSinhVienEntities db = new QLDoAnSinhVienEntities();
        //string idGV = HomeController.
        // GET: GiangVien
        public ActionResult Index()
        {
            string id = Session["TaiKhoanAdmin"].ToString();
            var gv = db.GiangViens.Where(p => p.maGiangVien == id).FirstOrDefault();
            return View(gv);
        }

        public ActionResult GetSVHuongDan(string magv)
        {
            string id = Session["TaiKhoanAdmin"].ToString();
            var dssv = from sv in db.SinhViens
                       join hd in db.HuongDans on sv.maDeTai equals hd.maDeTai
                       join gv in db.GiangViens on hd.maGiangVien equals gv.maGiangVien
                       where gv.maGiangVien == id
                       select sv;
            return View(dssv);
        }
        //Quan ly de tai
        public ActionResult QuanLyDeTai(string magv)
        {
            string id = Session["TaiKhoanAdmin"].ToString();
            var dsdt = from hd in db.HuongDans
                       join dt in db.DeTais on hd.maDeTai equals dt.maDeTai
                       where hd.maGiangVien == id
                       select dt;
            return View(dsdt);
        }
        //Sua de tai
        public ActionResult EditDeTai(string id)
        {
            var dt = db.DeTais.Where(p => p.maDeTai == id).FirstOrDefault<DeTai>();
            if (dt == null)
                return HttpNotFound();
            return View(dt);
        }
        [HttpPost]
        public ActionResult EditDeTai(DeTai dt)
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

            return RedirectToAction("QuanLyDeTai", new { magv = id});
        }

        //Xoa de tai
        public ActionResult Delete(string id)
        {
            DeTai detai = db.DeTais.Single(d => d.maDeTai == id);
            return View(detai);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            string ma = Session["TaiKhoanAdmin"].ToString();
            var sv = from s in db.SinhViens
                     where s.maDeTai == id
                     select s.maSinhVien;


            if (sv != null)
            {
                return View(RedirectToAction("QuanLyDeTai", ma));
            }
            else
            {
                //DeTai detai = db.DeTais.Single(d => d.maDeTai == id);
                //HuongDan huongdan = db.HuongDans.Single(h => h.maDeTai == id);
                var huongdan = db.HuongDans.Where(p => p.maDeTai == id).FirstOrDefault();
                var detai = db.DeTais.Where(p => p.maDeTai == id).FirstOrDefault();
                
                db.HuongDans.Remove(huongdan);
                db.DeTais.Remove(detai);
                db.SaveChanges();
                return View(RedirectToAction("QuanLyDeTai", ma));
            }



        }




        //    public ActionResult DeleteDeTai(string id)
        //{
        //    DeTai detai = db.DeTais.Find(id);
        //    if (detai == null)
        //        return HttpNotFound();
        //    return View(detai);
        //}
        //[HttpPost]
        //public ActionResult DeleteDeTaiConfirm(string id)
        //{
        //    //if (dt.soSVThamGia == 0)
        //    //{
        //    //using (var d = new QLDoAnSinhVienEntities())
        //    //{

        //    //    var detai = d.DeTais
        //    //    .Where(s => s.maDeTai == dt.maDeTai)
        //    //    .FirstOrDefault();

        //    //    d.Entry(detai).State = System.Data.Entity.EntityState.Deleted;
        //    //    d.SaveChanges();

        //    //    return View(dt);
        //    //}
        //    DeTai detai = db.DeTais.Find(id);
        //    if (detai == null)
        //        return HttpNotFound();
        //    //HuongDan hd = db.HuongDans.Find(id, "10013401");
        //    //db.HuongDans.Remove(hd);


        //    db.DeTais.Remove(detai);
        //    db.SaveChanges();
        //    return RedirectToAction("QuanLyDeTai", new { magv = "10013401" });
        //    //}
        //    //return View();       
        //}

        //cham diem
        public ActionResult ChamDiem(string id)
        {
            var hd = db.HuongDans.Where(p => p.maDeTai == id).FirstOrDefault<HuongDan>();
            if (hd == null)
                return HttpNotFound();
            return View(hd);
        }
        [HttpPost]
        public ActionResult ChamDiem(HuongDan hd)
        {
            string id = Session["TaiKhoanAdmin"].ToString();
            using (var d = new QLDoAnSinhVienEntities())
            {
                var huongdan = d.HuongDans.Where(s => s.maDeTai == hd.maDeTai)
                                                    .FirstOrDefault<HuongDan>();

                if (huongdan != null)
                {
                    huongdan.diemDeTai = hd.diemDeTai;
                    huongdan.nhanXet = hd.nhanXet;

                    d.SaveChanges();
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return RedirectToAction("QuanLyDeTai", new { magv = id });
        }

    }


}