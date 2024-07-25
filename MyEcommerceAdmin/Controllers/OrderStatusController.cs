using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MyEcommerceAdmin.StatePattern;
namespace MyEcommerceAdmin.Controllers
{
    public class OrderStatusController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-PFD42FH\\SQLEXPRESS;Initial Catalog=R49_MyEcommerceDB;Integrated Security=True";
        private IOrderStatusState _currentState;

        public OrderStatusController()
        {
            // Initialize with pending state
            _currentState = new PendingState(_connectionString);
        }

        // GET: OrderStatus/Index
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT OrderID, OrderDate, OrderAmount, Status FROM OrderStatus";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dt);
            }

            return View(dt);
        }

        // POST: OrderStatus/Approve/5
        [HttpPost]
        public ActionResult Approve(int id)
        {
            _currentState.Approve(id);
            return RedirectToAction("Index");
        }

        // POST: OrderStatus/Cancel/5
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            _currentState.Cancel(id);
            return RedirectToAction("Index");
        }
    }
}
