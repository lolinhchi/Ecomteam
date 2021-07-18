using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraintreeHttp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using PayPal.Core;
using PayPal.v1.Payments;
using TeamBee.Connect_Data;
using TeamBee.Helper;
using TeamBee.Models;
using TeamBee.ViewModels;
using Order = TeamBee.Models.Order;

namespace TeamBee.Controllers
{
    public class CartController : Controller
    {
        private readonly TeamBeeDbContext _context;
        public double TyGiaUSD = 23300;//store in Database
        private readonly string _clientId;
        private readonly string _secretKey;
        public CartController(TeamBeeDbContext context, IConfiguration config)
        {
            _context = context;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }
        public IActionResult Index()
        {

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            return View();
        }

        public IActionResult Checkout(int id)
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            ViewBag.User = _context.Users.Where(p => p.Id == id).SingleOrDefault();
            PayPalConfig payPalConfig = PayPalService.GetPayPalConfig();
            ViewBag.payPalConfig = payPalConfig;
            return View();
        }


        public IActionResult tienmat(OrderViewModels UserOrder)
        {
          //  OrderViewModels UserOrder = JsonConvert.DeserializeObject<OrderViewModels>(User);
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();
            var id = HttpContext.Session.GetInt32("id");
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
            Order order = new Order
            {
                User_Id = (int)id,
                TotalQuantity = (int)ViewBag.Soluong,
                TotalPrice = (int)ViewBag.Total,
                ShipEmail = UserOrder.ShipEmail,
                ShipName = UserOrder.ShipName,
                ShipPhone = UserOrder.ShipPhone,
                Address = UserOrder.Address,
                GhiChu = UserOrder.GhiChu + ' ' + NgayGiao,
                Status = "0",
                Pay ="Tiền Mặt",
                DateCreate = DateTime.Now

            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("Order", order.Id);
            Tracking New_Tracking = new Tracking
            {
                Cart_Id = order.Id,
                TongTien = GetGioHang.Sum(p => p.SoLuong * p.Gia).ToString(),
                SDT = order.ShipPhone.ToString(),
                GhiChu = order.GhiChu,
                Time = DateTime.Now,
                HoTen = order.ShipName,
                ThongTin = "Đã tạo đơn. Chờ xử lý",
                DiaChi = order.Address
            };
            _context.Trackings.Add(New_Tracking);
            _context.SaveChanges();

            string content = "<br>Bạn đã đặt đơn hàng thành công. Tổng Số lượng đơn hàng là: " + ViewBag.Soluong;
            foreach (var item in ViewBag.Cart)
            {

                CartDetail cartdetail = new CartDetail
                {

                    Cart_Id = order.Id,
                    Product_Id = item.Id,
                    Quantity = item.SoLuong,
                    PriceSingle = (int)item.Gia,
                    DateCreate = DateTime.Now
                };
                content += "<br>" + LayTenHang(cartdetail.Product_Id) + " " + cartdetail.Quantity + " x " + String.Format("{0:0,0 VNĐ}", cartdetail.PriceSingle) + " : " + String.Format("{0:0,0 VNĐ}", cartdetail.Quantity * cartdetail.PriceSingle);

                var pro = _context.Products.Where(p => p.Id == cartdetail.Product_Id).SingleOrDefault();
                pro.Orders += 1;
                _context.Products.Update(pro);
                _context.SaveChanges();

                _context.CartDetails.Add(cartdetail);
                _context.SaveChanges();

            }
            //@String.Format("{0:0,0 VNĐ}", @item.Gia)
            string email = UserOrder.ShipEmail;

            content += "<br>Tổng tiền :" + String.Format("{0:0,0 VNĐ}", ViewBag.Total);
            if (SendMailHelper.Send(email, "Đặt Hàng Thành Công", content))
            {
                ViewBag.msg = "Success";
            }
            else
            {
                ViewBag.msg = "Fail";
            }

            SessionHelper.Set(HttpContext.Session, "cart", null);

            return View("ThanhCong");
        }

        //public IActionResult ThanhToanPaypal(string User)
        //{
        //    OrderViewModels UserOrder = JsonConvert.DeserializeObject<OrderViewModels>(User);
        //    ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
        //    ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
        //    // lấy Categoryư
        //    string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
        //    ViewBag.Category = _context.Categorys.ToList();
        //    HttpContext.Session.SetInt32("PayId", UserOrder.Id);
        //    HttpContext.Session.SetString("ShipName", UserOrder.ShipName);
        //    HttpContext.Session.SetString("ShipEmail", UserOrder.ShipEmail);
        //    HttpContext.Session.SetString("ShipPhone", UserOrder.ShipPhone);
        //    HttpContext.Session.SetString("Address", UserOrder.Address);
        //    HttpContext.Session.SetString("GhiChu", UserOrder.GhiChu + ' ' + NgayGiao);


        //    return Content("1");
        //}


        public IActionResult success()
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            var id = HttpContext.Session.GetInt32("id");
            ViewBag.User = _context.Users.Where(p => p.Id == id).SingleOrDefault();
            if (GetGioHang != null)
            {
                ViewBag.Cart = GetGioHang;
                ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
                ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
                //  string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
                Order order = new Order
                {
                    User_Id = (int)id,
                    TotalQuantity = (int)ViewBag.Soluong,
                    TotalPrice = (int)ViewBag.Total,
                    ShipEmail = HttpContext.Session.GetString("ShipEmail"),
                    ShipName = HttpContext.Session.GetString("ShipName"),
                    ShipPhone = HttpContext.Session.GetString("ShipPhone"),
                    Address = HttpContext.Session.GetString("Address"),
                    GhiChu = HttpContext.Session.GetString("GhiChu"),
                    Status = "0",
                    Pay = HttpContext.Session.GetInt32("PayId") == 1 ? "Tiền Mặt" : HttpContext.Session.GetInt32("PayId") == 2 ? "Ngân Lượng"  :"Paypal" ,
                    DateCreate = DateTime.Now
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("Order", order.Id);

                Tracking New_Tracking = new Tracking
                {
                    Cart_Id = order.Id,
                    TongTien = GetGioHang.Sum(p => p.SoLuong * p.Gia).ToString(),
                    SDT = order.ShipPhone.ToString(),
                    GhiChu = order.GhiChu,
                    Time = DateTime.Now,
                    HoTen = order.ShipName,
                    ThongTin = "Đã tạo đơn. Chờ xử lý",
                    DiaChi = order.Address
                };
                _context.Trackings.Add(New_Tracking);
                _context.SaveChanges();
                string content = "<br>Bạn đã đặt đơn hàng thành công. Tổng Số lượng đơn hàng là: " + ViewBag.Soluong;
                foreach (var item in ViewBag.Cart)
                {

                    CartDetail cartdetail = new CartDetail
                    {

                        Cart_Id = order.Id,
                        Product_Id = item.Id,
                        Quantity = item.SoLuong,
                        PriceSingle = (int)item.Gia,
                        DateCreate = DateTime.Now
                    };
                    content += "<br>" + LayTenHang(cartdetail.Product_Id) + " " + cartdetail.Quantity + " x " + String.Format("{0:0,0 VNĐ}", cartdetail.PriceSingle) + " : " + String.Format("{0:0,0 VNĐ}", cartdetail.Quantity * cartdetail.PriceSingle);

                    var pro = _context.Products.Where(p => p.Id == cartdetail.Product_Id).SingleOrDefault();
                    pro.Orders += 1;
                    _context.Products.Update(pro);
                    _context.SaveChanges();

                    _context.CartDetails.Add(cartdetail);
                    _context.SaveChanges();

                }
                //@String.Format("{0:0,0 VNĐ}", @item.Gia)
                //string email = UserOrder.ShipEmail;

                //content += "<br>Tổng tiền :" + String.Format("{0:0,0 VNĐ}", ViewBag.Total);
                //if (SendMailHelper.Send(email, "Đặt Hàng Thành Công", content))
                //{
                //    ViewBag.msg = "Success";
                //}
                //else
                //{
                //    ViewBag.msg = "Fail";
                //}

                SessionHelper.Set(HttpContext.Session, "cart", null);

                return View();
            }
            return NotFound();

        }
        // add Item
        public IActionResult AddItem(int idpro, int soluong)
        {

            List<CartItem> cart = GetGioHang;
            //kiểm tra xem hàng đã có trong giỏ chưa
            CartItem item = cart.SingleOrDefault(p => p.Id == idpro);
            //nếu có
            if (item != null)
            {
                item.SoLuong += soluong;//tăng số lượng
            }
            else
            {
                // dựa vào idpro l
                var pro = _context.Products.SingleOrDefault(p => p.Id == idpro);
                double giasale = pro.Price - ((Double)pro.SalePrice / 100) * pro.Price;
                CartItem Item = new CartItem
                {
                    Id = pro.Id,
                    SoLuong = soluong,
                    Name = pro.Name,
                    Anh = pro.Thumbnail,
                    Gia = giasale,
                    USD = Math.Round((giasale / TyGiaUSD), 2)
                };
                cart.Add(Item);

            }

            SessionHelper.Set(HttpContext.Session, "cart", cart);
            //return Content("ok");
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            return PartialView("_HomeIndex");



        }
        //TĂng Item
        public IActionResult PushItem(int idpro)
        {

            var cart = GetGioHang;
            CartItem item = cart.SingleOrDefault(p => p.Id == idpro);
            item.SoLuong += 1;
            SessionHelper.Set(HttpContext.Session, "cart", cart);
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            return RedirectToAction("index", "cart");

        }
        // Giảm Item
        public IActionResult minus(int idpro, int sl)
        {
            var cart = GetGioHang;
            CartItem item = cart.SingleOrDefault(p => p.Id == idpro);
            if (sl > 1)
            {
                item.SoLuong -= 1;

            }
            else
            {
                cart.Remove(item);
            }
            SessionHelper.Set(HttpContext.Session, "cart", cart);
            return RedirectToAction("index", "cart");
        }
        // xóa Item
        public IActionResult xoa(int idpro)
        {

            var cart = GetGioHang;
            CartItem item = cart.SingleOrDefault(p => p.Id == idpro);
            cart.Remove(item);
            SessionHelper.Set(HttpContext.Session, "cart", cart);
            return RedirectToAction("index", "cart");

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

        public string LayTenHang(int id)
        {
            return _context.Products.Where(p => p.Id == id).SingleOrDefault().Name;

        }

        [Route("~/cart/PayPalPaid/{id?}")]
        public IActionResult PayPalPaid(int id)
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            ViewBag.User = _context.Users.Where(p => p.Id == id).SingleOrDefault();

            // var id = HttpContext.Session.GetInt32("id");
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            //  string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
            Order order = new Order
            {
                User_Id = (int)id,
                TotalQuantity = (int)ViewBag.Soluong,
                TotalPrice = (int)ViewBag.Total,
                ShipEmail = HttpContext.Session.GetString("ShipEmail"),
                ShipName = HttpContext.Session.GetString("ShipName"),
                ShipPhone = HttpContext.Session.GetString("ShipPhone"),
                Address = HttpContext.Session.GetString("Address"),
                GhiChu = HttpContext.Session.GetString("GhiChu"),
                Status = "0",
                Pay = HttpContext.Session.GetInt32("PayId") == 1 ? "Tiền Mặt" : HttpContext.Session.GetInt32("PayId") == 2 ? "Pay Pal" : "Ngân Lượng",
                DateCreate = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("Order", order.Id);
            Tracking New_Tracking = new Tracking
            {
                Cart_Id = order.Id,
                TongTien = GetGioHang.Sum(p => p.SoLuong * p.Gia).ToString(),
                SDT = order.ShipPhone.ToString(),
                GhiChu = order.GhiChu,
                Time = DateTime.Now,
                HoTen = order.ShipName,
                ThongTin = "Đã tạo đơn. Chờ xử lý",
                DiaChi = order.Address
            };
            _context.Trackings.Add(New_Tracking);
            _context.SaveChanges();
            string content = "<br>Bạn đã đặt đơn hàng thành công. Tổng Số lượng đơn hàng là: " + ViewBag.Soluong;
            foreach (var item in ViewBag.Cart)
            {

                CartDetail cartdetail = new CartDetail
                {

                    Cart_Id = order.Id,
                    Product_Id = item.Id,
                    Quantity = item.SoLuong,
                    PriceSingle = (int)item.Gia,
                    DateCreate = DateTime.Now
                };
                content += "<br>" + LayTenHang(cartdetail.Product_Id) + " " + cartdetail.Quantity + " x " + String.Format("{0:0,0 VNĐ}", cartdetail.PriceSingle) + " : " + String.Format("{0:0,0 VNĐ}", cartdetail.Quantity * cartdetail.PriceSingle);

                var pro = _context.Products.Where(p => p.Id == cartdetail.Product_Id).SingleOrDefault();
                pro.Orders += 1;
                _context.Products.Update(pro);
                _context.SaveChanges();

                _context.CartDetails.Add(cartdetail);
                _context.SaveChanges();

            }
            //@String.Format("{0:0,0 VNĐ}", @item.Gia)
            //string email = UserOrder.ShipEmail;

            //content += "<br>Tổng tiền :" + String.Format("{0:0,0 VNĐ}", ViewBag.Total);
            //if (SendMailHelper.Send(email, "Đặt Hàng Thành Công", content))
            //{
            //    ViewBag.msg = "Success";
            //}
            //else
            //{
            //    ViewBag.msg = "Fail";
            //}

            SessionHelper.Set(HttpContext.Session, "cart", null);


            return RedirectToAction("ThanhCong", "cart");
        }
        [Route("~/cart/Cancel/{id}/{order}")]
        public IActionResult Cancel(int id, int order)

        {


            Order delete = _context.Orders.Where(p => p.User_Id == id && p.Id == order).FirstOrDefault();
            if (delete != null)
            {
                List<CartDetail> details = _context.CartDetails.Where(od => od.Cart_Id == id).ToList();
                if (details != null)
                {
                    foreach (var detail in details)
                    {
                        _context.CartDetails.Remove(detail);
                        _context.SaveChanges();
                    }
                }
                _context.Orders.Remove(delete);
                _context.SaveChanges();
            }

            SessionHelper.Set(HttpContext.Session, "cart", null);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ThanhToanPaypal(string User)
        {
            OrderViewModels UserOrder = JsonConvert.DeserializeObject<OrderViewModels>(User);
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Categoryư
            string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
            ViewBag.Category = _context.Categorys.ToList();
            HttpContext.Session.SetInt32("PayId", UserOrder.Id);
            HttpContext.Session.SetString("ShipName", UserOrder.ShipName);
            HttpContext.Session.SetString("ShipEmail", UserOrder.ShipEmail);
            HttpContext.Session.SetString("ShipPhone", UserOrder.ShipPhone);
            HttpContext.Session.SetString("Address", UserOrder.Address);
            HttpContext.Session.SetString("GhiChu", UserOrder.GhiChu + ' ' + NgayGiao);

            //var environment = new SandboxEnvironment(_clientId, _secretKey);
            //var client = new PayPalHttpClient(environment);
            //#region Create Paypal Order
            //var itemList = new ItemList()
            //{
            //    Items = new List<Item>()
            //};
            //var total = Math.Round(GetGioHang.Sum(p => p.SoLuong * p.Gia) / TyGiaUSD, 2);
            //foreach (var item in GetGioHang)
            //{
            //    itemList.Items.Add(new Item()
            //    {
            //        Name = item.Name,
            //        Currency = "USD",
            //        Price = Math.Round(item.Gia / TyGiaUSD, 2).ToString(),
            //        Quantity = item.SoLuong.ToString(),
            //        Sku = "sku",
            //        Tax = "0"
            //    });
            //}
            //#endregion

            //var paypalOrderId = DateTime.Now.Ticks;
            //var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            //var payment = new Payment()
            //{
            //    Intent = "sale",
            //    Transactions = new List<Transaction>()
            //    {
            //        new Transaction()
            //        {
            //            Amount = new Amount()
            //            {
            //                Total = total.ToString(),
            //                Currency = "USD",
            //                Details = new AmountDetails
            //                {
            //                    Tax = "0",
            //                    Shipping = "0",
            //                    Subtotal = total.ToString()
            //                }
            //            },
            //            ItemList = itemList,
            //            Description = $"Invoice #{paypalOrderId}",
            //            InvoiceNumber = paypalOrderId.ToString()
            //        }
            //    },
            //    RedirectUrls = new RedirectUrls()
            //    {
            //        CancelUrl = $"{hostname}/home/index",
            //        ReturnUrl = $"{hostname}/cart/success"
            //    },
            //    Payer = new Payer()
            //    {
            //        PaymentMethod = "paypal"
            //    }
            //};

            //PaymentCreateRequest request = new PaymentCreateRequest();
            //request.RequestBody(payment);

            //try
            //{
            //    var response = await client.Execute(request);
            //    var statusCode = response.StatusCode;
            //    Payment result = response.Result<Payment>();

            //    var links = result.Links.GetEnumerator();
            //    string paypalRedirectUrl = null;
            //    while (links.MoveNext())
            //    {
            //        LinkDescriptionObject lnk = links.Current;
            //        if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
            //        {
            //            //saving the payapalredirect URL to which user will be redirected for payment  
            //            paypalRedirectUrl = lnk.Href;
            //        }
            //    }

            //    return Content(paypalRedirectUrl);

            //}
            //catch (HttpException httpException)
            //{
            //    var statusCode = httpException.StatusCode;
            //    var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

            //    //Process when Checkout with Paypal fails
            //    return Content("/home/index");
            //}
            return View();

        }

        public async Task<IActionResult> CheckoutPayMent(OrderViewModels UserOrder)
        {
            UserOrder.Status = 2;
            //    if (UserOrder.Status == 3)
            //    {
            //        ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            //        ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            //        // lấy Categoryư
            //        string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
            //        ViewBag.Category = _context.Categorys.ToList();
            //        HttpContext.Session.SetInt32("PayId", UserOrder.Status);
            //        HttpContext.Session.SetString("ShipName", UserOrder.ShipName);
            //        HttpContext.Session.SetString("ShipEmail", UserOrder.ShipEmail);
            //        HttpContext.Session.SetString("ShipPhone", UserOrder.ShipPhone);
            //        HttpContext.Session.SetString("Address", UserOrder.Address);
            //        HttpContext.Session.SetString("GhiChu", UserOrder.GhiChu + ' ' + NgayGiao);
            //       // HttpContext.Session.SetInt32("Status", UserOrder.Status);

            //        var environment = new SandboxEnvironment(_clientId, _secretKey);
            //        var client = new PayPalHttpClient(environment);
            //        #region Create Paypal Order
            //        var itemList = new ItemList()
            //        {
            //            Items = new List<Item>()
            //        };
            //        var total =GetGioHang.Sum(p => p.SoLuong * p.USD);
            //        foreach (var item in GetGioHang)
            //        {
            //            itemList.Items.Add(new Item()
            //            {
            //                Name = item.Name,
            //                Currency = "USD",
            //                Price = item.USD.ToString(),
            //                Quantity = item.SoLuong.ToString(),
            //                Sku = "sku",
            //                Tax = "0"
            //            }); ;
            //        }
            //        #endregion

            //        var paypalOrderId = DateTime.Now.Ticks;
            //        var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            //        var payment = new Payment()
            //        {
            //            Intent = "sale",
            //            Transactions = new List<Transaction>()
            //        {
            //            new Transaction()
            //            {
            //                Amount = new Amount()
            //                {
            //                    Total = total.ToString(),
            //                    Currency = "USD",
            //                    Details = new AmountDetails
            //                    {
            //                        Tax = "0",
            //                        Shipping = "0",
            //                        Subtotal = total.ToString()
            //                    }
            //                },
            //                ItemList = itemList,
            //                Description = $"Invoice #{paypalOrderId}",
            //                InvoiceNumber = paypalOrderId.ToString()
            //            }
            //        },
            //            RedirectUrls = new RedirectUrls()
            //            {
            //                CancelUrl = $"{hostname}/home/index",
            //                ReturnUrl = $"{hostname}/cart/success"
            //            },
            //            Payer = new Payer()
            //            {
            //                PaymentMethod = "paypal"
            //            }
            //        };

            //        PaymentCreateRequest request = new PaymentCreateRequest();
            //        request.RequestBody(payment);

            //        try
            //        {
            //            var response = await client.Execute(request);
            //            var statusCode = response.StatusCode;
            //            Payment result = response.Result<Payment>();

            //            var links = result.Links.GetEnumerator();
            //            string paypalRedirectUrl = null;
            //            while (links.MoveNext())
            //            {
            //                LinkDescriptionObject lnk = links.Current;
            //                if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
            //                {
            //                    //saving the payapalredirect URL to which user will be redirected for payment  
            //                    paypalRedirectUrl = lnk.Href;
            //                }
            //            }

            //            return Redirect(paypalRedirectUrl);

            //        }
            //        catch (HttpException httpException)
            //        {
            //            var statusCode = httpException.StatusCode;
            //            var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

            //            //Process when Checkout with Paypal fails
            //            return Redirect("/home/index");
            //        }

            //    }
            //    else
            //if(UserOrder.Status == 2)
            //{
                string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
                ViewBag.Category = _context.Categorys.ToList();
                HttpContext.Session.SetInt32("PayId", UserOrder.Status);
                HttpContext.Session.SetString("ShipName", UserOrder.ShipName);
                HttpContext.Session.SetString("ShipEmail", UserOrder.ShipEmail);
                HttpContext.Session.SetString("ShipPhone", UserOrder.ShipPhone);
                HttpContext.Session.SetString("Address", UserOrder.Address);
                HttpContext.Session.SetString("GhiChu", UserOrder.GhiChu + ' ' + NgayGiao);
                // HttpContext.Session.SetInt32("Status", UserOrder.Status);
                return Redirect("/nganluong/index");
            //}
            //    else
            //    {

            //        // OrderViewModels UserOrder = JsonConvert.DeserializeObject<OrderViewModels>(User);
            //        ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            //        ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            //        // lấy Category
            //        ViewBag.Category = _context.Categorys.ToList();
            //        var id = HttpContext.Session.GetInt32("id");
            //        ViewBag.Cart = GetGioHang;
            //        ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            //        ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            //        string NgayGiao = UserOrder.NgayGiao != null ? "Ngày: " + UserOrder.NgayGiao : "";
            //        Order order = new Order
            //        {
            //            User_Id = (int)id,
            //            TotalQuantity = (int)ViewBag.Soluong,
            //            TotalPrice = (int)ViewBag.Total,
            //            ShipEmail = UserOrder.ShipEmail,
            //            ShipName = UserOrder.ShipName,
            //            ShipPhone = UserOrder.ShipPhone,
            //            Address = UserOrder.Address,
            //            GhiChu = UserOrder.GhiChu + ' ' + NgayGiao,
            //            Status = "0",
            //            Pay = UserOrder.Status ==1? "Tiền Mặt" :"Ngân Lương",
            //            DateCreate = DateTime.Now

            //        };

            //        _context.Orders.Add(order);
            //        _context.SaveChanges();
            //        HttpContext.Session.SetInt32("Order", order.Id);


            //        Tracking New_Tracking = new Tracking {
            //            Cart_Id = order.Id,
            //            TongTien = GetGioHang.Sum(p => p.SoLuong * p.Gia).ToString(),
            //            SDT = order.ShipPhone.ToString(),
            //            GhiChu = order.GhiChu,
            //            Time = DateTime.Now,
            //            HoTen = order.ShipName,
            //            ThongTin = "Đã tạo đơn. Chờ xử lý",
            //            DiaChi = order.Address
            //        };
            //        _context.Trackings.Add(New_Tracking);
            //        _context.SaveChanges();
            //        string content = "<br>Bạn đã đặt đơn hàng thành công. Tổng Số lượng đơn hàng là: " + ViewBag.Soluong;
            //        foreach (var item in ViewBag.Cart)
            //        {

            //            CartDetail cartdetail = new CartDetail
            //            {

            //                Cart_Id = order.Id,
            //                Product_Id = item.Id,
            //                Quantity = item.SoLuong,
            //                PriceSingle = (int)item.Gia,
            //                DateCreate = DateTime.Now
            //            };
            //            content += "<br>" + LayTenHang(cartdetail.Product_Id) + " " + cartdetail.Quantity + " x " + String.Format("{0:0,0 VNĐ}", cartdetail.PriceSingle) + " : " + String.Format("{0:0,0 VNĐ}", cartdetail.Quantity * cartdetail.PriceSingle);

            //            var pro = _context.Products.Where(p => p.Id == cartdetail.Product_Id).SingleOrDefault();
            //            pro.Orders += 1;
            //            _context.Products.Update(pro);
            //            _context.SaveChanges();

            //            _context.CartDetails.Add(cartdetail);
            //            _context.SaveChanges();

            //        }
            //        //@String.Format("{0:0,0 VNĐ}", @item.Gia)
            //        string email = UserOrder.ShipEmail;

            //        content += "<br>Tổng tiền :" + String.Format("{0:0,0 VNĐ}", ViewBag.Total);
            //        if (SendMailHelper.Send(email, "Đặt Hàng Thành Công", content))
            //        {
            //            ViewBag.msg = "Success";
            //        }
            //        else
            //        {
            //            ViewBag.msg = "Fail";
            //        }

            //        SessionHelper.Set(HttpContext.Session, "cart", null);
            //        return Redirect("/cart/ThanhCong");
            //    }


        }
        public IActionResult ThanhCong()
        {
            return View();
        }
    }
}