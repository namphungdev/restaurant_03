using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Helpers;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class CartController : Controller
    {
        private readonly RestaurantContext _context;


        public CartController(RestaurantContext context)
        {
            _context = context;


        }
        public async Task<IActionResult> Index()
        {
            var sp = _context.SanPhams.ToList();
            
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart") == null)
            {
                List<GioHang> cart = new List<GioHang>();               
                return View(cart);
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart").ToList();
                return View(cart);
            }
            
        }
        public async Task<IActionResult> Buy(int id, int soluong)
        {
            var sp = _context.SanPhams.ToList();
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart") == null)
            {
                List<GioHang> cart = new List<GioHang>();
                cart.Add(new GioHang { SanPham = sp.Find(p => p.MaSanPham == id), SoLuong = soluong  });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {              
                        cart[index].SoLuong += soluong;
                    
                }
                else
                {
                    cart.Add(new GioHang { SanPham = sp.Find(p => p.MaSanPham == id), SoLuong = soluong });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> update(int id, int soluong)
        {
            var sp = _context.SanPhams.ToList();
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart") == null)
            {
                List<GioHang> cart = new List<GioHang>();
                cart.Add(new GioHang { SanPham = sp.Find(p => p.MaSanPham == id), SoLuong = soluong });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    if(soluong < 1)
                    {
                        cart[index].SoLuong = 1;
                    }
                    else
                    {
                        cart[index].SoLuong = soluong;
                    }
                        
                  

                }
                else
                {
                    cart.Add(new GioHang { SanPham = sp.Find(p => p.MaSanPham == id), SoLuong = soluong });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int id)
        {
            List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].SanPham.MaSanPham.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public async Task<IActionResult> CheckOut()
        {
            var cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            var login = SessionHelper.GetObjectFromJson<List<Login>>(HttpContext.Session, "login");
            if (login == null)
            {

                return RedirectToAction("Index", "KhachHangs");


            }
            else
            {
                return View(login);
            }
        }

        [HttpPost]
        public async Task<IActionResult> thanhtoan(string email, string diachi, string sdt, int id)
        {
            if(id == 0)
            {
                ViewBag.loi = "Vui lòng chọn phương thức thanh toán !!!";
                var login = SessionHelper.GetObjectFromJson<List<Login>>(HttpContext.Session, "login");
                return View("CheckOut", login);
            }
            else
            {
                if (id == 1)
                {
                    var cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
                    var login = SessionHelper.GetObjectFromJson<List<Login>>(HttpContext.Session, "login");
                    int tien = 35000;
                    int soluong = 0;
                    int giamgia = 0;
                    foreach (var i in cart)
                    {
                        giamgia = (((int?)i.SanPham.Tien).Value - (((int?)i.SanPham.Tien).Value * ((int?)i.SanPham.GiamGia).Value / 100)) * i.SoLuong;
                        tien = tien + giamgia;
                        soluong = soluong + i.SoLuong;
                    }

                    var kh = _context.KhachHangs.First(p => p.Email == email);
                    HoaDon hd = new HoaDon();
                    hd.MaKhachHang = kh.MaKhachHang;
                    hd.NgayLap = DateTime.Now;
                    hd.TongTien = tien;
                    hd.SoLuong = soluong;
                    hd.DiaChi = diachi;
                    hd.Sdt = sdt;
                    hd.MaThanhToan = 1;
                    hd.MaVanChuyen = 1;
                    _context.HoaDons.Add(hd);
                    _context.SaveChanges();

                    int MaHoadon = hd.MaHoaDon;
                    List<ChiTietHoaDon> chitiethoadon = new List<ChiTietHoaDon>();
                    foreach (var item in cart)
                    {
                        ChiTietHoaDon cthd = new ChiTietHoaDon();
                        cthd.MaHoaDon = MaHoadon;
                        cthd.MaSanPham = item.SanPham.MaSanPham;
                        cthd.SoLuong = item.SoLuong;
                        cthd.TongTien = ((int?)item.SanPham.Tien).Value - (((int?)item.SanPham.Tien).Value * ((int?)item.SanPham.GiamGia).Value / 100);
                        chitiethoadon.Add(cthd);
                    }
                    _context.ChiTietHoaDons.AddRange(chitiethoadon);
                    _context.SaveChanges();
                    SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart").Clear();
                    List<GioHang> gioHangs = new List<GioHang>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", gioHangs);
                    return RedirectToAction("Index", "Home");

                }
                else 
                {
                    return RedirectToAction("Payment", "Cart");
                }
            }
           
            
        }

        public ActionResult Payment()
        {
            string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string returnUrl = "https://localhost:44319/Cart/PaymentConfirm";
            string tmnCode = "1P63PQKJ";
            string hashSecret = "KRGPFKMOBRONSHVPKNMVFOXTVWFKHLKI";
            var cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            var login = SessionHelper.GetObjectFromJson<List<Login>>(HttpContext.Session, "login");
            int tien = 35000;
            int soluong = 0;
            int giamgia = 0;
           /* int MaHoadon =0; */
            foreach (var i in cart)
            {
                giamgia = (((int?)i.SanPham.Tien).Value - (((int?)i.SanPham.Tien).Value * ((int?)i.SanPham.GiamGia).Value / 100)) * i.SoLuong;
                tien = tien + giamgia;
                soluong = soluong + i.SoLuong;
            }       
            var LastHoaDon = _context.HoaDons.ToList().Last();
            int LastMaHoaDon = LastHoaDon.MaHoaDon;
                PayLib pay = new PayLib();
                string GetIpAddress = (Request.HttpContext.Connection.RemoteIpAddress).ToString();
                pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                pay.AddRequestData("vnp_Amount", (tien * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                pay.AddRequestData("vnp_IpAddr", GetIpAddress); //Địa chỉ IP của khách hàng thực hiện giao dịch
                pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang " + (LastMaHoaDon + 1).ToString()); //Thông tin mô tả nội dung thanh toán
                pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
                string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
                return Redirect(paymentUrl);

            

            
        }

        public ActionResult PaymentConfirm()
        {

            if (Request.QueryString.Value.Count() > 0)
            {
                string hashSecret = "BAGAOHAPRHKQZASKQZASVPRSAKPXNYXS"; //Chuỗi bí mật
                string vnpayData = Request.QueryString.Value;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (int s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s.ToString()) && s.ToString().StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s.ToString(), vnpayData[s].ToString());
                    }
                }

                string orderId = Request.Query["vnp_TxnRef"]; /*Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn*/
                string vnpayTranId = Request.Query["vnp_TransactionNo"];/*Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY*/
                string vnp_ResponseCode = Request.Query["vnp_ResponseCode"];/* pay.GetResponseData("vnp_ResponseCode");*/ //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = true;/* pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?*/

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        var cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
                        var login = SessionHelper.GetObjectFromJson<List<Login>>(HttpContext.Session, "login");
                        int tien = 35000;
                        int soluong = 0;
                        int giamgia = 0;
                        foreach (var i in cart)
                        {
                            giamgia = (((int?)i.SanPham.Tien).Value - (((int?)i.SanPham.Tien).Value * ((int?)i.SanPham.GiamGia).Value / 100)) * i.SoLuong;
                            tien = tien + giamgia;
                            soluong = soluong + i.SoLuong;
                        }

                        foreach (var i in login)
                        {

                            var kh = _context.KhachHangs.First(p => p.Email == i.khachHang.Email);
                            HoaDon hd = new HoaDon();
                            hd.MaKhachHang = kh.MaKhachHang;
                            hd.NgayLap = DateTime.Now;
                            hd.TongTien = tien;
                            hd.SoLuong = soluong;
                            hd.DiaChi = i.khachHang.DiaChi;
                            hd.Sdt = i.khachHang.Sdt;
                            hd.MaThanhToan = 2;
                            hd.MaVanChuyen = 1;
                            _context.HoaDons.Add(hd);
                            _context.SaveChanges();

                            int MaHoadon = hd.MaHoaDon;
                            List<ChiTietHoaDon> chitiethoadon = new List<ChiTietHoaDon>();
                            foreach (var item in cart)
                            {
                                ChiTietHoaDon cthd = new ChiTietHoaDon();
                                cthd.MaHoaDon = MaHoadon;
                                cthd.MaSanPham = item.SanPham.MaSanPham;
                                cthd.SoLuong = item.SoLuong;
                                cthd.TongTien = ((int?)item.SanPham.Tien).Value - (((int?)item.SanPham.Tien).Value * ((int?)item.SanPham.GiamGia).Value / 100);
                                chitiethoadon.Add(cthd);
                            }
                            _context.ChiTietHoaDons.AddRange(chitiethoadon);
                            _context.SaveChanges();
                            SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart").Clear();
                            List<GioHang> gioHangs = new List<GioHang>();
                            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", gioHangs);                          
                            //Thanh toán thành công
                        }   
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId;
                        ViewBag.Id = " Mã giao dịch: " + vnpayTranId ;
                        ViewBag.Alert = "Giao dịch thành công";
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn: " + orderId;
                        ViewBag.Id = " Mã giao dịch: " + vnpayTranId;
                        ViewBag.Alert = "Giao dịch thất bại";
                        switch (vnp_ResponseCode)
                        {
                            case "24":
                                ViewBag.Error = "Giao dịch không thành công do: Khách hàng hủy giao dịch";
                                break;
                            case "51":
                                ViewBag.Error = "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.";
                                break;
                            case "11":
                                ViewBag.Error = "Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch.";
                                break;
                            case "12":
                                ViewBag.Error = "Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch.";
                                break;
                            case "13":
                                ViewBag.Error = "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch.";
                                break;
                            default:
                                ViewBag.Error = "Giao dịch không thành công | Mã lỗi: " + vnp_ResponseCode;
                                break;
                        }                      
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                    ViewBag.Alert = "Giao dịch thất bại";
                }
            }

            return View();
        }


    }
}
