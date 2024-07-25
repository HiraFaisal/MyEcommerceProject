using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEcommerceAdmin.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyEcommerceAdmin.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-PFD42FH\\SQLEXPRESS;Initial Catalog=R49_MyEcommerceDB;Integrated Security=True";

        MyEcommerceDbContext db = new MyEcommerceDbContext();
        // GET: CheckOut
        public ActionResult Index()
        {
            ViewBag.PayMethod = new SelectList(db.PaymentTypes, "PayTypeID", "TypeName");


            var data = this.GetDefaultData();

            return View(data);
        }


        //PLACE ORDER--LAST STEP
        public ActionResult PlaceOrder(FormCollection getCheckoutDetails)
        {

            int shpID = 1;
            if (db.ShippingDetails.Count() > 0)
            {
                shpID = db.ShippingDetails.Max(x => x.ShippingID) + 1;
            }
            int payID = 1;
            if (db.Payments.Count() > 0)
            {
                payID = db.Payments.Max(x => x.PaymentID) + 1;
            }
            int orderID = 1;
            if (db.Orders.Count() > 0)
            {
                orderID = db.Orders.Max(x => x.OrderID) + 1;
            }



            ShippingDetail shpDetails = new ShippingDetail();
            shpDetails.ShippingID = shpID;
            shpDetails.FirstName = getCheckoutDetails["FirstName"];
            shpDetails.LastName = getCheckoutDetails["LastName"];
            shpDetails.Email = getCheckoutDetails["Email"];
            shpDetails.Mobile = getCheckoutDetails["Mobile"];
            shpDetails.Address = getCheckoutDetails["Address"];
            shpDetails.City = getCheckoutDetails["City"];
            shpDetails.PostCode = getCheckoutDetails["PostCode"];
            db.ShippingDetails.Add(shpDetails);
            db.SaveChanges();

            Payment pay = new Payment();
            pay.PaymentID = payID;
            pay.Type = Convert.ToInt32(getCheckoutDetails["PayMethod"]);
            db.Payments.Add(pay);
            db.SaveChanges();

            Order o = new Order();
            o.OrderID = orderID;
            o.CustomerID = TempShpData.UserID;
            o.PaymentID = payID;
            o.ShippingID = shpID;
            o.Discount = Convert.ToInt32(getCheckoutDetails["discount"]);
            o.TotalAmount = Convert.ToInt32(getCheckoutDetails["totalAmount"]);
            o.isCompleted = true;
            o.OrderDate = DateTime.Now;
            db.Orders.Add(o);
            db.SaveChanges();

            foreach (var OD in TempShpData.items)
            {
                OD.OrderID = orderID;
                OD.Order = db.Orders.Find(orderID);
                OD.Product = db.Products.Find(OD.ProductID);
                db.OrderDetails.Add(OD);
                db.SaveChanges();
            }


            db.SaveChanges();
            string insertOrderStatusQuery = @"INSERT INTO OrderStatus (OrderID, OrderDate, OrderAmount, Status) VALUES (@OrderID, @OrderDate, @OrderAmount, @Status);";

            // Prepare parameters
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@OrderID", o.OrderID),
        new SqlParameter("@OrderDate", DateTime.Now),
        new SqlParameter("@OrderAmount", o.TotalAmount),
        new SqlParameter("@Status", "Pending")
    };

            // Execute the insert query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertOrderStatusQuery, connection);
                command.Parameters.AddRange(parameters.ToArray());

                connection.Open();
                command.ExecuteNonQuery();
            }

           db.SaveChanges();
            var orderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var customerName = getCheckoutDetails["FirstName"] + " " + getCheckoutDetails["LastName"];
            var customerEmail = getCheckoutDetails["Email"];
            var customerMobile = getCheckoutDetails["Mobile"];
            var customerAddress = getCheckoutDetails["Address"] + ", " +
                                  getCheckoutDetails["City"] + ", " + getCheckoutDetails["PostCode"];

            // Redirect to ThankYou action passing order details
            return RedirectToAction("Index", "ThankYou", new
            {
                orderID,
                orderDate,
                customerName,
                customerEmail,
                customerMobile,
                customerAddress
            });


        }

    }
}
