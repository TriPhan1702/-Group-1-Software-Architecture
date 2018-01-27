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
            var productList = from p in Context.Products where p.isActive select p;

            var productListModelView = new ProductListlViewModel()
            {
                Products = productList.ToList()
            };

            return View(productListModelView);
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