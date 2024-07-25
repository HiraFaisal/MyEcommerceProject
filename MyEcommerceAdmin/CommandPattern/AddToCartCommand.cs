using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.CommandPattern
{
    public class AddToCartCommand : ICommand
    {
        private readonly int _wishlistItemId;
        private readonly MyEcommerceDbContext _dbContext;

        public AddToCartCommand(int wishlistItemId, MyEcommerceDbContext dbContext)
        {
            _wishlistItemId = wishlistItemId;
            _dbContext = dbContext;
        }

        public void Execute()
        {
            var wishlistItem = _dbContext.Wishlists.Find(_wishlistItemId);
            if (wishlistItem != null)
            {
                OrderDetail orderDetail = new OrderDetail();
                int productId = wishlistItem.ProductID;
                orderDetail.ProductID = productId;
                int quantity = 1;
                decimal price = _dbContext.Products.Find(productId)?.UnitPrice ?? 0;
                orderDetail.Quantity = quantity;
                orderDetail.UnitPrice = price;
                orderDetail.TotalAmount = quantity * price;
                orderDetail.Product = _dbContext.Products.Find(productId);

                TempShpData.items = TempShpData.items ?? new List<OrderDetail>();

                TempShpData.items.Add(orderDetail);

                _dbContext.Wishlists.Remove(wishlistItem);
                _dbContext.SaveChanges();
            }
        }
    }


}
