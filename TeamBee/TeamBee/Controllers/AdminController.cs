using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Rotativa.AspNetCore;
using System;
using System.Drawing;
using System.Linq;
using TeamBee.Connect_Data;
using TeamBee.Helper;
using TeamBee.ViewModels;
using TeamBee.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using TeamBee.Function;
namespace TeamBee.Controllers
{
    public class AdminController : Controller
    {
        private readonly TeamBeeDbContext _context;
        private readonly IHostingEnvironment _hostingEnviroment;
        public AdminController(TeamBeeDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnviroment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.DonHang = _context.Orders.ToList();
                ViewBag.SanPham = _context.Products.ToList();
                ViewBag.KhachHang = _context.Users.Where(p => p.Permission == 0).ToList();
                ViewBag.QuanLy = _context.Users.Where(p => p.Permission == 1).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult DonHang(string name)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.Tatca = _context.Orders.ToList();
                ViewBag.xl = _context.Orders.Where(p => p.Status == "0").ToList();
                ViewBag.danggiao = _context.Orders.Where(p => p.Status == "1").ToList();
                ViewBag.hoanthanh = _context.Orders.Where(p => p.Status == "2").ToList();
                ViewBag.dahuy = _context.Orders.Where(p => p.Status == "3").ToList();
                if (name == "tat-ca-don-hang")
                {
                    ViewBag.DonHang = _context.Orders.OrderBy(p => p.Id).ToList();
                    HttpContext.Session.SetInt32("excel", 4);
                }
                else if (name == "chua-xu-ly")
                {
                    ViewBag.DonHang = _context.Orders.Where(p => p.Status == "0").OrderByDescending(p => p.Id).ToList();
                    HttpContext.Session.SetInt32("excel", 0);
                }
                else if (name == "dang-giao-hang")
                {
                    ViewBag.DonHang = _context.Orders.Where(p => p.Status == "1").OrderByDescending(p => p.Id).ToList();
                    HttpContext.Session.SetInt32("excel", 1);
                }
                else if (name == "da-huy")
                {
                    ViewBag.DonHang = _context.Orders.Where(p => p.Status == "3").OrderByDescending(p => p.Id).ToList();
                    HttpContext.Session.SetInt32("excel", 3);
                }
                else
                {
                    ViewBag.DonHang = _context.Orders.Where(p => p.Status == "2").OrderByDescending(p => p.Id).ToList();
                    HttpContext.Session.SetInt32("excel", 2);
                }
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }


        public IActionResult Indonhang(int id)
        {

            var donhang = _context.Orders.Where(p => p.Id == id).SingleOrDefault();
            HttpContext.Session.SetString("ten", donhang.ShipName);
            HttpContext.Session.SetString("sdt", donhang.ShipPhone);
            HttpContext.Session.SetString("diachi", donhang.Address);
            HttpContext.Session.SetString("iddonhang", donhang.Id.ToString());
            HttpContext.Session.SetString("ngay", donhang.DateCreate.ToString());
            DonHangChiTiet donHangChiTiet = new DonHangChiTiet(_context);

            SessionHelper.Set(HttpContext.Session, "DonHang", donHangChiTiet.GetDonHangChiTiet(donhang.Id));
            HttpContext.Session.SetInt32("total", donHangChiTiet.GetDonHangChiTiet(donhang.Id).Sum(p => p.Gia * p.SoLuong));

            return new ViewAsPdf();
            //return View();
        }


        // Đừng cố gắng đọc nha mù mắt đó :D
        public IActionResult Export(int trangthai)
        {


            if (HttpContext.Session.GetString("trangthai") == null)
            {
                if (trangthai > 3)
                {
                    //step1: create array to holder header labels
                    string[] col_names = new string[]{
                "Mã Đơn Hàng",
                "Ngày Đặt Hàng",
                "Tên Khách Hàng",
                "Số Lượng",
                "Tổng Tiền",
                "Trạng Thái",
                "Hình Thức Thanh Toán"
            };

                    //step2: create result byte array
                    byte[] result;

                    //step3: create a new package using memory safe structure
                    using (var package = new ExcelPackage())
                    {
                        //step4: create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add("Tất cả đơn hàng");

                        //step5: fill in header row
                        //worksheet.Cells[row,col].  {Style, Value}
                        for (int i = 0; i < col_names.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                            worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                                                                              //border the cell
                            worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            //set background color for each sell
                            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        }


                        int row = 2;
                        //step6: loop through query result and fill in cells
                        foreach (var item in _context.Orders.ToList())
                        {
                            for (int col = 1; col <= 7; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                //worksheet.Cells[row, col].Style.Font.Bold = true;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }
                            var status = "Chưa Xử Lý";
                            if (item.Status == "1")
                            {
                                status = "Đang Giao Hàng";
                            }
                            else if (item.Status == "2")
                            {
                                status = "Đã Giao Hàng";
                            }
                            else if (item.Status == "3")
                            {
                                status = "Đã hủy";
                            }
                            //set row,column data
                            worksheet.Cells[row, 1].Value = item.Id;
                            worksheet.Cells[row, 2].Value = item.DateCreate.ToString();
                            worksheet.Cells[row, 3].Value = item.ShipName;
                            worksheet.Cells[row, 4].Value = item.TotalQuantity;
                            worksheet.Cells[row, 5].Value = item.TotalPrice;
                            worksheet.Cells[row, 6].Value = status;
                            worksheet.Cells[row, 7].Value = item.Pay;


                            //toggle background color
                            //even row with ribbon style
                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= 7; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 255, 0));

                                }

                            }
                            row++;
                        }
                        //step7: auto fit columns
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //step8: convert the package as byte array
                        result = package.GetAsByteArray();
                    }//end using

