using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoAn.Models
{
    public class GiangVienModel
    {
        public string MaGiangVien { get; set; }
        public string TenGiangVien { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
    }
}