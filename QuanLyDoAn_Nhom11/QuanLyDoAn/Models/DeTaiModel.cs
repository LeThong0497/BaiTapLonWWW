using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoAn.Models
{
    public class DeTaiModel
    {
        public string MaDeTai { get; set; }

        public string TenDeTai { get; set; }

        public int SoSinhVienThamGia { get; set; }

        public string GhiChu { get; set; }

        public int SoSinhVienDangKy { get; set; }
    }
}