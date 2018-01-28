using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Market.Models;
using Market.ModelView;

namespace Market.Controllers
{
    public class HomeController : Controller
    {

        private MarketDBEntities Context;

        public HomeController()
        {
            Context = new MarketDBEntities();
        }

        public ActionResult Index()
        {
            var productList = (from p in Context.Products where p.isActive select p).ToList();

            var productListModelView = new ProductListlViewModel()
            {
                Products = productList.ToList()
            };

            return View(productListModelView);
        }

        public ActionResult SearchProduct(string txtSearchValue)
        {
            var products = (from p in Context.Products
                where p.name.Contains(txtSearchValue) || p.shortDescription.Contains(txtSearchValue) ||
                      p.description.Contains(txtSearchValue)
                select p).ToList();

            var productListModelView = new ProductListlViewModel()
            {
                Products = products.ToList()
            };
            return View("Index", productListModelView);
        }

        public ActionResult AddProductToCart(int productId, bool toHomePage)
        {
            var product = Context.Products.FirstOrDefault(p => p.id == productId);

            if (product == null)
            {
                TempData["message"] = "<script>alert('Item not found')</script>";
            }
            else
            {
                var cart = Session["cart"] as CartViewModel;

                if (cart == null)
                {
                    cart = new CartViewModel();

                    cart.AddItemToCart(product);
                }
                else
                {
                    var item = cart.Items.Find(i => i.ProductId == product.id);

                    if (item == null)
                    {
                        cart.AddItemToCart(product);
                    }
                    else
                    {
                        item.Quantity++;
                    }
                }

                Session["cart"] = cart;

                TempData["message"] = "<script>alert('Item Added')</script>";
            }
            
            return toHomePage ? RedirectToAction("Index") : RedirectToAction("ProductDetail", "Product", new {productId = product.id});
        }

        public ActionResult RemoverItemFromCart(int productId)
        {
            var cart = Session["cart"] as CartViewModel;

            cart?.RemoveItemFromCart(productId);

            return RedirectToAction("ViewCart");
        }

        public ActionResult ViewCart()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}