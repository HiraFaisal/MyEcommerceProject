using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEcommerceAdmin.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StocksReport()
        {
            try
            {
                string connectionString = @"Data Source=DESKTOP-PFD42FH\SQLEXPRESS;Initial Catalog=R49_MyEcommerceDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Products p INNER JOIN Categories c ON c.CategoryID = p.CategoryID INNER JOIN Suppliers s ON s.SupplierID = p.SupplierID", con);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Products");
                    ReportDocument rd = LoadReport("~/Reports/Stocks.rpt");
                    rd.SetDataSource(ds);
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }
            catch (Exception ex)
            {
                return Content("<h2>Error: " + ex.Message + "</h2>", "text/html");
            }
        }

        public ActionResult CustomersReport()
        {
            try
            {
                string connectionString = @"Data Source=DESKTOP-PFD42FH\SQLEXPRESS;Initial Catalog=R49_MyEcommerceDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Customers", con);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Customers");
                    ReportDocument rd = LoadReport("~/Reports/Customers.rpt");
                    rd.SetDataSource(ds);
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }
            catch (Exception ex)
            {
                return Content("<h2>Error: " + ex.Message + "</h2>", "text/html");
            }
        }

        public ActionResult SalesReport()
        {
            try
            {
                string connectionString = @"Data Source=DESKTOP-PFD42FH\SQLEXPRESS;Initial Catalog=R49_MyEcommerceDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM OrderDetails od INNER JOIN Orders o ON o.OrderID = od.OrderID INNER JOIN Products p ON p.ProductID = od.ProductID", con);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "OrderDetails");
                    ReportDocument rd = LoadReport("~/Reports/Sales.rpt");
                    rd.SetDataSource(ds);
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);
                }
            }
            catch (Exception ex)
            {
                return Content("<h2>Error: " + ex.Message + "</h2>", "text/html");
            }
        }

        private ReportDocument LoadReport(string reportPath)
        {
            ReportDocument rd = new ReportDocument();
            string rptPath = Server.MapPath(reportPath);
            rd.Load(rptPath);
            return rd;
        }
    }
    }




    
