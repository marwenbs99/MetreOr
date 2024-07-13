using MetreOr.Data;
using MetreOr.Models;
using MetreOr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace MetreOr.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(SignUpViewModel newUser)
        {
            if (newUser.Password != newUser.RepeatPassword || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid logout attempt.");
                return View(newUser);
            }

            var passwordHasher = new PasswordHasher<AppUser>();

            AppUser userSignup = new()
            {
                Email = newUser.Email,
                FirstName = newUser.FirstName[0].ToString().ToUpper() + newUser.FirstName.Substring(1),
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,
                Password = newUser.Password,
                Adresse = newUser.Adresse,
                Birthday = newUser.Birthday,
                IsVerified = false,
                DateOfInscription = DateTime.Now
            };

            _context.AppUsers.Add(userSignup);
            _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }
    }
}
