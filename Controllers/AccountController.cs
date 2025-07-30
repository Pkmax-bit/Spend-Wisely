using Chitieu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Chitieu.ViewModel; // đúng namespace ViewModel
using Chitieu.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Chitieu.ViewModel;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                Password = model.Password, // Khuyến nghị: nên hash
                Kichhoat = false,
                Random = Guid.NewGuid().ToString()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            SendActivationEmail(user);

            ViewBag.Message = "Vui lòng kiểm tra email để kích hoạt tài khoản!";
            return View();
        }

        return View(model);
    }

    private void SendActivationEmail(User user)
    {
        string from = "phannguyendangkhoa0915@gmail.com";
        string to = user.Email;
        string subject = "Kích hoạt tài khoản";
        string body = $"Chào bạn,<br/><br/>" +
                      $"Vui lòng nhấn vào liên kết bên dưới để kích hoạt tài khoản của bạn:<br/>" +
                      $"<a href=\"https://localhost:7185/Account/Activate?email={user.Email}&code={user.Random}\">Kích hoạt tài khoản</a><br/><br/>" +
                      $"Trân trọng.";

        MailMessage mail = new MailMessage(from, to, subject, body);
        mail.IsBodyHtml = true;

        var smtp = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(from, "qspqzngjncjtmrtc"),
            EnableSsl = true
        };

        smtp.Send(mail);
    }

    public IActionResult Activate(string email, string code)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || user.Random != code)
        {
            ViewBag.Success = false;
        }
        else if (user.Kichhoat == true)
        {
            ViewBag.AlreadyActivated = true;
        }
        else
        {
            user.Kichhoat = true;
            _context.SaveChanges();
            ViewBag.Success = true;
        }

        return View(); // => Views/Account/Activate.cshtml
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
        if (user == null)
        {
            ModelState.AddModelError("", "Sai thông tin đăng nhập.");
            return View(model);
        }

        if (user.Kichhoat != true)
        {
            ModelState.AddModelError("", "Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email.");
            return View(model);
        }

        HttpContext.Session.SetString("Email", user.Email);
        return RedirectToAction("Index", "Home");
    }
}
