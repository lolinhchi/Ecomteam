using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using TeamBee.Connect_Data;
using TeamBee.Helper;
using TeamBee.Models;
using TeamBee.ViewModels;
using Twilio;

using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

namespace TeamBee.Controllers
{
    public class UserController : Controller
    {
        private readonly TeamBeeDbContext _context;
        private readonly string AuthyAPIKey = ConfigurationBuilderExtensions.GetAuthyAPIKey();
        public UserController(TeamBeeDbContext context)
        {
            _context = context;
        }


        public IActionResult Login(string message = "", string messagedk = "")
        {
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            this.ViewBag.Error = message;
            this.ViewBag.Dk = messagedk;

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModels user)
        {
            if (user.Username == null || user.Password == null)
            {
                this.ViewBag.Error = "Vui lòng nhập tài khoản và mật khẩu!";
                return (RedirectToAction("Login", new { message = this.ViewBag.Error }));
            }
            else
            {
                var md5PassWord = MD5.GetMD5(user.Password.Trim());
                var UserLogin = _context.Users.Where(p => p.UserName == user.Username.Trim() && p.Password == md5PassWord).SingleOrDefault();

                if (UserLogin != null)
                {
                    if (UserLogin.Confirm == 1)
                    {
                        HttpContext.Session.SetInt32("admin", Convert.ToInt32(UserLogin.Permission));
                        HttpContext.Session.SetInt32("id", Convert.ToInt32(UserLogin.Id));
                        // 
                        HttpContext.Session.SetString("Name", UserLogin.NameLast.ToString() + ' ' + UserLogin.NameFirst.ToString());

                        return RedirectToAction("index", "home");

                    }
                    else
                    {
                        this.ViewBag.Error = "Tài Khoản chưa được kích hoạt vui lòng vào email xác nhận !";
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

        public IActionResult LoginFB(string token)
        {
            return Content(token);
        }

        public IActionResult TaoTaiKhoanChoFB(TaiKhoanLoginFB taiKhoan)
        {
            try
            {
                var mail = taiKhoan.email != null ? taiKhoan.email : taiKhoan.id + "@gmail.com";
                User check = _context.Users.Where(p => p.Email == mail).SingleOrDefault();
                if (check != null)
                {
                    
                    HttpContext.Session.SetInt32("id", Convert.ToInt32(check.Id));
                    // 
                    HttpContext.Session.SetString("Name", taiKhoan.last_name.ToString() + ' ' + taiKhoan.first_name.ToString());


                }
                else
                {
                    User new_user = new User
                    {
                        UserName = taiKhoan.email != null ? taiKhoan.email : taiKhoan.id,
                        Password = "1",
                        Email = taiKhoan.email != null ? taiKhoan.email : taiKhoan.id + "@gmail.com",
                        NameFirst = taiKhoan.last_name,
                        NameLast = taiKhoan.first_name,
                        Address = "quận 5",
                        PhoneNumber = taiKhoan.id,
                        Permission = 0,
                        Active = 0,
                        Confirm = 1

                    };
                    _context.Users.Add(new_user);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("id", Convert.ToInt32(new_user.Id));
                    // 
                    HttpContext.Session.SetString("Name", new_user.NameLast.ToString() + ' ' + new_user.NameFirst.ToString());
                }
                return Content("1");
            }
            catch
            {
                return Content("0");
            }


        }
        public IActionResult ViewDetaliOrder(int id)
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

        public IActionResult SuaTaiKhoan(UserViewModels user)
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            string url = "~/user/profile?id=" + user.Id;
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


                HttpContext.Session.SetInt32("edit", 1);
                return Redirect(url);
            }
            catch
            {
                HttpContext.Session.SetInt32("edit", 0);
                return Redirect(url);
            }

        }
        public IActionResult success()
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
        public IActionResult OrdersList()
        {
            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");

            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();


            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            ViewBag.OrdersList = _context.Orders.Where(p => p.User_Id == HttpContext.Session.GetInt32("id")).ToList();
            return View();
        }
        public IActionResult Wishlist()
        {

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // danh sách yêu thích
            ViewBag.Wishlist = GetWishlist;
            ViewBag.Soluong1 = GetWishlist.Sum(p => p.Id);
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();


            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            return View();

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("id");
            SessionHelper.Set(HttpContext.Session, "cart", null);
            SessionHelper.Set(HttpContext.Session, "Wishlist", null);
            return RedirectToAction("index", "home");
        }

        public IActionResult Profile(int id)
        {

            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // map cart
            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);
            if (HttpContext.Session.GetInt32("edit") == 1)
            {
                this.ViewBag.tk = "Sửa Thành Công";
            }
            else if (HttpContext.Session.GetInt32("edit") == 0)
            {
                this.ViewBag.tk = "Sửa Thất Bại";
            }
            else
            {
                this.ViewBag.tk = "";
            }
            HttpContext.Session.Remove("edit");
            return View(_context.Users.Where(p => p.Id == id).SingleOrDefault());




        }
        public IActionResult AddWishlist(int idpro)
        {
            List<WishItem> Wishlist = GetWishlist;


            //kiểm tra xem hàng đã có trong wishlist chưa
            WishItem item = Wishlist.SingleOrDefault(p => p.Id == idpro);
            //nếu có
            if (item == null)
            {
                var pro = _context.Products.SingleOrDefault(p => p.Id == idpro);
                WishItem Item = new WishItem
                {
                    Id = pro.Id,
                    Name = pro.Name,
                    Anh = pro.Thumbnail,
                    Gia = pro.Price - ((Double)pro.SalePrice / 100) * pro.Price
                };
                Wishlist.Add(Item);

            }

            SessionHelper.Set(HttpContext.Session, "Wishlist", Wishlist);
            // danh sách yêu thích
            ViewBag.Wishlist = GetWishlist;
            ViewBag.Soluong1 = GetWishlist.Sum(p => p.Id);
            return PartialView("_ThemDanhSachYeuThich");
        }

        public IActionResult AddCartItemToWishList(int idpro)
        {
            List<CartItem> cart = GetGioHang;
            //kiểm tra xem hàng đã có trong giỏ chưa
            CartItem item = cart.SingleOrDefault(p => p.Id == idpro);
            //nếu có
            if (item != null)
            {
                item.SoLuong += 1;//tăng số lượng
            }
            else
            {
                var pro = _context.Products.SingleOrDefault(p => p.Id == idpro);
                CartItem Item = new CartItem
                {
                    Id = pro.Id,
                    SoLuong = 1,
                    Name = pro.Name,
                    Anh = pro.Thumbnail,
                    Gia = pro.Price - ((Double)pro.SalePrice / 100) * pro.Price
                };
                cart.Add(Item);
            }
            // gán lại giỏ hàng
            SessionHelper.Set(HttpContext.Session, "cart", cart);

            // thêm xong xóa sp trong wishlist
            var Wishlist = GetWishlist;
            WishItem item1 = Wishlist.SingleOrDefault(p => p.Id == idpro);
            Wishlist.Remove(item1);
            SessionHelper.Set(HttpContext.Session, "Wishlist", Wishlist);
            // danh sách yêu thích
            ViewBag.Wishlist = GetWishlist;
            ViewBag.Soluong1 = GetWishlist.Sum(p => p.Id);
            return PartialView("_DanhSachYeuThichThemGioHang");

        }

        public IActionResult CheckEmail(string email)
        {
            User user = _context.Users.Where(p => p.Email == email.Trim()).SingleOrDefault();
            if (user != null)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        public IActionResult CheckTenDangNhap(string username)
        {
            var aa = _context.Users.Where(p => p.UserName == username.Trim());
            User user = _context.Users.Where(p => p.UserName == username.Trim()).SingleOrDefault();
            if (user != null)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        public IActionResult CheckSoDienThoai(string sdt)
        {
            User user = _context.Users.Where(p => p.PhoneNumber == sdt.Trim()).SingleOrDefault();
            if (user != null)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        [HttpPost] // gắn này nè 
        public IActionResult Register(UserViewModels user)
        {
            try
            {
                // lấy Category
                ViewBag.Category = _context.Categorys.ToList();
                User New_User = new User()
                {
                    UserName = user.UserName,
                    NameFirst = user.NameFirst,
                    NameLast = user.NameLast,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Password = MD5.GetMD5(user.Password.Trim()),
                    Confirm = 1,
                    Active = 0,
                    Permission = 0,
                };

                

                //string accountSid = Sms.GetAccountSid();
                //string authToken = Sms.GetAuthToken();

                //TwilioClient.Init(accountSid, authToken);

                //string sdt = Sms.GetToPhone().Substring(1);
                //string from = Sms.GetFromPhone();
                //var message = MessageResource.Create(
                //    body: New_User.UserName + " Đã đăng kí tài khoản tại shop của bạn",
                //    from: new Twilio.Types.PhoneNumber(from),
                //    to: new Twilio.Types.PhoneNumber("+84" + sdt)
                //);
                _context.Users.Add(New_User);
                _context.SaveChanges();
                var body = "Nhấn " + "<a href=" + @TeamBee.Helper.BaseURL.GetURL() + "user/kichhoat/" + New_User.Id + ">vào đây" + "</a>" + " để kích hoạt tài khoản.";
                if (SendMailHelper.Send(New_User.Email, "Đăng kí tài khoản thành công", body))
                {
                    ViewBag.msg = "Success";
                }
                else
                {
                    ViewBag.msg = "Fail";
                }
                this.ViewBag.Dk = "Đăng Kí Thành Công!";
                return (RedirectToAction("Login", new { messagedk = this.ViewBag.Dk }));
            }
            catch (Exception ex)
            {
                this.ViewBag.Dk = "Đăng Kí Thất Bại..!";
                return (RedirectToAction("Login", new { messagedk = this.ViewBag.Dk }));
            }
        }
        public IActionResult xoa(int idpro)
        {

            var Wishlist = GetWishlist;
            WishItem item = Wishlist.SingleOrDefault(p => p.Id == idpro);
            Wishlist.Remove(item);
            SessionHelper.Set(HttpContext.Session, "Wishlist", Wishlist);
            // danh sách yêu thích
            ViewBag.Wishlist = GetWishlist;
            ViewBag.Soluong1 = GetWishlist.Sum(p => p.Id);
            return PartialView("_XoaDanhSachYeuThich");

        }
        // get sesion
        public List<WishItem> GetWishlist
        {

            get
            {
                List<WishItem> myWishlist = SessionHelper.Get<List<WishItem>>(HttpContext.Session, "Wishlist");
                if (myWishlist == default(List<WishItem>))
                {
                    myWishlist = new List<WishItem>();
                }
                return myWishlist;
            }
        }
        // get sesion
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
        public IActionResult verifiy()
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
        [HttpPost]
        public async Task<IActionResult> verifiy(string verifiy)
        {

            ItemProductsViewModel itemProductsViewModel = new ItemProductsViewModel(_context);
            ViewBag.AllProduct = itemProductsViewModel.GetProducts("all");
            // lấy Category
            ViewBag.Category = _context.Categorys.ToList();

            ViewBag.Cart = GetGioHang;
            ViewBag.Total = GetGioHang.Sum(p => p.SoLuong * p.Gia);
            ViewBag.Soluong = GetGioHang.Sum(p => p.SoLuong);

            //var client = new HttpClient();

            //// Add authentication header
            //client.DefaultRequestHeaders.Add("X-Authy-API-Key", AuthyAPIKey);

            //// https://api.authy.com/protected/$AUTHY_API_FORMAT/phones/verification/check?phone_number=$USER_PHONE&country_code=$USER_COUNTRY&verification_code=$VERIFY_CODE
            //HttpResponseMessage response = client.GetAsync(
            //  "https://api.authy.com/protected/json/phones/verification/check?phone_number=" + Sms.GetToPhone() + "&country_code=84&verification_code=" + verifiy).Result;

            //// Get the response content.
            //HttpContent responseContent = response.Content;
            //// Get the response content.
            //SmsVerifly smsVerifly = JsonConvert.DeserializeObject<SmsVerifly>(responseContent.ReadAsStringAsync().Result);


            if (verifiy.Length>=4)
            {
                string accountSid = ConfigurationBuilderExtensions.GetAccountSid();
                string authToken = ConfigurationBuilderExtensions.GetAuthToken();
                var x = ConfigurationBuilderExtensions.GetAccountSid();
                TwilioClient.Init(accountSid, authToken);
                var verificationCheck = VerificationCheckResource.Create(
                       to: HttpContext.Session.GetString("SDT"),
                       code: verifiy,
                       pathServiceSid: ConfigurationBuilderExtensions.GetServiceSid()
                   );

                //Console.WriteLine(verificationCheck.Status);


                if (bool.Parse(verificationCheck.Valid.ToString()))
                {
                    User UserLogin = _context.Users.Where(p=>p.Id==Int32.Parse( HttpContext.Session.GetInt32("iduser").ToString())).SingleOrDefault();

                    HttpContext.Session.SetInt32("admin", Convert.ToInt32(UserLogin.Permission));

                    HttpContext.Session.SetInt32("id", Convert.ToInt32(UserLogin.Id));
                    //// 
                    HttpContext.Session.SetString("Name", UserLogin.NameLast.ToString() + ' ' + UserLogin.NameFirst.ToString());

                    return RedirectToAction("index", "home");
                }
                else
                {
                    this.ViewBag.loi = "Mã xác nhận không đúng vui lòng nhập lại";
                    return View();
                }

            }
            else
            {
                this.ViewBag.loi = "Mã xác nhận không đúng vui lòng nhập lại";
                return View();
            }
        }

        public string RandomKey()
        {
            Random rd = new Random();
            return rd.Next(100000, 999999).ToString();
        }

        public IActionResult GuiLaiSms()
        {
            //var client = new HttpClient();
            ////string sdt = Sms.GetToPhone().Substring(1);

            //client.DefaultRequestHeaders.Add("X-Authy-API-Key", AuthyAPIKey);

            //var requestContent = new FormUrlEncodedContent(new[] {
            //                    new KeyValuePair<string, string>("via", "sms"),
            //                    new KeyValuePair<string, string>("phone_number", HttpContext.Session.GetString("SDT")),
            //                     new KeyValuePair<string, string>("locale", "vi"),
            //                      new KeyValuePair<string, string>("code_length", "6"),
            //                    new KeyValuePair<string, string>("country_code", "84"),


            //                });

            //HttpResponseMessage response = client.PostAsync(
            //    "https://api.authy.com/protected/json/phones/verification/start",
            //    requestContent).Result;

            //HttpContent responseContent = response.Content;

            //Console.WriteLine(responseContent.ReadAsStringAsync().Result);

            string accountSid = ConfigurationBuilderExtensions.GetAccountSid();
            string authToken = ConfigurationBuilderExtensions.GetAuthToken();
            //string sdt = "+84" + UserLogin.PhoneNumber.Substring(1);
            
            TwilioClient.Init(accountSid, authToken);

            var verificationCheck = VerificationResource.Create(
                to: HttpContext.Session.GetString("SDT"),
                channel: "sms",
                pathServiceSid: ConfigurationBuilderExtensions.GetServiceSid()
            );
            this.ViewBag.loi = "Mã xác nhận đã được gửi. Vui lòng kiểm tra lại.";
            return RedirectToAction("verifiy", "user");
        }

        [Route("~/sitemap.xml")]
        public IActionResult sitemap()
        {
            XmlDocument document = new XmlDocument();
         
            string newpath = Path.Combine(Directory.GetCurrentDirectory());
            string url = newpath + "/sitemap.xml";
            document.Load(url);
            return Content(document.InnerXml, "text/xml");
            // return View("~/user/sitemap.xml");
        }

        [Route("~/robots.txt")]
        public IActionResult robots()
        {
           

            string newpath = Path.Combine(Directory.GetCurrentDirectory());
            string url = newpath + "/robots.txt";
            var x = System.IO.File.ReadAllText(url);
            return Content(x);
            // return View("~/user/sitemap.xml");
        }

        public IActionResult kichhoat(int id)
        {
            User User_Old = _context.Users.Where(p => p.Id == id).SingleOrDefault();
            User_Old.Confirm = 1;
            _context.Users.Update(User_Old);
            _context.SaveChanges();
            this.ViewBag.Error = "Kích hoạt tài khoản thành công!";
            return (RedirectToAction("Login", new { message = this.ViewBag.Error }));
        }

        public IActionResult Quenmatkhau()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Quenmatkhau(string email)
        {
            User User_Old = _context.Users.Where(p => p.Email == email).SingleOrDefault();
            HttpContext.Session.SetInt32("Idquenmatkhau", User_Old.Id);
            return View();
        }

        public IActionResult doimatkhau(int id)
        {
           // User User_Old = _context.Users.Where(p => p.Email == email).SingleOrDefault();
           // HttpContext.Session.SetInt32("Idquenmatkhau", User_Old.Id);
            return View();
        }
        public IActionResult Resetmatkhau(string matkhau)
        {
            int id = (int)HttpContext.Session.GetInt32("Idquenmatkhau");
            User User_Old = _context.Users.Where(p => p.Id == id).SingleOrDefault();
            User_Old.Password = matkhau;
            _context.Users.Update(User_Old);
            _context.SaveChanges();
            this.ViewBag.Error = "Kích hoạt tài khoản thành công!";
            return (RedirectToAction("Login", new { message = this.ViewBag.Error }));
        }


        public IActionResult Tracking(int id)
        {
            var query = (from T in _context.Trackings where T.Cart_Id ==id
                                               
                                               orderby T.Id ascending

                                               select new TrackingViewModel
                                               {
                                                   Id=T.Id.ToString(),
                                                   ThongTin=T.ThongTin,
                                                   HoTen=T.HoTen,
                                                   GhiChu=T.GhiChu,
                                                   DiaChi=T.DiaChi,
                                                   TongTien=T.TongTien.ToString(),
                                                   SDT=T.SDT,
                                                   Time=T.Time.ToString("HH:mm"),
                                                   Ngay=T.Time.ToString("dd/MM/yyyy")

                                               });
           var list  = query.ToList();
            string sJSONResponse = JsonConvert.SerializeObject(list);
            return Content(sJSONResponse);
        }
    }
}