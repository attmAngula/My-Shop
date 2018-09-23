using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbSet;

        // ctor
        public SQLRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        /// <summary>
        /// Gets a collection of entities from the database context.
        /// </summary>
        /// <returns>
        /// Returns a DbSet.
        /// </returns>
        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        /// <summary>
        /// Commits (or saves) any changes to the database context.
        /// </summary>
        public void Commit()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Removes a single entity frim the database context.
        /// </summary>
        /// <param name="Id">
        /// Identification of the entity to be removed.
        /// </param>
        public void Delete(string Id)
        {
            var item = Find(Id);
            if (context.Entry(item).State == EntityState.Detached)
                dbSet.Attach(item);

            dbSet.Remove(item);
        }

        /// <summary>
        /// Finds a single entity in the database context.
        /// </summary>
        /// <param name="Id">
        /// Identification of the entity to be found.
        /// </param>
        /// <returns>
        /// Returns an entity which matches the identification.
        /// </returns>
        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        /// <summary>
        /// Adds an entity to the database context.
        /// </summary>
        /// <param name="item">
        /// The item to be added.
        /// </param>
        public void Insert(T item)
        {
            dbSet.Add(item);
        }

        /// <summary>
        /// Modifies (or updates) an entity in the database context.
        /// </summary>
        /// <param name="item">
        /// The item to be updated.
        /// </param>
        public void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
