using Chitieu.Data;
using Chitieu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace Chitieu.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateProfile(User model, IFormFile? HinhanhFile)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound();

            user.Hoten = model.Hoten;
            user.Sdt = model.Sdt;

            if (HinhanhFile != null && HinhanhFile.Length > 0)
            {
                var fileName = Path.GetFileName(HinhanhFile.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    HinhanhFile.CopyTo(stream);
                }
                user.Hinhanh = "/uploads/" + fileName;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddFamilyMember()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFamilyMember(NguoiChiTieu model, IFormFile? HinhanhFile)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return RedirectToAction("Login", "Account");

            model.IdUser = user.Id;

            if (HinhanhFile != null && HinhanhFile.Length > 0)
            {
                var fileName = Path.GetFileName(HinhanhFile.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    HinhanhFile.CopyTo(stream);
                }
                model.Hinhanh = "/uploads/" + fileName;
            }

            _context.NguoiChiTieus.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
