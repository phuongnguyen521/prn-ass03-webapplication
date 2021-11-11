using BusinessObject.Object;
using DataAccess.Repository.Implement;
using DataAccess.Repository.Interface;
using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class ProductsController : Controller
    {
        private IProductRepository productRepository = null;
        private IOrderDetailRepository orderDetailRepository = null;
        private AuthorityController authorityController = new AuthorityController();
        public ProductsController() 
        {
            productRepository = new ProductRepository();
            orderDetailRepository = new OrderDetailRepository();
        } 
        // GET: ProductController
        public async Task<IActionResult> Index(string? searchProductName, string? SelectedUnitPriceRange, int? page)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (page == null)
                    page = 1;
                int pageSize = 5;
                var List = productRepository.getUnitPrice();
                ViewBag.UnitPriceList = new SelectList(List);
                @ViewBag.SearchProductName = searchProductName;
                IEnumerable<Product> productList = productRepository.GetProduct();
                if (searchProductName != null && SelectedUnitPriceRange != null &&
                    SelectedUnitPriceRange.Equals("Select Unit Price Range") == false)
                {
                    productList = productRepository
                        .SearchProductByProductNameAndUnitPrice(searchProductName, SelectedUnitPriceRange);
                }
                return View(await PaginatedList<Product>
                    .CreateAsync(productList.AsQueryable(), page ?? 1, pageSize));
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var product = productRepository.GetProductByID(id.Value);
                if (product == null)
                    return NotFound();
                return View(product);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
                return View();
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string error = checkDuplicatedProductID(product.ProductId);
                    if (error is not null)
                        error = error + " " + checkDuplicatedProductName(product.ProductName);
                    else
                        error = checkDuplicatedProductName(product.ProductName);
                    if (error is null)
                    {
                        productRepository.InsertProduct(product);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        ViewBag.Message = error;
                }
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }

        public string checkDuplicatedProductID(int productID)
        {
            IEnumerable<Product> products = productRepository.GetProduct();
            var product = products.SingleOrDefault(temp
                => temp.ProductId == productID);
            if (product != null)
                return "Product ID is duplicated";
            return null;
        }

        public string checkDuplicatedProductName(string productName)
        {
            IEnumerable<Product> products = productRepository.GetProduct();
            var product = products.SingleOrDefault(temp
                => temp.ProductName.Equals(productName));
            if (product != null)
                return "Product ID is duplicated";
            return null;
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var product = productRepository.GetProductByID(id.Value);
                if (product == null)
                    return NotFound();
                return View(product);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (id != product.CategoryId)
                    return NotFound();
                if (ModelState.IsValid)
                {
                    string error = checkDuplicatedProductID(product.ProductId);
                    if (error is not null)
                        error = error + " " + checkDuplicatedProductName(product.ProductName);
                    else
                        error = checkDuplicatedProductName(product.ProductName);
                    if (error is null)
                    {
                        productRepository.UpdateProduct(product);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        ViewBag.Message = error;
                }
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var product = productRepository.GetProductByID(id.Value);
                if (product == null)
                    return NotFound();
                string error = checkDeleteProduct(product.ProductId);
                if (error is not null)
                {
                    ViewBag.Message = error;
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                productRepository.DeleteProduct(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public string checkDeleteProduct(int productID)
        {
            var orderDetails = orderDetailRepository.GetOrderDetailListByProductID(productID);
            if (orderDetails is not null)
                return "You cannot delete this product";
            return null;
        }
    }
}
