using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        // ctor
        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            this.basketContext = basketContext;
            this.productContext = productContext;
        }

        /// <summary>
        /// Gets an existing basket from the cookies. Creates a new
        /// basket if no basket exists in the cookies.
        /// </summary>
        /// <param name="httpContext">
        /// Context from the browser.
        /// </param>
        /// <param name="createIfNull">
        /// True or false. Option to create a cookie if none is found.
        /// </param>
        /// <returns>
        /// Returns a Basket object.
        /// </returns>
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;

        } // end of GetBasket()

        /// <summary>
        /// Creates a new basket and adds it to the cookies.
        /// </summary>
        /// <param name="httpContext">
        /// Context from the browser.
        /// </param>
        /// <returns>
        /// Returns the created basket.
        /// </returns>
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        /// <summary>
        /// Adds an item to the basket.
        /// </summary>
        /// <param name="httpContext">
        /// Context from the browser.
        /// </param>
        /// <param name="productId">
        /// Id of the product to be added.
        /// </param>
        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }

            basketContext.Commit();
        }

        /// <summary>
        /// Removes an item from the basket.
        /// </summary>
        /// <param name="httpContext">
        /// Context from the browser.
        /// </param>
        /// <param name="itemId">
        /// Item to be removed.
        /// </param>
        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        /// <summary>
        /// Provides a list of all items in the basket.
        /// </summary>
        /// <param name="httpContext">
        /// Context from the browser.
        /// </param>
        /// <returns>
        /// Returns a list of items in the basket.
        /// </returns>
        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price
                              }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        /// <summary>
        /// Generates a basket summary.
        /// </summary>
        /// <param name="httpContext">
        /// Context from the browser.
        /// </param>
        /// <returns>
        /// Returns the BasketSummaryViewModel.
        /// </returns>
        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0; // if basketCount == null, then assign 0 instead
                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;
            }else
            {
                return model;
            }
        }
        
    }
}
