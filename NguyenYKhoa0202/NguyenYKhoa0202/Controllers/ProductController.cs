using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenYKhoa0202.Controllers
{
    public class ProductController : Controller
    {
        NorthwindDataContext da = new NorthwindDataContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListProduct()
        {
            var listProduct = da.Products.OrderByDescending(s=>s.ProductID).ToList();
            return View(listProduct);
        }
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID==id);
            return View(p);
        }

        public ActionResult Create()
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Product p)
        {
            try
            {
                //Tạo mới 1 sp Product
                Product produce = new Product();
                produce = p;
                //Gắn các thuộc tính cần thêm
                produce.SupplierID = int.Parse(collection["NCC"]);
                produce.CategoryID = int.Parse(collection["LSP"]);
                //Xử lý thêm 
                da.Products.InsertOnSubmit(produce);
                //Cập nhật thay đổi xuống db
                da.SubmitChanges();
                //Hiển thị lại db
                return RedirectToAction("ListProduct");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            var p = da.Products.FirstOrDefault(e => e.ProductID == id);
            return View(p);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                //Lấy product muốn sửa
                var product = da.Products.FirstOrDefault(s => s.ProductID == id);
                //Gán các thuôc tính cần sửa
                product.ProductName = collection["ProductName"];
                product.UnitPrice = decimal.Parse(collection["UnitPrice"]);
                product.SupplierID = int.Parse(collection["NCC"]);
                product.CategoryID = int.Parse(collection["LSP"]);
                product.UnitsInStock = short.Parse(collection["UnitsInStock"]);

                //Cập nhật thay đổi
                da.SubmitChanges(); 

                return RedirectToAction("ListProduct");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);

            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var p = da.Products.FirstOrDefault(d => d.ProductID == id);
            return View(p);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var delp = da.Products.First(d => d.ProductID == id);
                da.Products.DeleteOnSubmit(delp);
                da.SubmitChanges();
                return RedirectToAction("ListProduct");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }
    }
}
