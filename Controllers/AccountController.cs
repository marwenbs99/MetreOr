using MetreOr.Data;
using MetreOr.Models;
using MetreOr.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace MetreOr.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            _webHostEnvironment = webHostEnvironment;
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
            newUser.Password = passwordHasher.HashPassword(newUser, newUser.Password);
            newUser.RepeatPassword = string.Empty;

            return RedirectToAction("Verify", "Account", newUser);
        }

        [HttpGet]
        [ActionName("Verify")]
        public IActionResult VerifyGet(IdentityVerificationViewModel newUser)
        {
            return View(newUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(IdentityVerificationViewModel identity, string? returnURL)
        {
            if (!ModelState.IsValid || !identity.AreFilesValid())
            {
                TempData["Statut"] = "Warning";
                TempData["NotificationMessage"] = $"Les fichiers téléchargés pour la carte d identité recto et verso doivent être de type image.";
                return LocalRedirect(returnURL);
            }

            var guid = Guid.NewGuid();
            string uploadsFolder = _webHostEnvironment.WebRootPath + "\\assets\\CID\\" + guid;
            bool isSaved = await SaveCID(uploadsFolder, identity.CIDRecto, identity.CIDVerso);

            if (isSaved)
            {
                AppUser userSignup = new()
                {
                    Email = identity.Email,
                    FirstName = identity.FirstName[0].ToString().ToUpper() + identity.FirstName.Substring(1),
                    LastName = identity.LastName,
                    PhoneNumber = identity.PhoneNumber,
                    Password = identity.Password,
                    Adresse = identity.Adresse,
                    Birthday = identity.Birthday,
                    IsVerified = false,
                    DateOfInscription = DateTime.Now,
                    Guid = guid,
                };

                _context.AppUsers.Add(userSignup);
                _context.SaveChanges();

                return RedirectToAction("SignupComplet", "Account", new { userName = identity.FirstName });
            }
            else
            {
                return View(identity);
            }
        }

        public IActionResult SignupComplet(string userName)
        {
            return View(new SignupCompletViewModel { UserName = userName });
        }

        private bool IsEmailUsed(string email)
        {
            if (_context.AppUsers.Where(x => x.Email.Equals(email)).Any())
            {
                return true;
            }
            return false;
        }

        private async Task<bool> SaveCID(string path, IFormFile CIDRecto, IFormFile CIDVerso)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                // Save recto
                string uniqueFileNameRecto = Guid.NewGuid().ToString() + "_" + CIDRecto.FileName;
                string filePathRecto = Path.Combine(path, uniqueFileNameRecto);
                using (var fileStream = new FileStream(filePathRecto, FileMode.Create))
                {
                    await CIDRecto.CopyToAsync(fileStream);
                }

                // Save verso
                string uniqueFileNameVerso = Guid.NewGuid().ToString() + "_" + CIDVerso.FileName;
                string filePathVerso = Path.Combine(path, uniqueFileNameVerso);
                using (var fileStream = new FileStream(filePathVerso, FileMode.Create))
                {
                    await CIDVerso.CopyToAsync(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
