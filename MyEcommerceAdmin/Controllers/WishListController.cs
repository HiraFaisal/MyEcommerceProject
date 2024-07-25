using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyEcommerceAdmin.CommandPattern;
using MyEcommerceAdmin.Models;

namespace MyEcommerceAdmin.Controllers
{
    public class WishListController : Controller
    {
        private readonly MyEcommerceDbContext _db = new MyEcommerceDbContext();

        // Command for adding item to cart
   

        public ActionResult Index()
        {
            this.GetDefaultData();

            var wishlistProducts = _db.Wishlists.Where(x => x.CustomerID == TempShpData.UserID).ToList();
            return View(wishlistProducts);
        }

        public ActionResult Remove(int id)
        {
            ICommand removeFromWishlistCommand = new RemoveFromWishlistCommand(id, _db);
            removeFromWishlistCommand.Execute();

            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int id)
        {
            ICommand addToCartCommand = new AddToCartCommand(id, _db);
            addToCartCommand.Execute();

            return Redirect(TempData["returnURL"]?.ToString() ?? "~/Home/Index");
        }
    }
}
