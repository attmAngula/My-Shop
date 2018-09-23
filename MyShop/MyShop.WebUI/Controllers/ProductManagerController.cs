using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using MyShop.Core.ViewModels;
using MyShop.Core.Contracts;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        /// <summary>
        /// Constructor:
        /// Instanciates the ProductRepository object.
        /// </summary>
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }
        
        // ******************** Retrieve ******************** //

        /// <summary>
        /// Index action:
        /// Retreaves/reads products from the context.
        /// </summary>
        /// <returns>
        /// Returns a view with products.
        /// </returns>
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        
        // ******************** Create ******************** //

        /// <summary>
        /// [HttpGet] Create:
        /// Makes an instance of a product.
        /// </summary>
        /// <returns>
        /// Returns only a view.
        /// </returns>
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();
            return View(viewModel);
        }

        /// <summary>
        /// [HttpPost] Create: 
        /// Creates a new product.
        /// </summary>
        /// <param name="product">
        /// Product object parsed by the POST method
        /// when the submit button is clicked.
        /// </param>
        /// <returns>
        /// Returns the same view if ModelState is invalid.
        /// Redirects to the Index view if valid.
        /// </returns>
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }
        
        // ******************** Edit ******************** //

        /// <summary>
        /// [HttpGet] Edit:
        /// Edits a selected product item from the list.
        /// </summary>
        /// <param name="Id">
        /// Id of the product item to be edited.
        /// </param>
        /// <returns>
        /// Returns a view with the product to be edited. Returns a
        /// an HttpNotFound if product item is not found.
        /// </returns>
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();

                return View(viewModel); 
            }
        }

        /// <summary>
        /// [HttpPost] Edit:
        /// Saves or commits the edit changes to the cache
        /// after the form is submitted.
        /// </summary>
        /// <param name="product">
        /// The edited product from the form.
        /// </param>
        /// <param name="Id">
        /// The edited product Id from the form.
        /// </param>
        /// <returns>
        /// Returns HttpNotFound if there is no matching product item.
        /// Returns Index view if a product item was edited successfully.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        // ******************** Delete ******************** //

        /// <summary>
        /// Gets the delete page with a specified product
        /// item to be deleted.
        /// </summary>
        /// <param name="Id">
        /// Id of the specified product.
        /// </param>
        /// <returns>
        /// Returns the Delete view with the specified item, or 
        /// HttpNotFound if specified item does not exist.
        /// </returns>
        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

    }
}