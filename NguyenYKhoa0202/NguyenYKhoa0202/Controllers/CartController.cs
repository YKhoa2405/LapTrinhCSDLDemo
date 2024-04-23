using NguyenYKhoa0202.Models;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenYKhoa0202.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public List<CartModel> GetListCarts()
        {
            List<CartModel> carts = Session["CartModel"] as List<CartModel>;
            if(carts == null)
            {
                carts = new List<CartModel>();
                Session["CartModel"] = carts;
            }
            return carts;
        }

        public ActionResult ListCarts()
        {
            List<CartModel> carts = GetListCarts();
            ViewBag.CountProduct = carts.Sum(s => s.Quantity);
            ViewBag.Total = carts.Sum(s => s.Total);
            return View(carts);
        }

        public ActionResult AddToCart(int id)
        {
            List<CartModel> carts = GetListCarts();

            CartModel c = carts.Find(s => s.ProductID == id);
            if (c != null)
            {   
                c.Quantity++;
            }
            else
            {
                c = new CartModel(id);
                carts.Add(c);
            }

            return RedirectToAction("ListCarts");
        }

    }
}
