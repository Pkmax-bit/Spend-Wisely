﻿using Chitieu.Data;
using Chitieu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Chitieu.ViewModel;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;

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

            var randomCode = Guid.NewGuid().ToString();
            string hashedPassword = HashPassword(model.Password, randomCode);

            var user = new User
            {
                Email = model.Email,
                Password = hashedPassword,
                Kichhoat = false,
                Random = randomCode
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

        return View();
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Sai thông tin đăng nhập.");
            return View(model);
        }

        string hashedInput = HashPassword(model.Password, user.Random ?? "");

        if (user.Password != hashedInput)
        {
            ModelState.AddModelError("", "Sai thông tin đăng nhập.");
            return View(model);
        }

        if (user.Kichhoat != true)
        {
            ModelState.AddModelError("", "Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email.");
            return View(model);
        }

        // Set session
        HttpContext.Session.SetString("Email", user.Email);

        // Create claims for authentication
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Email)
    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        // Sign in the user
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        user.Lastlogin = DateTime.Now;
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private string HashPassword(string password, string random)
    {
        using (var sha256 = SHA256.Create())
        {
            var combined = password + random;
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}