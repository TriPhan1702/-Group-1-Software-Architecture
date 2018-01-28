using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market.Models;

namespace Market.ModelView
{
    public class CartViewModelItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }

    public class CartViewModel
    {
        public List<CartViewModelItem> Items { get; set; }

        public CartViewModel()
        {
            Items = new List<CartViewModelItem>();
        }

        public void AddItemToCart(Product product)
        {
            Items.Add(new CartViewModelItem()
            {
                ProductId = product.id,
                Name = product.name,
                Quantity = 1,
                ImageUrl = product.imageURL,
                PricePerUnit = product.price,
            });
        }

        public void RemoveItemFromCart(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }
    }
}