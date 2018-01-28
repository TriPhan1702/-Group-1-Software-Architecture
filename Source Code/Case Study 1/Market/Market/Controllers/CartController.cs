using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class CartController : Controller
    {
        public ActionResult ViewCart()
        {
            return View();
        }
    }
}