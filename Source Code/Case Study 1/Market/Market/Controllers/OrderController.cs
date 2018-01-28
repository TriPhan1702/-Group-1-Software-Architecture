using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Market.Models;

namespace Market.Controllers
{
    public class OrderController : Controller
    {
        private MarketDBEntities Context;

        public OrderController()
        {
            Context = new MarketDBEntities();
        }
        // GET: Order
        public ActionResult Checkout()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Something went wrong, please try again')</script>";

                return RedirectToAction("ViewCart","Cart");
            }
        }
    }
}