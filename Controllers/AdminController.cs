using MetreOr.Data;
using MetreOr.Enum;
using MetreOr.Models;
using MetreOr.ViewModels.AdminVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetreOr.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            UserToVerifyViewModel verifyUserList = new UserToVerifyViewModel
            {
                UserToVerify = _context.AppUsers.Where(x => x.IsVerified.Equals(false)).ToList()
            };

            return View(verifyUserList);
        }

        public ActionResult ConfirmUser(Guid guidit)
        {
            var currentUser = _context.AppUsers.FirstOrDefault(x => x.Guid == guidit);
            if (currentUser == null)
            {
                return View();
            }
            var currentUserToVerifyVM = new CurrentUserToVerifyViewModel
            { 
                Email = currentUser.Email,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Adresse = currentUser.Adresse,
                UserGuid = currentUser.Guid,
                PhoneNumber = currentUser.PhoneNumber,
                Birthday = currentUser.Birthday,
            };

            return View(currentUserToVerifyVM);
        }

        [HttpPost]
        public ActionResult ConfirmUser(CurrentUserToVerifyViewModel userToVerify)
        {
            switch (userToVerify.ActionType)
            {
                case BtnActionType.corriger:
                        var currentUser = _context.AppUsers.FirstOrDefault(x => x.Guid == userToVerify.UserGuid);
                    currentUser.FirstName = userToVerify.FirstName;
                    currentUser.LastName = userToVerify.LastName;
                    currentUser.Adresse = userToVerify.Adresse;
                    _context.Update(currentUser);
                    _context.SaveChanges();
                    return View(userToVerify);
                case BtnActionType.envoyer:
                    return RedirectToAction("Index");
            }
            return View();
        }
    }
}
