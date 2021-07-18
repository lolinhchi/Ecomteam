using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmeticShop.Models;
using Microsoft.AspNetCore.Mvc;
using TeamBee.Connect_Data;
using TeamBee.Helper;
using TeamBee.Models;
using TeamBee.ViewModels;
using Microsoft.AspNetCore.Http;

namespace TeamBee.Controllers
{
    public class NganLuongController : Controller
    {
        private readonly TeamBeeDbContext _context;

        public NganLuongController(TeamBeeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            return View();
        }
        public List<CartItem> GetGioHang
        {
            get
            {
                List<CartItem> myCart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "cart");
                if (myCart == default(List<CartItem>))
                {
                    myCart = new List<CartItem>();
                }
                return myCart;
            }
        }

        public IActionResult ExecutePayment(string buyer_fullname, string buyer_email,
            string buyer_mobile, string option_payment, string bankcode)
        {
            try
            {
                string payment_method = option_payment;
                string str_bankcode = bankcode;


                RequestInfo info = new RequestInfo();
                info.Merchant_id = "48305";
                info.Merchant_password = "1d56734bff914cd62f5e675782b92176";
                info.Receiver_email = "hauxuan94@gmail.com";

                //mk nl ac1d19ab3a0450d7b6d0c3b885448a86
                // secrrt d0c398afaa9c40c3abffc82127a38354 
                //api b6dc94eff0e64a3cafbe6927921bfe03

                info.cur_code = "vnd";
                info.bank_code = str_bankcode;


                //int id_new = db.HoaDon.Max(m => m.MaHd) + 1;
                DateTime currentDate = DateTime.Now;
                // Voucher voucher = db.Voucher.SingleOrDefault(s => s.NgayBatDau <= currentDate && s.NgayHetHan >= currentDate);
                double giamGia = 0;
                // if (voucher != null)
                // {
                //    giamGia = (double)voucher.GiamGia;
                //}
                double totalAmount = 0;
                ViewBag.Cart = GetGioHang;

                totalAmount = GetGioHang.Sum(p => p.SoLuong * p.Gia);
                int order = _context.Orders.Select(p => p.Id).DefaultIfEmpty(0).Max();
                if (order > 0)
                {
                    HttpContext.Session.SetInt32("order", order);
                }

                info.Order_code = "1";//(id_new + 1).ToString();
                info.Total_amount = totalAmount.ToString();
                info.fee_shipping = "0";
                info.Discount_amount = giamGia.ToString();
                info.order_description = "Thanh toan test thu dong hang";
                //info.return_url = "https://cosmeticshop20.azurewebsites.net/Checkout/NganLuongPaid/?orderid=" + HttpContext.Session.GetInt32("OrderId").ToString();
                //info.return_url = BaseURL.GetURL();
                info.return_url = BaseURL.GetURL() + "cart/success";
                info.cancel_url = BaseURL.GetURL() + "cart/Cancel/" + HttpContext.Session.GetInt32("id") + "/" + HttpContext.Session.GetInt32("order"); ;

                info.Buyer_fullname = buyer_fullname;
                info.Buyer_email = buyer_email;
                info.Buyer_mobile = buyer_mobile;

                APICheckoutV3 objNLChecout = new APICheckoutV3();
                ResponseInfo result = objNLChecout.GetUrlCheckout(info, payment_method);

                if (result.Error_code == "00")
                {

                    //SaveOrder(totalAmount, "nganluong");
                    return Redirect(result.Checkout_url);
                    //Response.Redirect(result.Checkout_url);
                }
                else
                    return View();
                // txtserverkt.InnerHtml = result.Description;
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                return View();
            }

        }
    }
}