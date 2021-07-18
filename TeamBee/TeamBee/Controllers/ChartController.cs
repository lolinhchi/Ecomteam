using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamBee.Connect_Data;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Controllers
{
    public class ChartController : Controller
    {
        private readonly TeamBeeDbContext _context;

        //
        public ChartController(TeamBeeDbContext context)
        {
            _context = context;
        }
        public ActionResult ColumnChart()
        {

            List<DataPoint> dataPoints = new List<DataPoint>();


            HttpContext.Session.SetString("piechart", JsonConvert.SerializeObject(ResultPieChart(1)));

            ViewBag.DataPoints = HttpContext.Session.GetString("piechart");

            ViewBag.Category = _context.Categorys.ToList();
            ViewBag.DataPoints1 = HttpContext.Session.GetString("linechart");
            HttpContext.Session.Remove("linechart");
            return View();
        }
        public ActionResult BuildChart(int option = 1)
        {

            return Json(Result(option));
        }

        public ActionResult BuildLineChart(int option = 1)
        {

            return Json(ResultLineChart(option));
        }
        public ActionResult BuildPieChart(int option = 1)
        {

            return Json(ResultPieChart(option));
        }
        public List<OrderChartModel> Result(int option)
        {
            List<OrderChartModel> res = new List<OrderChartModel>();
            List<DataPoint> dataPoints1 = new List<DataPoint>();
            if (option == 1)
            {
                List<Order> orders = _context.Orders.ToList();
                res.Add(new OrderChartModel()
                {
                    stdName = DateTime.Now.Date.ToString("dd/MM/yyyy"),
                    marksObtained = orders.Where(p => p.DateCreate.Date == DateTime.Now.Date) != null ? orders.Where(p => p.DateCreate.Date == DateTime.Now.Date).Sum(p => p.TotalPrice) : 0
                });

                dataPoints1.Add(new DataPoint(DateTime.Now.Date.ToString("dd/MM/yyyy"), orders.Where(p => p.DateCreate.Date == DateTime.Now.Date) != null ? (double)orders.Where(p => p.DateCreate.Date == DateTime.Now.Date).Sum(p => p.TotalPrice) : 0));
            }
            else
            {
                List<Order> orders = _context.Orders.Where(p => (DateTime.Now - p.DateCreate).Days <= option).ToList();
                DateTime minDate = DateTime.Now.AddDays(-option); // để suy nghỉ tí :))
                List<DateTime> dates = new List<DateTime>();
                for (int i = 1; i <= option; i++)
                {
                    dates.Add(minDate);
                    minDate = minDate.AddDays(1);
                }
                foreach (var orderDate in dates)
                {
                    res.Add(new OrderChartModel()
                    {
                        stdName = orderDate.ToString("dd/MM/yyyy"),
                        marksObtained = orders.Where(p => p.DateCreate.Date == orderDate.Date) != null ? orders.Where(p => p.DateCreate.Date == orderDate.Date).Sum(p => p.TotalPrice) : 0
                    });
                    dataPoints1.Add(new DataPoint(orderDate.ToString("dd/MM/yyyy"), orders.Where(p => p.DateCreate.Date == orderDate.Date) != null ? orders.Where(p => p.DateCreate.Date == orderDate.Date).Sum(p => p.TotalPrice) : 0));
                }
            }

            HttpContext.Session.SetString("linechart", JsonConvert.SerializeObject(ResultLineChart(option)));
            ViewBag.DataPoints1 = HttpContext.Session.GetString("linechart");

            return res;
        }
        public List<DataPoint> ResultLineChart(int option)
        {

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            if (option == 1)
            {

                List<Order> orders = _context.Orders.ToList();
                dataPoints1.Add(new DataPoint(DateTime.Now.Date.ToString("dd/MM/yyyy"), orders.Where(p => p.DateCreate.Date == DateTime.Now.Date) != null ? (double)orders.Where(p => p.DateCreate.Date == DateTime.Now.Date).Sum(p => p.TotalPrice) : 0));
            }
            else
            {
                List<Order> orders = _context.Orders.Where(p => (DateTime.Now - p.DateCreate).Days <= option).ToList();
                DateTime minDate = DateTime.Now.AddDays(-option); // để suy nghỉ tí :))
                List<DateTime> dates = new List<DateTime>();
                for (int i = 1; i <= option; i++)
                {
                    dates.Add(minDate);
                    minDate = minDate.AddDays(1);
                }
                foreach (var orderDate in dates)
                {

                    dataPoints1.Add(new DataPoint(orderDate.ToString("dd/MM/yyyy"), orders.Where(p => p.DateCreate.Date == orderDate.Date) != null ? orders.Where(p => p.DateCreate.Date == orderDate.Date).Sum(p => p.TotalPrice) : 0));
                }
            }



            return dataPoints1;
        }

        public List<DataPoint> ResultPieChart(int option)
        {

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            if (option == 1)
            {

                int TongDonHang = _context.Orders.Where(p => p.DateCreate.Date == DateTime.Now.Date).Count();
                int DonXuLy = _context.Orders.Where(p => p.DateCreate.Date == DateTime.Now.Date && int.Parse(p.Status) == 0).Count();
                int DangGiaoHang = _context.Orders.Where(p => p.DateCreate.Date == DateTime.Now.Date && int.Parse(p.Status) == 1).Count();
                int DaGiaHang = _context.Orders.Where(p => p.DateCreate.Date == DateTime.Now.Date && int.Parse(p.Status) == 2).Count();
                dataPoints1.Add(new DataPoint("Chờ Xử Lý", tinhPhanTram(TongDonHang, DonXuLy)));
                dataPoints1.Add(new DataPoint("Đang Giao Hàng", tinhPhanTram(TongDonHang, DangGiaoHang)));
                dataPoints1.Add(new DataPoint("Đã Giao Hàng", tinhPhanTram(TongDonHang, DaGiaHang)));

            }
            else
            {
                List<Order> orders = _context.Orders.Where(p => (DateTime.Now - p.DateCreate).Days <= option).ToList();
                DateTime minDate = DateTime.Now.AddDays(-option); // để suy nghỉ tí :))


                DateTime BatDau = minDate;
                DateTime KetThuc = minDate.AddDays(option);
                int TongDonHang = _context.Orders.Where(p => p.DateCreate.Date >= BatDau.Date && p.DateCreate.Date <= KetThuc.Date).Count();
                int DonXuLy = _context.Orders.Where(p => p.DateCreate.Date >= BatDau.Date && p.DateCreate.Date <= KetThuc.Date && int.Parse(p.Status) == 0).Count();
                int DangGiaoHang = _context.Orders.Where(p => p.DateCreate.Date >= BatDau.Date && p.DateCreate.Date <= KetThuc.Date && int.Parse(p.Status) == 1).Count();
                int DaGiaHang = _context.Orders.Where(p => p.DateCreate.Date >= BatDau.Date && p.DateCreate.Date <= KetThuc.Date && int.Parse(p.Status) == 2).Count();
                dataPoints1.Add(new DataPoint("Chờ Xử Lý", tinhPhanTram(TongDonHang, DonXuLy)));
                dataPoints1.Add(new DataPoint("Đang Giao Hàng", tinhPhanTram(TongDonHang, DangGiaoHang)));
                dataPoints1.Add(new DataPoint("Đã Giao Hàng", tinhPhanTram(TongDonHang, DaGiaHang)));

            }

            HttpContext.Session.SetString("piechart", JsonConvert.SerializeObject(dataPoints1));
            ViewBag.DataPoints = HttpContext.Session.GetString("piechart");
            return dataPoints1;
        }

        public double tinhPhanTram(double tong, double sl)
        {
            return Math.Floor(((sl / tong) * 100) * 100) / 100;
        }
    }
}