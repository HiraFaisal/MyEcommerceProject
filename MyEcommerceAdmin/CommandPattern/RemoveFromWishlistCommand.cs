using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.CommandPattern
{
    public class RemoveFromWishlistCommand : ICommand
    {
        private readonly int _wishlistItemId;
        private readonly MyEcommerceDbContext _dbContext;

        public RemoveFromWishlistCommand(int wishlistItemId, MyEcommerceDbContext dbContext)
        {
            _wishlistItemId = wishlistItemId;
            _dbContext = dbContext;
        }

        public void Execute()
        {
            var wishlistItem = _dbContext.Wishlists.Find(_wishlistItemId);
            if (wishlistItem != null)
            {
                _dbContext.Wishlists.Remove(wishlistItem);
                _dbContext.SaveChanges();
            }
        }
    }

}