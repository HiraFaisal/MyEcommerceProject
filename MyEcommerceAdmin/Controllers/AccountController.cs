using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEcommerceAdmin.Models;
using System.IO;

namespace MyEcommerceAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-PFD42FH\\SQLEXPRESS;Initial Catalog=R49_MyEcommerceDB;Integrated Security=True";

        // GET: Account
        public ActionResult Index()
        {
            var usr = GetUserById(TempShpData.UserID);

            return View(usr);
        }

       

        // LOG IN
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection formColl)
        {
            string usrName = formColl["UserName"];
            string Pass = formColl["Password"];

            if (ModelState.IsValid)
            {
                var cust = ValidateUser(usrName, Pass);

                if (cust != null)
                {
                    TempShpData.UserID = cust.CustomerID;
                    Session["username"] = cust.UserName;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        // LOG OUT
        public ActionResult Logout()
        {
            Session["username"] = null;
            TempShpData.UserID = 0;
            TempShpData.items = null;
            return RedirectToAction("Index", "Home");
        }

        public Customer GetUser(string _usrName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE UserName = @UserName", conn);
                cmd.Parameters.AddWithValue("@UserName", _usrName);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerID = (int)reader["CustomerID"],
                            First_Name = reader["First_Name"].ToString(),
                            Last_Name = reader["Last_Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateofBirth = reader["DateofBirth"] != DBNull.Value ? (DateTime?)reader["DateofBirth"] : null,
                            Country = reader["Country"].ToString(),
                            City = reader["City"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            PicturePath = reader["PicturePath"].ToString(),
                            
                        };
                    }
                }
            }
            return null;
        }

        private Customer GetUserById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE CustomerID = @CustomerID", conn);
                cmd.Parameters.AddWithValue("@CustomerID", userId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerID = (int)reader["CustomerID"],
                            First_Name = reader["First_Name"].ToString(),
                            Last_Name = reader["Last_Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateofBirth = reader["DateofBirth"] != DBNull.Value ? (DateTime?)reader["DateofBirth"] : null,
                            Country = reader["Country"].ToString(),
                            City = reader["City"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            PicturePath = reader["PicturePath"].ToString(),
                           
                        };
                    }
                }
            }
            return null;
        }

        private Customer ValidateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE UserName = @UserName AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerID = (int)reader["CustomerID"],
                            First_Name = reader["First_Name"].ToString(),
                            Last_Name = reader["Last_Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateofBirth = reader["DateofBirth"] != DBNull.Value ? (DateTime?)reader["DateofBirth"] : null,
                            Country = reader["Country"].ToString(),
                            City = reader["City"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString(),
                            PicturePath = reader["PicturePath"].ToString(),
                            
                        };
                    }
                }
            }
            return null;
        }

        // UPDATE CUSTOMER DATA
        [HttpPost]
        public ActionResult Update(Customer cust)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateCustomer", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerID", cust.CustomerID);
                    cmd.Parameters.AddWithValue("@First_Name", cust.First_Name);
                    cmd.Parameters.AddWithValue("@Last_Name", cust.Last_Name);
                    cmd.Parameters.AddWithValue("@UserName", cust.UserName);
                    cmd.Parameters.AddWithValue("@Password", cust.Password);
    
                    cmd.Parameters.AddWithValue("@DateofBirth", cust.DateofBirth);
                    cmd.Parameters.AddWithValue("@Country", cust.Country);
                    cmd.Parameters.AddWithValue("@City", cust.City);
                    cmd.Parameters.AddWithValue("@PostalCode", cust.PostalCode);
                    cmd.Parameters.AddWithValue("@Email", cust.Email);
                    cmd.Parameters.AddWithValue("@Phone", cust.Phone);
                    cmd.Parameters.AddWithValue("@Address", cust.Address);
                  

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Session["username"] = cust.UserName;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // CREATE: Customer
        public ActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCustomer(CustomerVM cvm)
        {
            if (ModelState.IsValid)
            { 
                if (cvm.Picture != null)
                {
                    string filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(cvm.Picture.FileName));
                    cvm.Picture.SaveAs(Server.MapPath(filePath));

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("InsertCustomer", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@First_Name", cvm.First_Name);
                        cmd.Parameters.AddWithValue("@Last_Name", cvm.Last_Name);
                        cmd.Parameters.AddWithValue("@UserName", cvm.UserName);
                        cmd.Parameters.AddWithValue("@Password", cvm.Password);
                        cmd.Parameters.AddWithValue("@Gender", cvm.Gender);
                        cmd.Parameters.AddWithValue("@DateofBirth", cvm.DateofBirth);
                        cmd.Parameters.AddWithValue("@Country", cvm.Country);
                        cmd.Parameters.AddWithValue("@City", cvm.City);
                        cmd.Parameters.AddWithValue("@PostalCode", cvm.PostalCode);
                        cmd.Parameters.AddWithValue("@Email", cvm.Email);
                        cmd.Parameters.AddWithValue("@Phone", cvm.Phone);
                        cmd.Parameters.AddWithValue("@Address", cvm.Address);
                        cmd.Parameters.AddWithValue("@PicturePath", filePath);
                       

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("Login", "Account");
                    }

                    
                }
            }
            return PartialView("_Error");
        }
    }
}
