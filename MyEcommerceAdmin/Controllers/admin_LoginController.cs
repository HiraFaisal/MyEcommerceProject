using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEcommerceAdmin.Models;
using System.Data.SqlClient;
using MyEcommerceAdmin.SingletonPattern;

namespace MyEcommerceAdmin.Controllers
{
    public class admin_LoginController : Controller
    {
        private readonly AdminLoginService loginService;
        private readonly MyEcommerceDbContext db;

        public admin_LoginController()
        {
            this.loginService = AdminLoginService.GetInstance();
            this.db = new MyEcommerceDbContext();
        }

        // GET: admin_Login
        public ActionResult Index()
        {
            ViewBag.AlertMessage = "Welcome to admin login!";
            return View();
        }

        [HttpPost]
        public ActionResult Login(admin_Login login)
        {
            if (ModelState.IsValid)
            {
                var result = loginService.ValidateUser(db, login.UserName, login.Password);

                if (result != null)
                {
                    Session["username"] = result.UserName;
                    TempData["EmpID"] = result.EmpID;
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return View();
        }

        // Logout Server Code
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "admin_Login");
        }
    }
}
