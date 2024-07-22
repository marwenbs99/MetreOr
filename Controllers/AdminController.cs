using MetreOr.Data;
using MetreOr.Enum;
using MetreOr.Models;
using MetreOr.ViewModels.AdminVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;
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

            var CheckList = new List<CheckBoxItem>
            {
                new CheckBoxItem { Key= "Prénom correct ?", Value= false },
                new CheckBoxItem { Key= "Nom correct ?", Value= false  },
                new CheckBoxItem { Key="Date de naissance correcte ?",Value=  false  },
                new CheckBoxItem { Key="Adresse correcte ?", Value= false  },
                new CheckBoxItem {  Key="Email format correct ?", Value= false  },
                new CheckBoxItem{ Key="Numéro de téléphone correct ?", Value= false  },
            };

            var currentUserToVerifyVM = new CurrentUserToVerifyViewModel
            {
                Email = currentUser.Email,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Adresse = currentUser.Adresse,
                UserGuid = currentUser.Guid,
                PhoneNumber = currentUser.PhoneNumber,
                Birthday = currentUser.Birthday,
                CheckBoxes = CheckList,

            };

            return View(currentUserToVerifyVM);
        }

        [HttpPost]
        public ActionResult ConfirmUser(CurrentUserToVerifyViewModel userToVerify)
        {
            var currentUser = _context.AppUsers.FirstOrDefault(x => x.Guid == userToVerify.UserGuid);
            switch (userToVerify.ActionType)
            {
                case BtnActionType.corriger:                    
                    currentUser.FirstName = userToVerify.FirstName;
                    currentUser.LastName = userToVerify.LastName;
                    currentUser.Adresse = userToVerify.Adresse;
                    _context.Update(currentUser);
                    _context.SaveChanges();
                    return View(userToVerify);
                case BtnActionType.envoyer:
                    if(   userToVerify.CheckBoxes.Where(x => x.Value.Equals(true)).Count() == userToVerify.CheckBoxes.Count()  ) 
                    {
                        currentUser.IsVerified = true;
                        _context.AppUsers.Update(currentUser);
                        _context.SaveChanges();
                    }
                    return RedirectToAction("Index");
            }
            return View();
        }
    }
}
