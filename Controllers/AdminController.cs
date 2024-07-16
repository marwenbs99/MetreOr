using MetreOr.Data;
using MetreOr.ViewModels.AdminVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
