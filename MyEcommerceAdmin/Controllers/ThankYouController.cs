using System;
using System.Linq;
using System.Web.Mvc;
using MyEcommerceAdmin.Models;
using MyEcommerceAdmin.Observer_Pattern;

namespace MyEcommerceAdmin.Controllers
{
    public class ThankYouController : Controller
    {
        private readonly MyEcommerceDbContext _dbContext;
        private OrderSubject _orderSubject;

        // Constructor with dependency injection
        public ThankYouController(MyEcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
            _orderSubject = new OrderSubject();
            _orderSubject.Attach(new EmailNotificationObserver());
        }

        // Parameterless constructor
        public ThankYouController() : this(new MyEcommerceDbContext())
        {
        }

        // GET: ThankYou
        public ActionResult Index(int orderID)
        {
            // Get order details from the database
            var order = _dbContext.Orders.Include("ShippingDetail").Include("Payment").FirstOrDefault(o => o.OrderID == orderID);

            if (order == null)
            {
                // Handle order not found scenario
                return HttpNotFound();
            }

            // Notify observers (send email)
            _orderSubject.Notify(order);

            // Reset cart and session data
            ViewBag.cartBox = null;
            ViewBag.Total = null;
            ViewBag.NoOfItem = null;
            TempShpData.items = null;

            return View("ThankYou");
        }
    }
}