                    //step9: return byte array as a file
                    return File(result, "application/vnd.ms-excel", "TrangThaiDonHang.xls");
                }
                else
                {
                    string tensheet = "Chưa Xử Lý";
                    if (trangthai == 1)
                    {
                        tensheet = "Đang Giao Hàng";
                    }
                    else if (trangthai == 2)
                    {
                        tensheet = "Đã Giao Hàng";
                    }
                    else if (trangthai == 3)
                    {
                        tensheet = "Đã hủy";
                    }
                    //step1: create array to holder header labels
                    string[] col_names = new string[]{
                "Mã Đơn Hàng",
                "Ngày Đặt Hàng",
                "Tên Khách Hàng",
                "Số Lượng",
                "Tổng Tiền",
                "Trạng Thái",
                "Hình Thức Thanh Toán"
            };

                    //step2: create result byte array
                    byte[] result;

                    //step3: create a new package using memory safe structure
                    using (var package = new ExcelPackage())
                    {
                        //step4: create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add(tensheet);

                        //step5: fill in header row
                        //worksheet.Cells[row,col].  {Style, Value}
                        for (int i = 0; i < col_names.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                            worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                                                                              //border the cell
                            worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            //set background color for each sell
                            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        }


                        int row = 2;
                        //step6: loop through query result and fill in cells
                        foreach (var item in _context.Orders.Where(p => Int32.Parse(p.Status) == trangthai).ToList())
                        {
                            for (int col = 1; col <= 7; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                //worksheet.Cells[row, col].Style.Font.Bold = true;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }
                            var status = "Chưa Xử Lý";
                            if (item.Status == "1")
                            {
                                status = "Đang Giao Hàng";
                            }
                            else if (item.Status == "2")
                            {
                                status = "Đã Giao Hàng";
                            }
                            else if (item.Status == "3")
                            {
                                status = "Đã hủy";
                            }
                            //set row,column data
                            worksheet.Cells[row, 1].Value = item.Id;
                            worksheet.Cells[row, 2].Value = item.DateCreate.ToString();
                            worksheet.Cells[row, 3].Value = item.ShipName;
                            worksheet.Cells[row, 4].Value = item.TotalQuantity;
                            worksheet.Cells[row, 5].Value = item.TotalPrice;
                            worksheet.Cells[row, 6].Value = status;
                            worksheet.Cells[row, 7].Value = item.Pay;


                            //toggle background color
                            //even row with ribbon style
                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= 7; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 255, 0));

                                }

                            }
                            row++;
                        }
                        //step7: auto fit columns
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //step8: convert the package as byte array
                        result = package.GetAsByteArray();
                    }//end using

                    //step9: return byte array as a file
                    return File(result, "application/vnd.ms-excel", "TrangThaiDonHang.xls");
                }
            }
            else
            {
                DateTime tu = DateTime.Parse(HttpContext.Session.GetString("tu").ToString());
                DateTime den = DateTime.Parse(HttpContext.Session.GetString("den").ToString());
                if (trangthai > 3)
                {
                    //step1: create array to holder header labels
                    string[] col_names = new string[]{
                "Mã Đơn Hàng",
                "Ngày Đặt Hàng",
                "Tên Khách Hàng",
                "Số Lượng",
                "Tổng Tiền",
                "Trạng Thái",
                "Hình Thức Thanh Toán"
            };

                    //step2: create result byte array
                    byte[] result;

                    //step3: create a new package using memory safe structure
                    using (var package = new ExcelPackage())
                    {
                        //step4: create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add("Tất cả đơn hàng");

                        //step5: fill in header row
                        //worksheet.Cells[row,col].  {Style, Value}
                        for (int i = 0; i < col_names.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                            worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                                                                              //border the cell
                            worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            //set background color for each sell
                            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        }


                        int row = 2;
                        //step6: loop through query result and fill in cells
                        foreach (var item in _context.Orders.Where(p => p.DateCreate >= tu && p.DateCreate <= den).ToList())
                        {
                            for (int col = 1; col <= 7; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                //worksheet.Cells[row, col].Style.Font.Bold = true;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }
                            var status = "Chưa Xử Lý";
                            if (item.Status == "1")
                            {
                                status = "Đang Giao Hàng";
                            }
                            else if (item.Status == "2")
                            {
                                status = "Đã Giao Hàng";
                            }
                            else if (item.Status == "3")
                            {
                                status = "Đã hủy";
                            }
                            //set row,column data
                            worksheet.Cells[row, 1].Value = item.Id;
                            worksheet.Cells[row, 2].Value = item.DateCreate.ToString();
                            worksheet.Cells[row, 3].Value = item.ShipName;
                            worksheet.Cells[row, 4].Value = item.TotalQuantity;
                            worksheet.Cells[row, 5].Value = item.TotalPrice;
                            worksheet.Cells[row, 6].Value = status;
                            worksheet.Cells[row, 7].Value = item.Pay;


                            //toggle background color
                            //even row with ribbon style
                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= 7; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 255, 0));

                                }

                            }
                            row++;
                        }
                        //step7: auto fit columns
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //step8: convert the package as byte array
                        result = package.GetAsByteArray();
                    }//end using

                    //step9: return byte array as a file
                    return File(result, "application/vnd.ms-excel", "TrangThaiDonHang.xls");
                }
                else
                {
                    string tensheet = "Chưa Xử Lý";
                    if (trangthai == 1)
                    {
                        tensheet = "Đang Giao Hàng";
                    }
                    else if (trangthai == 2)
                    {
                        tensheet = "Đã Giao Hàng";
                    }
                    else if (trangthai == 3)
                    {
                        tensheet = "Đã hủy";
                    }

                    int check = 0;
                    if (HttpContext.Session.GetString("trangthai") == "dang-giao-hang")
                    {
                        check = 1;
                    }
                    else if (HttpContext.Session.GetString("trangthai") == "da-giao-hang")
                    {
                        check = 2;
                    }
                    //step1: create array to holder header labels
                    string[] col_names = new string[]{
                "Mã Đơn Hàng",
                "Ngày Đặt Hàng",
                "Tên Khách Hàng",
                "Số Lượng",
                "Tổng Tiền",
                "Trạng Thái",
                "Hình Thức Thanh Toán"
            };

                    //step2: create result byte array
                    byte[] result;

                    //step3: create a new package using memory safe structure
                    using (var package = new ExcelPackage())
                    {
                        //step4: create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add(tensheet);

                        //step5: fill in header row
                        //worksheet.Cells[row,col].  {Style, Value}
                        for (int i = 0; i < col_names.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                            worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                                                                              //border the cell
                            worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            //set background color for each sell
                            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        }


                        int row = 2;
                        //step6: loop through query result and fill in cells
                        foreach (var item in _context.Orders.Where(p => Int32.Parse(p.Status) == check && p.DateCreate >= tu && p.DateCreate <= den).ToList())
                        {
                            for (int col = 1; col <= 7; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                //worksheet.Cells[row, col].Style.Font.Bold = true;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }
                            var status = "Chưa Xử Lý";
                            if (item.Status == "1")
                            {
                                status = "Đang Giao Hàng";
                            }
                            else if (item.Status == "2")
                            {
                                status = "Đã Giao Hàng";
                            }
                            else if (item.Status == "3")
                            {
                                status = "Đã hủy";
                            }
                            //set row,column data
                            worksheet.Cells[row, 1].Value = item.Id;
                            worksheet.Cells[row, 2].Value = item.DateCreate.ToString();
                            worksheet.Cells[row, 3].Value = item.ShipName;
                            worksheet.Cells[row, 4].Value = item.TotalQuantity;
                            worksheet.Cells[row, 5].Value = item.TotalPrice;
                            worksheet.Cells[row, 6].Value = status;
                            worksheet.Cells[row, 7].Value = item.Pay;


                            //toggle background color
                            //even row with ribbon style
                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= 7; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 255, 0));

                                }

                            }
                            row++;
                        }
                        //step7: auto fit columns
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //step8: convert the package as byte array
                        result = package.GetAsByteArray();
                    }//end using

                    //step9: return byte array as a file
                    return File(result, "application/vnd.ms-excel", "TrangThaiDonHang.xls");
                }
            }


        }//end fun

        // thay đổi trạng thái đơn hàng
        public IActionResult Change(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    var ord = _context.Orders.Where(p => p.Id == id).SingleOrDefault();
                    if (ord.Status == "0")
                    {
                        ord.Status = "1";
                        Tracking New_Tracking = new Tracking
                        {
                            Cart_Id = ord.Id,
                            TongTien = ord.TotalPrice.ToString(),
                            SDT = ord.ShipPhone.ToString(),
                            GhiChu = ord.GhiChu,
                            Time = DateTime.Now,
                            HoTen = ord.ShipName,
                            ThongTin = "Đã xử lý. Đang giao hàng",
                            DiaChi = ord.Address
                        };
                        _context.Trackings.Add(New_Tracking);
                        _context.SaveChanges();
                    }
                    else if (ord.Status == "1")
                    {
                        ord.Status = "2";
                        Tracking New_Tracking = new Tracking
                        {
                            Cart_Id = ord.Id,
                            TongTien = ord.TotalPrice.ToString(),
                            SDT = ord.ShipPhone.ToString(),
                            GhiChu = ord.GhiChu,
                            Time = DateTime.Now,
                            HoTen = ord.ShipName,
                            ThongTin = "Đã giao hàng",
                            DiaChi = ord.Address
                        };
                        _context.Trackings.Add(New_Tracking);
                        _context.SaveChanges();
                    }
                    _context.Orders.Update(ord);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult XemDonHang(int Id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                DonHangChiTiet donHangChiTiet = new DonHangChiTiet(_context);

                ViewBag.Result = donHangChiTiet.GetDonHangChiTiet(Id);
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }


        public IActionResult SuaDonHang(int Id)
        {
            if (HttpContext.Session.GetInt32("admin") >= 0)
            {

                ViewBag.Result = _context.Orders.Where(p => p.Id == Id).SingleOrDefault();
                HttpContext.Session.SetInt32("idsua", Id);
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult SeachOrder(string trangthai, DateTime tu, DateTime den)
        {
            // excel 

            HttpContext.Session.SetString("trangthai", trangthai);
            HttpContext.Session.SetString("tu", tu.ToString());
            HttpContext.Session.SetString("den", den.ToString());

            ViewBag.Tatca = _context.Orders.ToList();
            ViewBag.xl = _context.Orders.Where(p => p.Status == "0").ToList();
            ViewBag.danggiao = _context.Orders.Where(p => p.Status == "1").ToList();
            ViewBag.hoanthanh = _context.Orders.Where(p => p.Status == "2").ToList();
            ViewBag.dahuy = _context.Orders.Where(p => p.Status == "3").ToList();
            if (trangthai == "tat-ca-don-hang")
            {
                ViewBag.DonHang = _context.Orders.Where(p => p.DateCreate >= DateTime.Parse(tu.ToString("MM/dd/yyyy 00:00:00")) && p.DateCreate <= DateTime.Parse(den.ToString("MM/dd/yyyy 23:59:59"))).ToList();

            }
            else
            {
                int TD = 0;
                if (trangthai == "chua-xu-ly")
                {
                    TD = 0;

                }
                else if (trangthai == "dang-giao-hang")
                {
                    TD = 1;

                }
                else if (trangthai == "da-huy")
                {
                    TD = 3;

                }
                else
                {
                    TD = 2;

                }
                ViewBag.DonHang = _context.Orders.Where(p => p.Status == TD.ToString() && p.DateCreate >= DateTime.Parse(tu.ToString("MM/dd/yyyy 00:00:00")) && p.DateCreate <= DateTime.Parse(den.ToString("MM/dd/yyyy 23:59:59"))).ToList();
            }



            return PartialView("_SeachOrder", ViewBag.DonHang);
        }

        [HttpPost]
        public IActionResult Save(OrderViewModels order)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                var _order = _context.Orders.Where(p => p.Id == HttpContext.Session.GetInt32("idsua")).SingleOrDefault();
                _order.ShipName = order.ShipName;
                _order.ShipPhone = order.ShipPhone;
                _order.Address = order.Address;
                _context.Orders.Update(_order);
                _context.SaveChanges();
                return Content("ok");
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult XemChiTietDonHang(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                var donhang = _context.Orders.Where(p => p.Id == id).SingleOrDefault();
                ViewBag.Ten = donhang.ShipName;
                ViewBag.Sdt = donhang.ShipPhone;
                ViewBag.Diachi = donhang.Address;
                ViewBag.Ma = donhang.Id;
                ViewBag.Ngay = donhang.DateCreate;
                DonHangChiTiet donHangChiTiet = new DonHangChiTiet(_context);

                ViewBag.Result = donHangChiTiet.GetDonHangChiTiet(donhang.Id);
                ViewBag.Total = donHangChiTiet.GetDonHangChiTiet(donhang.Id).Sum(p => p.Gia * p.SoLuong);

                //   return new ViewAsPdf();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult XoaDonHangById(int id)
        {


            try
            {
                Order ord = _context.Orders.Where(p => p.Id == id).FirstOrDefault();
                //List<int> mangid = _context.CartDetails.Where(p => p.Cart_Id == delete.Id).Select(p => p.Id).ToList();

                //foreach (var item in mangid)
                //{

                //    CartDetail cartdetail = _context.CartDetails.Where(p => p.Id == item).SingleOrDefault();
                //    _context.CartDetails.Remove(cartdetail);
                //    _context.SaveChanges();

                //}

                //_context.Orders.Remove(delete);
                //_context.SaveChanges();
                ord.Status = "3";
                Tracking New_Tracking = new Tracking
                {
                    Cart_Id = ord.Id,
                    TongTien = ord.TotalPrice.ToString(),
                    SDT = ord.ShipPhone.ToString(),
                    GhiChu = ord.GhiChu,
                    Time = DateTime.Now,
                    HoTen = ord.ShipName,
                    ThongTin = "Đã hủy đơn",
                    DiaChi = ord.Address
                };
                _context.Trackings.Add(New_Tracking);
                _context.SaveChanges();
                _context.Orders.Update(ord);
                _context.SaveChanges();
                return Content("1");
            }
            catch
            {
                return Content("0");
            }
        
           
        }


        public IActionResult NhomSanPham()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                ViewBag.Result = _context.Categorys.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult ProductList()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                ProductAdminViewModel productAdminViewModel = new ProductAdminViewModel(_context);
                ViewBag.Result = productAdminViewModel.GetProductAdminViewModel();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult BlogItemInput()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.BlogCategory = _context.Blogs.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }

        }

        public IActionResult BlogItemEdit(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.BlogCategory = _context.Blogs.ToList();
                ViewBag.BlogItem = _context.BlogItems.Where(p => p.Id == id).SingleOrDefault();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }

        }

        [HttpPost]
        public IActionResult BlogItemEditById(BlogItemViewModel blog)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                //Product data = vm.Product;
                BlogItem edit = _context.BlogItems.Where(p => p.Id == blog.Id).FirstOrDefault();


                // Tải hình ảnh sản phẩm lên thư mục wwwroot/imgVETA
                if (blog.file != null)
                {

                    edit.Thumbnail = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    string uniqueName = edit.Thumbnail; // Tạo tên hình ảnh theo chuỗi ngày tháng lúc đăng ảnh
                    string newpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA"); // Trỏ đường dẫn đến thư mục wwwroot/uploads
                    newpath = Path.Combine(newpath, uniqueName); // Trỏ đường dẫn đến tên hình ảnh
                    newpath = newpath + Path.GetExtension(blog.file.FileName); // Gắn đuôi (loại file) cho hình
                    blog.file.CopyTo(new FileStream(newpath, FileMode.Create)); // Copy hình từ nguồn sang wwwroot/uploads
                    edit.Thumbnail += Path.GetExtension(blog.file.FileName);
                }
                edit.Title = blog.Title;
                edit.Content = blog.Content;
                edit.Category_BlogId = blog.Category_Id;
                edit.URL = blog.Title.ToUrlFriendly();
                _context.BlogItems.Update(edit);
                _context.SaveChanges();
                return Redirect("~/admin/blogitemlist");
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        [HttpPost]
        public IActionResult AddBlogItem(BlogItemViewModel blog)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                BlogItem new_blogitem = new BlogItem();

                new_blogitem.Thumbnail = "#";
                // Tải hình ảnh sản phẩm lên thư mục wwwroot/uploads
                if (blog.file != null)
                {
                    new_blogitem.Thumbnail = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                    string uniqueName = new_blogitem.Thumbnail; // Tạo tên hình ảnh theo chuỗi ngày tháng lúc đăng ảnh
                    string newpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA"); // Trỏ đường dẫn đến thư mục wwwroot/uploads
                    newpath = Path.Combine(newpath, uniqueName); // Trỏ đường dẫn đến tên hình ảnh
                    newpath = newpath + Path.GetExtension(blog.file.FileName); // Gắn đuôi (loại file) cho hình
                    blog.file.CopyTo(new FileStream(newpath, FileMode.Create)); // Copy hình từ nguồn sang wwwroot/uploads
                    new_blogitem.Thumbnail += Path.GetExtension(blog.file.FileName);
                }
                new_blogitem.Title = blog.Title;
                new_blogitem.Content = blog.Content;
                new_blogitem.Category_BlogId = blog.Category_Id;
                new_blogitem.DateCreate = DateTime.Now;
                new_blogitem.URL = blog.Title.ToUrlFriendly();
                _context.BlogItems.Add(new_blogitem);
                _context.SaveChanges();
                return Redirect("~/admin/blogitemlist");
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult BlogItemDelete(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {


                try
                {
                    BlogItem delete = _context.BlogItems.Where(p => p.Id == id).FirstOrDefault();
                    _context.Remove(delete);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult BlogItemList()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                BlogItemDetailViewModel blogItem = new BlogItemDetailViewModel(_context);
                ViewBag.Result = blogItem.getAllBlogItem("all");
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult BlogCategoryList()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.Result = _context.Blogs.ToList();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
            return View();
        }

        public IActionResult BlogCategoryInput()
        {
            return View();
        }

        public IActionResult DeleteBlogCategoryById(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    var Blog = _context.Blogs.Where(p => p.Id == id).SingleOrDefault();
                    _context.Blogs.Remove(Blog);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult BlogCategoryEdit(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                ViewBag.Result = _context.Blogs.Where(p => p.Id == id).SingleOrDefault();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult EditBlogCategory(BlogViewModels blog)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                try
                {
                    var BlogCate = _context.Blogs.Where(p => p.Id == blog.Id).SingleOrDefault();
                    BlogCate.Name = blog.Name;
                    BlogCate.DateModify = DateTime.Now;
                    BlogCate.URL = blog.Name.ToUrlFriendly();
                    // _context.Categorys.Update(Category);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult AddBlogCategory(BlogViewModels blog)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    var new_blog = new Blog();
                    new_blog.Name = blog.Name;
                    new_blog.DateCreate = DateTime.Now;
                    new_blog.DateModify = DateTime.Now;
                    new_blog.URL = blog.Name.ToUrlFriendly();
                    _context.Blogs.Add(new_blog);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
            
        }

        // category
        public IActionResult CategoryInput()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult AddCategory(CategoryViewmodel cate)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    var Category = new Category();
                    Category.Name = cate.Name;
                    Category.DateCreate = DateTime.Now;
                    Category.DateModify = DateTime.Now;
                    Category.URL = cate.Name.ToUrlFriendly();
                    _context.Categorys.Add(Category);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult CategoryInputEdit(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                ViewBag.Result = _context.Categorys.Where(p => p.Id == id).SingleOrDefault();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult EditCategory(CategoryViewmodel cate)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                try
                {
                    var Category = _context.Categorys.Where(p => p.Id == cate.Id).SingleOrDefault();
                    Category.Name = cate.Name;
                    Category.DateModify = DateTime.Now;
                    Category.URL = cate.Name.ToUrlFriendly();
                   // _context.Categorys.Update(Category);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult CategoryDeleteById(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    var pro = _context.Products.Where(p => p.Category_Id == id).ToList();
                    foreach (var item in pro)
                    {
                        // Xóa ảnh
                        //string old_image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA", item.Thumbnail);
                        //if (System.IO.File.Exists(old_image))
                        //{
                        //    System.IO.File.Delete(old_image);
                        //}

                        // Xóa các bình luận
                        List<Comment> comments = _context.Comments.Where(c => c.Product_Id == item.Id).ToList();
                        if (comments != null)
                        {
                            foreach (var comment in comments)
                            {
                                _context.Comments.Remove(comment);
                                _context.SaveChanges();
                            }
                        }
                        _context.Products.Remove(item);
                        _context.SaveChanges();
                    }
                    var Category = _context.Categorys.Where(p => p.Id == id).SingleOrDefault();

                    _context.Categorys.Remove(Category);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult ExportCategory()
        {
            //step1: create array to holder header labels
            string[] col_names = new string[]{
                "Mã ",
                "Tên Nhóm",
                "Ngày Tạo",
                "Ngày Sửa Cuối",

            };

            //step2: create result byte array
            byte[] result;

            //step3: create a new package using memory safe structure
            using (var package = new ExcelPackage())
            {
                //step4: create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Tất cả Nhóm Loại");

                //step5: fill in header row
                //worksheet.Cells[row,col].  {Style, Value}
                for (int i = 0; i < col_names.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                    worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                                                                      //border the cell
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //set background color for each sell
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                }


                int row = 2;
                //step6: loop through query result and fill in cells
                foreach (var item in _context.Categorys.ToList())
                {
                    for (int col = 1; col <= 4; col++)
                    {
                        worksheet.Cells[row, col].Style.Font.Size = 12;
                        //worksheet.Cells[row, col].Style.Font.Bold = true;
                        worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    }

                    //set row,column data
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.Name;
                    worksheet.Cells[row, 3].Value = item.DateCreate.ToString();
                    worksheet.Cells[row, 4].Value = item.DateModify.ToString();



                    //toggle background color
                    //even row with ribbon style
                    if (row % 2 == 0)
                    {
                        for (int col = 1; col <= 4; col++)
                        {
                            worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 255, 0));

                        }

                    }
                    row++;
                }
                //step7: auto fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //step8: convert the package as byte array
                result = package.GetAsByteArray();
            }//end using

            //step9: return byte array as a file
            return File(result, "application/vnd.ms-excel", "Catogory.xls");
        }

        // product

        public IActionResult ProductInput()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.Category = _context.Categorys.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }

        }


        [HttpPost]
        public IActionResult AddProduct(ProductViewModels product)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                Product New_product = new Product();

                New_product.Thumbnail = "#";
                // Tải hình ảnh sản phẩm lên thư mục wwwroot/uploads
                if (product.file != null)
                {
                    New_product.Thumbnail = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                    string uniqueName = New_product.Thumbnail; // Tạo tên hình ảnh theo chuỗi ngày tháng lúc đăng ảnh
                    string newpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA"); // Trỏ đường dẫn đến thư mục wwwroot/uploads
                    newpath = Path.Combine(newpath, uniqueName); // Trỏ đường dẫn đến tên hình ảnh
                    newpath = newpath + Path.GetExtension(product.file.FileName); // Gắn đuôi (loại file) cho hình
                    product.file.CopyTo(new FileStream(newpath, FileMode.Create)); // Copy hình từ nguồn sang wwwroot/uploads
                    New_product.Thumbnail += Path.GetExtension(product.file.FileName);
                }
                New_product.Name = product.Name;
                New_product.Description = product.Description;
                New_product.Name = product.Name;
                New_product.Price = product.Price;
                New_product.SalePrice = product.SalePrice;
                New_product.Category_Id = product.Category_Id;
                New_product.URL = product.Name.ToUrlFriendly();
                New_product.Status = 1;
                _context.Products.Add(New_product);
                _context.SaveChanges();
                return Redirect("~/admin/productlist");
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult ProductEdit(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                ViewBag.Category = _context.Categorys.ToList();
                ViewBag.ProductEdit = _context.Products.Where(p => p.Id == id).SingleOrDefault();
                return View(_context.Products.Where(p => p.Id == id).SingleOrDefault());
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        [HttpPost]
        public IActionResult ProductEditById(ProductViewModels product)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                //Product data = vm.Product;
                Product productedit = _context.Products.Where(p => p.Id == product.Id).FirstOrDefault();


                // Tải hình ảnh sản phẩm lên thư mục wwwroot/imgVETA
                if (product.file != null)
                {
                    
                    productedit.Thumbnail = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    string uniqueName = productedit.Thumbnail; // Tạo tên hình ảnh theo chuỗi ngày tháng lúc đăng ảnh
                    string newpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA"); // Trỏ đường dẫn đến thư mục wwwroot/uploads
                    newpath = Path.Combine(newpath, uniqueName); // Trỏ đường dẫn đến tên hình ảnh
                    newpath = newpath + Path.GetExtension(product.file.FileName); // Gắn đuôi (loại file) cho hình
                    product.file.CopyTo(new FileStream(newpath, FileMode.Create)); // Copy hình từ nguồn sang wwwroot/uploads
                    productedit.Thumbnail += Path.GetExtension(product.file.FileName);
                }
                productedit.Name = product.Name;
                productedit.Description = product.Description;
                productedit.Name = product.Name;
                productedit.Price = product.Price;
                productedit.SalePrice = product.SalePrice;
                productedit.Category_Id = product.Category_Id;
                productedit.URL = product.Name.ToUrlFriendly();
                _context.Products.Update(productedit);
                _context.SaveChanges();
                return Redirect("~/admin/productlist");
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }


        public IActionResult ProductDelete(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {


                try
                {
                    Product delete = _context.Products.Where(p => p.Id == id).FirstOrDefault();

                    RemoveProduct(delete);
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public void RemoveProduct(Product product)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                // Xóa ảnh
                //string old_image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA", product.Thumbnail);
                //if (System.IO.File.Exists(old_image))
                //{
                //    System.IO.File.Delete(old_image);
                //}

                // Xóa các bình luận
                List<Comment> comments = _context.Comments.Where(c => c.Product_Id == product.Id).ToList();
                if (comments != null)
                {
                    foreach (var comment in comments)
                    {
                        _context.Comments.Remove(comment);
                        _context.SaveChanges();
                    }
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
            }

        }

        public void RemoveImage(Product product)
        {
            string url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgVETA", product.Thumbnail);
            if (System.IO.File.Exists(url))
            {
                System.IO.File.Delete(url);
            }

        }

        //login
        public IActionResult Login(string message = "")
        {
            this.ViewBag.Error = message;
            return View();
        }

        [HttpPost]
        public IActionResult CheckLogin(LoginViewModels admin)
        {
            if (admin.Username == null || admin.Password == null)
            {
                this.ViewBag.Error = "Vui lòng nhập tài khoản và mật khẩu!";
                return (RedirectToAction("Login", new { message = this.ViewBag.Error }));
            }
            else
            {
                var UserLogin = _context.Users.Where(p => p.UserName == admin.Username.Trim() && p.Password == MD5.GetMD5(admin.Password.Trim())).SingleOrDefault();

                if (UserLogin != null)
                {
                    if (Int32.Parse(UserLogin.Permission.ToString()) >= 1)
                    {

                        HttpContext.Session.SetInt32("admin", Convert.ToInt32(UserLogin.Permission));
                        HttpContext.Session.SetInt32("idadmin", Convert.ToInt32(UserLogin.Id));
                        HttpContext.Session.SetString("Time", DateTime.Now.ToString());
                        HttpContext.Session.SetString("Name", UserLogin.UserName.ToString());

                        return RedirectToAction("index", "admin");

                    }
                    else
                    {
                        this.ViewBag.Error = "Bạn không phải nhân viên or admin!";
                        return (RedirectToAction("Login", new { message = this.ViewBag.Error }));
                    }

                }
                else
                {

                    this.ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác !";
                    return (RedirectToAction("Login", new { message = this.ViewBag.Error }));
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin");
            HttpContext.Session.Remove("idadmin");
            HttpContext.Session.Remove("Time");
            HttpContext.Session.Remove("Name");
            return RedirectToAction("login", "admin");
        }

        public IActionResult ProfileAdmin(int id)
        {
            if (HttpContext.Session.GetInt32("editadmin") == 1)
            {
                this.ViewBag.tkadmin = "Sửa Thành Công";
            }
            else if (HttpContext.Session.GetInt32("editadmin") == 0)
            {
                this.ViewBag.tkadmin = "Sửa Thất Bại";
            }
            else
            {
                this.ViewBag.tkadmin = "";
            }
            return View(_context.Users.Where(p => p.Id == id).SingleOrDefault());
        }

        public IActionResult SuaProfile(UserViewModels user)
        {
            string url = "~/admin/profileadmin?id=" + user.Id;
            try
            {
                User edituser = _context.Users.Where(p => p.Id == user.Id).SingleOrDefault();

                edituser.UserName = user.UserName;
                edituser.Password = MD5.GetMD5(user.passwordnew.Trim());
                edituser.Email = user.Email;
                edituser.NameFirst = user.NameFirst;
                edituser.NameLast = user.NameLast;
                edituser.Address = user.Address;
                edituser.PhoneNumber = user.PhoneNumber;

                _context.Users.Update(edituser);
                _context.SaveChanges();


                HttpContext.Session.SetInt32("editadmin", 1);
                return Redirect(url);
            }
            catch
            {
                HttpContext.Session.SetInt32("editadmin", 0);
                return Redirect(url);
            }
        }
        //CRUD nhân vien

        public IActionResult DanhSachNhanVien()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.Result = _context.Users.Where(p => p.Permission != 0).ToList();

                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }

        }

        public IActionResult ThemNhanVien()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult Addnhanvien(UserViewModels user)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    User new_user = AdminThemNhanVien.themNhanVien(user);
                    _context.Users.Add(new_user);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult SuaNhanVienHoanQuanLy(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.Reuslt = _context.Users.Where(p => p.Id == id).SingleOrDefault();

                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult SuaNhanVienHoacQuanLy(UserViewModels user)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    User edituser = _context.Users.Where(p => p.Id == user.Id).SingleOrDefault();
                    edituser.UserName = user.UserName;
                    edituser.Password = MD5.GetMD5(user.Password.Trim());
                    edituser.Email = user.Email;
                    edituser.NameFirst = user.NameFirst;
                    edituser.NameLast = user.NameLast;
                    edituser.Address = user.Address;
                    edituser.PhoneNumber = user.PhoneNumber;
                    edituser.Permission = user.status;
                    _context.Users.Update(edituser);
                    _context.SaveChanges();

                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }

            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult XoaNhanVienHoacQuanLy(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    User user = _context.Users.Where(p => p.Id == id).SingleOrDefault();
                    List<int> mangidorder = _context.Orders.Where(p => p.User_Id == user.Id).Select(p => p.Id).ToList();

                    foreach (var item in mangidorder)
                    {
                        Order order = _context.Orders.Where(p => p.Id == item).SingleOrDefault();
                        _context.Orders.Remove(order);
                        _context.SaveChanges();
                    }
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }

            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        //CRUD khách hàng
        public IActionResult DanhSachKhachHang()

        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                ViewBag.Result = _context.Users.Where(p => p.Permission == 0).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult ThemKhachHang()
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult AddKhachHang(UserViewModels user)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    User new_user = new User
                    {
                        UserName = user.UserName,
                        Password = MD5.GetMD5(user.Password.Trim()),
                        Email = user.Email,
                        NameFirst = user.NameFirst,
                        NameLast = user.NameLast,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Permission = 0,
                        Active = 0,
                        Confirm = 1

                    };
                    _context.Users.Add(new_user);
                    _context.SaveChanges();
                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult SuaKhachHang(int id)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {

                ViewBag.Reuslt = _context.Users.Where(p => p.Id == id).SingleOrDefault();

                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }
        public IActionResult EditKhachHang(UserViewModels user)
        {
            if (HttpContext.Session.GetInt32("admin") > 0)
            {
                try
                {
                    User edituser = _context.Users.Where(p => p.Id == user.Id).SingleOrDefault();
                    edituser.UserName = user.UserName;
                    edituser.Password = MD5.GetMD5(user.Password.Trim());
                    edituser.Email = user.Email;
                    edituser.NameFirst = user.NameFirst;
                    edituser.NameLast = user.NameLast;
                    edituser.Address = user.Address;
                    edituser.PhoneNumber = user.PhoneNumber;
                    edituser.Permission = user.status;
                    _context.Users.Update(edituser);
                    _context.SaveChanges();

                    return Content("1");
                }
                catch
                {
                    return Content("0");
                }
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public IActionResult ResetPassWord(int id)
        {
            try
            {
                User user = _context.Users.Where(p => p.Id == id).SingleOrDefault();
                user.Password = MD5.GetMD5("123456");
                _context.Users.Update(user);
                _context.SaveChanges();
                return Content("1");
            }
            catch
            {
                return Content("0");
            }

        }

        public IActionResult GuiTinNhanHangLoatFB()
        {
            return View();
        }

        public IActionResult TrangThaiSanPham(int id)
        {
            try
            {
                var pro = _context.Products.Where(p => p.Id == id).SingleOrDefault();
                if (pro.Status == 1)
                {
                    pro.Status = 0;
                }
                else
                {
                    pro.Status = 1;
                }
                _context.Products.Update(pro);
                _context.SaveChanges();
                ProductAdminViewModel productAdminViewModel = new ProductAdminViewModel(_context);
                ViewBag.Result = productAdminViewModel.GetProductAdminViewModel();
                return PartialView("_trangthaisanpham", ViewBag.Result);
            }
            catch
            {
                return RedirectToAction("productlist", "admin");
            }
        }


        public IActionResult ExportKhachHang(int trangthai)
        {


            
                
                    //step1: create array to holder header labels
                    string[] col_names = new string[]{
                "Họ Tên",
                "Email",
                "SĐT"
            };

                    //step2: create result byte array
                    byte[] result;

                    //step3: create a new package using memory safe structure
                    using (var package = new ExcelPackage())
                    {
                        //step4: create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add("Danh sách khách hàng");

                        //step5: fill in header row
                        //worksheet.Cells[row,col].  {Style, Value}
                        for (int i = 0; i < col_names.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Style.Font.Size = 14;  //font
                            worksheet.Cells[1, i + 1].Value = col_names[i];  //value
                            worksheet.Cells[1, i + 1].Style.Font.Bold = true; //bold
                                                                              //border the cell
                            worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            //set background color for each sell
                            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        }


                        int row = 2;
                        //step6: loop through query result and fill in cells
                        foreach (var item in _context.Users.Where(p => p.Permission == 0).ToList())
                        {
                            for (int col = 1; col <= 3; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                //worksheet.Cells[row, col].Style.Font.Bold = true;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }
                           
                            //set row,column data
                            
                            worksheet.Cells[row, 1].Value = item.NameFirst+ " "+item.NameLast;
                            worksheet.Cells[row, 2].Value = item.Email;
                            worksheet.Cells[row, 3].Value = item.PhoneNumber;
                           


                            //toggle background color
                            //even row with ribbon style
                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= 3; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 255, 0));

                                }

                            }
                            row++;
                        }
                        //step7: auto fit columns
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        //step8: convert the package as byte array
                        result = package.GetAsByteArray();
                    }//end using

                    //step9: return byte array as a file
                    return File(result, "application/vnd.ms-excel", "DanhSachKhanhHang.xls");
                
              
            
            
          


        }//end fun
    }


}