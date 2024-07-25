using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEcommerceAdmin.Models;
using System.Data.SqlClient;

namespace MyEcommerceAdmin.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyEcommerceDbContext db = new MyEcommerceDbContext();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Database.SqlQuery<Product>("SELECT * FROM Products").ToList();
            return View(products);
        }

        // CREATE: Product
        public ActionResult Create()
        {
            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;
                if (pvm.Picture != null)
                {
                    filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));
                }

                var query = @"INSERT INTO Products (Name, SupplierID, CategoryID, SubCategoryID, UnitPrice, OldPrice, Discount, UnitInStock, ProductAvailable, ShortDescription, PicturePath) 
                              VALUES (@Name, @SupplierID, @CategoryID, @SubCategoryID, @UnitPrice, @OldPrice, @Discount, @UnitInStock, @ProductAvailable, @ShortDescription, @PicturePath)";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", pvm.Name),
                    new SqlParameter("@SupplierID", pvm.SupplierID),
                    new SqlParameter("@CategoryID", pvm.CategoryID),
                    new SqlParameter("@SubCategoryID", pvm.SubCategoryID ?? (object)DBNull.Value),
                    new SqlParameter("@UnitPrice", pvm.UnitPrice),
                    new SqlParameter("@OldPrice", pvm.OldPrice ?? (object)DBNull.Value),
                    new SqlParameter("@Discount", pvm.Discount ?? (object)DBNull.Value),
                    new SqlParameter("@UnitInStock", pvm.UnitInStock ?? (object)DBNull.Value),
                    new SqlParameter("@ProductAvailable", pvm.ProductAvailable ?? (object)DBNull.Value),
                    new SqlParameter("@ShortDescription", pvm.ShortDescription),
                   
                    new SqlParameter("@PicturePath", filePath ?? (object)DBNull.Value)
                };

                db.Database.ExecuteSqlCommand(query, parameters);
                return PartialView("_Success");
            }

            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return PartialView("_Error");
        }

        // EDIT: Product
        public ActionResult Edit(int id)
        {
            var product = db.Database.SqlQuery<Product>("SELECT * FROM Products WHERE ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductVM pvm = new ProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                SubCategoryID = product.SubCategoryID,
                UnitPrice = product.UnitPrice,
                OldPrice = product.OldPrice,
                Discount = product.Discount,
                UnitInStock = product.UnitInStock,
                ProductAvailable = product.ProductAvailable,
                ShortDescription = product.ShortDescription,
                Note = product.Note,
                PicturePath = product.PicturePath
            };

            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        [HttpPost]
        public ActionResult Edit(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));
                }

                var query = @"UPDATE Products SET Name = @Name, SupplierID = @SupplierID, CategoryID = @CategoryID, SubCategoryID = @SubCategoryID, UnitPrice = @UnitPrice, OldPrice = @OldPrice, 
                              Discount = @Discount, UnitInStock = @UnitInStock, ProductAvailable = @ProductAvailable, ShortDescription = @ShortDescription,  PicturePath = @PicturePath
                              WHERE ProductID = @ProductID";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", pvm.ProductID),
                    new SqlParameter("@Name", pvm.Name),
                    new SqlParameter("@SupplierID", pvm.SupplierID),
                    new SqlParameter("@CategoryID", pvm.CategoryID),
                    new SqlParameter("@SubCategoryID", pvm.SubCategoryID ?? (object)DBNull.Value),
                    new SqlParameter("@UnitPrice", pvm.UnitPrice),
                    new SqlParameter("@OldPrice", pvm.OldPrice ?? (object)DBNull.Value),
                    new SqlParameter("@Discount", pvm.Discount ?? (object)DBNull.Value),
                    new SqlParameter("@UnitInStock", pvm.UnitInStock ?? (object)DBNull.Value),
                    new SqlParameter("@ProductAvailable", pvm.ProductAvailable ?? (object)DBNull.Value),
                    new SqlParameter("@ShortDescription", pvm.ShortDescription),
                
                    new SqlParameter("@PicturePath", filePath ?? (object)DBNull.Value)
                };

                db.Database.ExecuteSqlCommand(query, parameters);
                return RedirectToAction("Index");
            }

            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        // DETAILS: Product
        public ActionResult Details(int id)
        {
            var product = db.Database.SqlQuery<Product>("SELECT * FROM Products WHERE ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductVM pvm = new ProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                SubCategoryID = product.SubCategoryID,
                UnitPrice = product.UnitPrice,
                OldPrice = product.OldPrice,
                Discount = product.Discount,
                UnitInStock = product.UnitInStock,
                ProductAvailable = product.ProductAvailable,
                ShortDescription = product.ShortDescription,
                Note = product.Note,
                PicturePath = product.PicturePath
            };

            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        [HttpPost]
        public ActionResult Details(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));
                }

                var query = @"UPDATE Products SET Name = @Name, SupplierID = @SupplierID, CategoryID = @CategoryID, SubCategoryID = @SubCategoryID, UnitPrice = @UnitPrice, OldPrice = @OldPrice, 
                              Discount = @Discount, UnitInStock = @UnitInStock, ProductAvailable = @ProductAvailable, ShortDescription = @ShortDescription, Note = @Note, PicturePath = @PicturePath
                              WHERE ProductID = @ProductID";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", pvm.ProductID),
                    new SqlParameter("@Name", pvm.Name),
                    new SqlParameter("@SupplierID", pvm.SupplierID),
                    new SqlParameter("@CategoryID", pvm.CategoryID),
                    new SqlParameter("@SubCategoryID", pvm.SubCategoryID ?? (object)DBNull.Value),
                    new SqlParameter("@UnitPrice", pvm.UnitPrice),
                    new SqlParameter("@OldPrice", pvm.OldPrice ?? (object)DBNull.Value),
                    new SqlParameter("@Discount", pvm.Discount ?? (object)DBNull.Value),
                    new SqlParameter("@UnitInStock", pvm.UnitInStock ?? (object)DBNull.Value),
                    new SqlParameter("@ProductAvailable", pvm.ProductAvailable ?? (object)DBNull.Value),
                    new SqlParameter("@ShortDescription", pvm.ShortDescription),
                    new SqlParameter("@Note", pvm.Note),
                    new SqlParameter("@PicturePath", filePath ?? (object)DBNull.Value)
                };

                db.Database.ExecuteSqlCommand(query, parameters);
                return RedirectToAction("Index");
            }

            ViewBag.supplierList = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            ViewBag.categoryList = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.SubCategoryList = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            return View(pvm);
        }

        // DELETE: Product
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var product = db.Database.SqlQuery<Product>("SELECT * FROM Products WHERE ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var product = db.Database.SqlQuery<Product>("SELECT * FROM Products WHERE ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }

            string file_name = product.PicturePath;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }

            db.Database.ExecuteSqlCommand("DELETE FROM Products WHERE ProductID = @id", new SqlParameter("@id", id));
            return RedirectToAction("Index");
        }
    }
}
