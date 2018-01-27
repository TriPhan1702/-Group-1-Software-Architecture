using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Market.Models;
using Market.ModelView;

namespace Market.Controllers
{
    public class ProductController : Controller
    {
        private MarketDBEntities Context;

        public ProductController()
        {
            Context = new MarketDBEntities();
        }

        public ActionResult Manage()
        {
            var productList = Context.Products;
            var productListViewModel = new ProductListlViewModel()
            {
                Products = productList.ToList()
            };

            return View(productListViewModel);
        }

        //Activate/Deactivate Product
        public ActionResult SwitchProductStatus(int productId)
        {
            try
            {
                var product = Context.Products.Single(a => a.id == productId);

                product.isActive = !product.isActive;

                Context.SaveChanges();

                TempData["message"] = product.isActive
                    ? "<script>alert('Product enabled')</script>"
                    : "<script>alert('Product disabled')</script>";
                return RedirectToAction("Manage");
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Product not found')</script>";
                return RedirectToAction("Manage");
            }
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckAddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View("AddProduct", product);
            }

            Context.Products.Add(new Product()
            {
                name = product.name,
                shortDescription = product.name,
                price = product.price,
                isActive = true,
                description = product.description,
                imageURL = product.imageURL,
            });

            try
            {
                Context.SaveChanges();

                TempData["message"] = "<script>alert('Product Successfully Added')</script>";

                return RedirectToAction("Manage");
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Could not add product to database, please try again')</script>";
                return View("AddProduct", product);
            }
        }

        public ActionResult UpdateProduct(int productId)
        {
            try
            {
                var product = Context.Products.Single(p => p.id == productId);

                return View(product);
            }
            catch (Exception)
            {
                TempData["message"] = "<script>alert('Product not found, please try again')</script>";

                return RedirectToAction("Manage");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckUpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View("AddProduct", product);
            }
            
                var productFromDataBase = Context.Products.Single(p => p.id == product.id);

                productFromDataBase.name = product.name;
                productFromDataBase.shortDescription = product.shortDescription;
                productFromDataBase.price = product.price;
                productFromDataBase.imageURL = product.imageURL;
                productFromDataBase.description = product.description;

                Context.SaveChanges();

                TempData["message"] = "<script>alert('Product Successfully Updated')</script>";

                return RedirectToAction("Manage");
        }
    }
}