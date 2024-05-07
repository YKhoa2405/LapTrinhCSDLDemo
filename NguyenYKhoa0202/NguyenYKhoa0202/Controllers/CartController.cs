using NguyenYKhoa0202.Models;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using TransactionScope = System.Transactions.TransactionScope;

namespace NguyenYKhoa0202.Controllers
{
    public class CartController : Controller
    {
        NorthwindDataContext da = new NorthwindDataContext();

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

        public ActionResult OrderProduct()
        { 
            using(TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    da.Orders.InsertOnSubmit(order);
                    da.SubmitChanges();

                    List<CartModel> carts = GetListCarts();
                    foreach(var item in carts)
                    {
                        Order_Detail d = new Order_Detail();
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quantity.ToString());
                        d.UnitPrice = decimal.Parse(item.UnitPrice.ToString());
                        d.Discount = 0;

                        da.Order_Details.InsertOnSubmit(d);

                    }
                    da.SubmitChanges();
                    tranScope.Complete();
                    Session["CartModel"] = null;
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");
                }
            }


            return RedirectToAction("OrderDetailsList");
        }

        public ActionResult OrderDetailsList()
        {
            var ds = da.Order_Details.OrderByDescending(s => s.OrderID).ToList();
            return View(ds);
        }



    }
}
