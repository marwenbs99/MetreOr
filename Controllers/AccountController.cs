using MetreOr.Data;
using MetreOr.Models;
using MetreOr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

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
            if (!ModelState.IsValid)
            {
                TempData["Statut"] = "Warning";
                TempData["NotificationMessage"] = $"Assurez-vous de remplir correctement le formulaire!";
                ModelState.AddModelError("", "Invalid logout attempt.");
                return View(newUser);
            }

            if (!newUser.VerifyForm().FirstOrDefault().Key)
            {
                TempData["Statut"] = "Warning";
                TempData["NotificationMessage"] = newUser.VerifyForm().FirstOrDefault().Value;
                ModelState.AddModelError("", "Invalid logout attempt.");
                return View(newUser);
            }
            
            if (IsEmailUsed(newUser.Email))
            {
                TempData["Statut"] = "Warning";
                TempData["NotificationMessage"] = $"Un compte est déjà inscrit avec cet email. Veuillez en utiliser un autre..";
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
                DateOfInscription = DateTime.Now,
            };

            _context.AppUsers.Add(userSignup);
            _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }
        private bool IsEmailUsed(string email)
        {
            if (_context.AppUsers.Where(x => x.Email.Equals(email)).Any())
            {
                return true;
            }
            return false;
        }
    }

}
