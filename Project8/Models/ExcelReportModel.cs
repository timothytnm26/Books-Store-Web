using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project8.Models
{
    public class ExcelReportModel
    {
        public int MaDDH { get; set; }
        public DateTime? NgayDat { get; set; }
        public string TenSach { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
    }
}