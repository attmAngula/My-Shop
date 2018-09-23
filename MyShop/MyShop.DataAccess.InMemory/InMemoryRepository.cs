using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        // ctor
        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;

            if (items == null)
                items = new List<T>();
        }

        /// <summary>
        /// Stores the items in memory.
        /// </summary>
        public void Commit()
        {
            cache[className] = items;
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">
        /// The item to be added.
        /// </param>
        public void Insert(T item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Updates an item in the list of items.
        /// </summary>
        /// <param name="item">
        /// The item to be updated.
        /// </param>
        public void Update(T item)
        {
            T itemToUpdate = items.Find(i => i.Id == item.Id);
            if (itemToUpdate != null)
            {
                itemToUpdate = item;
            }
            else
            {
                throw new Exception($"{className} not found");
            }
        }

        /// <summary>
        /// Finds an item in the list of items.
        /// </summary>
        /// <param name="Id">
        /// Id of the item to be found.
        /// </param>
        /// <returns>
        /// Returns the item if found, ot throws an 
        /// exception if the item is not found.
        /// </returns>
        public T Find(string Id)
        {
            T item = items.Find(i => i.Id == Id);

            if (item != null)
                return item;
            else
                throw new Exception($"{className} not found");
        }

        /// <summary>
        /// Gets a queryable list of products.
        /// </summary>
        /// <returns>
        /// Returns a product list that can be queried.
        /// </returns>
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        /// <summary>
        /// Removes a specified item from the list.
        /// </summary>
        /// <param name="Id">
        /// Id of item to be removed.
        /// </param>
        public void Delete(string Id)
        {
            T itemToDelete = items.Find(i => i.Id == Id);

            if (itemToDelete != null)
                items.Remove(itemToDelete);
            else
                throw new Exception($"{className} not found");
        }

    }
}
