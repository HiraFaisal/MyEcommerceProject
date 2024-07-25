using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEcommerceAdmin.Models;


namespace MyEcommerceAdmin.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MyEcommerceDbContext db;

        public ProfileController()
        {
            this.db = new MyEcommerceDbContext();
        }

        // GET: Profile
        public ActionResult Index()
        {
            // Retrieve EmpID from TempData
            int? empId = TempData["EmpID"] as int?;

            if (empId == null)
            {
                return HttpNotFound("Employee ID not found in TempData");
            }

            // Find the employee by EmpID in the database
            var employee = db.admin_Employee.Find(empId);

            if (employee == null)
            {
                return HttpNotFound("Employee not found");
            }

            return View(employee);
        }
    }
}
