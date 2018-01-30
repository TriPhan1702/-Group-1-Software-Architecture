using System;
using System.Linq;
using System.Web.Mvc;
using Market.Models;
using Market.ModelView;

namespace Market.Controllers
{
    public class OrderController : Controller
    {
        private readonly MarketDBEntities Context;

        public OrderController()
        {
            Context = new MarketDBEntities();
        }

        // GET: Order
        public ActionResult Checkout()
        {
            var account = Session["CurrentUser"] as AccountViewModel;

            var cart = Session["cart"] as CartViewModel;

            if (account == null)
            {
                TempData["message"] = "<script>alert('You are not logged in')</script>";
                return RedirectToAction("Index", "Home");
            }

            if (cart == null || cart.Items.Count <= 0)
            {
                TempData["message"] = "<script>alert('Your Cart is Empty')</script>";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Paypal()
        {
            var account = Session["CurrentUser"] as AccountViewModel;

            var cart = Session["cart"] as CartViewModel;

            if (account == null)
            {
                TempData["message"] = "<script>alert('You are not logged in')</script>";
                return RedirectToAction("Index", "Home");
            }

            if (cart == null || cart.Items.Count <= 0)
            {
                TempData["message"] = "<script>alert('Your Cart is Empty')</script>";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var accountFromDatabase = Context.Users.Single(a => a.username.Equals(account.Username) && a.isActive);

                var order = new Order();

                order.User = accountFromDatabase;

                decimal sumMoney = 0;

                foreach (var item in cart.Items)
                {
                    sumMoney += item.Quantity * item.PricePerUnit;

                    order.Orders_Has_Products.Add(new Orders_Has_Products
                    {
                        Order = order,
                        productId = item.ProductId,
                        pricePerUnit = item.PricePerUnit,
                        quantity = item.Quantity
                    });
                }

                order.sumMoney = sumMoney;
                order.paymentTypeId = PaymentType.PayPalPaymentMethodId;

                Context.Orders.Add(order);

                Context.SaveChanges();

                Session["cart"] = null;

                return View();
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Check failed, please try again')</script>";
                return View("Checkout");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreditCard(CheckoutViewModel form)
        {
            if (!ModelState.IsValid) return View("Checkout", form);

            var account = Session["CurrentUser"] as AccountViewModel;

            var cart = Session["cart"] as CartViewModel;

            if (account == null)
            {
                TempData["message"] = "<script>alert('You are not logged in')</script>";
                return RedirectToAction("Index", "Home");
            }

            if (cart == null || cart.Items.Count <= 0)
            {
                TempData["message"] = "<script>alert('Your Cart is Empty')</script>";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var accountFromDatabase = Context.Users.Single(a => a.username.Equals(account.Username) && a.isActive);

                var order = new Order();

                order.User = accountFromDatabase;

                decimal sumMoney = 0;

                foreach (var item in cart.Items)
                {
                    sumMoney += item.Quantity * item.PricePerUnit;

                    order.Orders_Has_Products.Add(new Orders_Has_Products
                    {
                        Order = order,
                        productId = item.ProductId,
                        pricePerUnit = item.PricePerUnit,
                        quantity = item.Quantity
                    });
                }

                order.sumMoney = sumMoney;
                order.paymentTypeId = PaymentType.CardPaymentMethodId;

                Context.Orders.Add(order);

                Context.SaveChanges();

                Session["cart"] = null;

                return View();
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Checkout failed, please try again')</script>";
                return View("Checkout", form);
            }
        }
    }
}