using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        // ctor
        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
                products = new List<Product>();
        }

        /// <summary>
        /// Stores the current list of products to the cache
        /// before they are saved.
        /// </summary>
        public void Commit()
        {
            cache["products"] = products;
        }

        /// <summary>
        /// Adds an item to the product list.
        /// </summary>
        /// <param name="product">
        /// The item to be added.
        /// </param>
        public void Insert(Product product)
        {
            products.Add(product);
        }

        /// <summary>
        /// Updates an item in the product list.
        /// </summary>
        /// <param name="product">
        /// The item to be updated.
        /// </param>
        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        /// <summary>
        /// Finds a specified product.
        /// </summary>
        /// <param name="Id">
        /// Id of the product to be found.
        /// </param>
        /// <returns>
        /// Returns product if found. Throws an 
        /// exception if not found.
        /// </returns>
        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }

        /// <summary>
        /// Gets a queryable list of products.
        /// </summary>
        /// <returns>
        /// Returns a product list that can be queried.
        /// </returns>
        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        /// <summary>
        /// Removes a specified product from the list.
        /// </summary>
        /// <param name="Id">
        /// Id of product to be removed.
        /// </param>
        public void Delete(string Id)
        {
            Product productToDelete = products.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found.");
            }
        }
    }
}
