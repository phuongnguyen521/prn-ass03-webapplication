using BusinessObject.Object;
using ClosedXML.Excel;
using DataAccess.Repository.Implement;
using DataAccess.Repository.Interface;
using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class OrdersController : Controller
    {
        private IOrderRepository orderRepository = null;
        private IOrderDetailRepository orderDetailRepository = null;
        private IProductRepository productRepository = null;
        private IMemberRepository memberRepository = null;
        private AuthorityController authorityController = new AuthorityController();
        public OrdersController()
        {
            orderDetailRepository = new OrderDetailRepository();
            orderRepository = new OrderRepository();
            productRepository = new ProductRepository();
            memberRepository = new MemberRepository();
        }
        // GET: OrdersController
        public async Task<IActionResult> Index(DateTime? searchStartOrder,
            DateTime? searchEndOrder, int? page)
        {
            
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                IEnumerable<Order> orders = null;
                if (page == null)
                    page = 1;
                int pageSize = 5;
                @ViewBag.SearchStartOrder = searchStartOrder;
                @ViewBag.SearchEndOrder = searchEndOrder;
                if (searchStartOrder != null && searchEndOrder != null)
                {
                    orders = orderRepository.
                        GetOrdersByMemberIDAndTime(0, searchStartOrder.Value, searchEndOrder.Value);
                }
                else
                    orders = orderRepository.GetOrders();
                // Reference: https://www.c-sharpcorner.com/article/session-state-in-asp-net-core/
                HttpContext.Session.SetComplexData("OrderListTemp", orders);
                return View(await PaginatedList<Order>.CreateAsync(orders.AsQueryable(), page ?? 1, pageSize));
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        public ActionResult Export()
        {
            IEnumerable<Order> orderList = HttpContext.Session.GetComplexData<IEnumerable<Order>>("OrderListTemp");
            if (orderList != null )
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Orders");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Order ID";
                    worksheet.Cell(currentRow, 2).Value = "Member ID";
                    worksheet.Cell(currentRow, 3).Value = "Order Date";
                    worksheet.Cell(currentRow, 4).Value = "Required Date";
                    worksheet.Cell(currentRow, 5).Value = "Shipped Date";
                    worksheet.Cell(currentRow, 6).Value = "Freight";
                    worksheet.Cell(currentRow, 7).Value = "Total";
                    foreach(var temp in orderList)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = temp.OrderId;
                        worksheet.Cell(currentRow, 2).Value = temp.MemberId;
                        worksheet.Cell(currentRow, 3).Value = temp.OrderDate;
                        worksheet.Cell(currentRow, 4).Value = temp.RequiredDate;
                        worksheet.Cell(currentRow, 5).Value = temp.ShippedDate;
                        worksheet.Cell(currentRow, 6).Value = temp.Freight;
                        decimal total = orderDetailRepository.GetTotalOrder(temp);
                        worksheet.Cell(currentRow, 7).Value = total;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-oficedocument.spreadsheetml", "Report.xls");
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //GET: OrdersController/HistoryOrders
        public async Task<IActionResult> HistoryOrders(int? page)
        {
            if (page == null)
                page = 1;
            int pageSize = 5;
            IEnumerable<Order> orders = null;
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (String.IsNullOrEmpty(user))
                return NotFound();
            else
            {
                if (user.Equals("Admin"))
                    orders = orderRepository.GetOrders();
                else
                {
                    int memberID = Convert.ToInt32(user);
                    orders = orderRepository.GetOrdersByMemberID(memberID);
                }
            }
            return View(await PaginatedList<Order>.CreateAsync(orders.AsQueryable(), page ?? 1, pageSize));
        }

        // GET: OrdersController/Details/5
        public ActionResult Details(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkorderDetailAuthority(path,user))
            {
                if (id == null)
                    return NotFound();
                var order = orderRepository.GetOrdersByOrderID(id.Value);
                if (order == null)
                    return NotFound();
                IEnumerable<OrderDetail> orderDetails = orderDetailRepository.GetOrderDetailListByOrderID(id.Value);
                if (orderDetails == null)
                    return NotFound();
                order.OrderDetails = orderDetails.ToList();
                decimal total = orderDetailRepository.GetTotalOrder(order);
                ViewData["Total"] = total;
                return View(order);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        //GET: OrdersController/HistoryOrders/1

        // GET: OrdersController/Create
        public ActionResult Create() 
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
                return View();
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string error = checkOrder(order, true);
                    if (error is not null)
                        ViewBag.Message = error;
                    else
                    {
                        HttpContext.Session.SetComplexData("NewOrder", order);
                        return RedirectToAction(nameof(AddProduct));
                    }
                }
                return View(order);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(order);
            }
        }

        public string checkOrder(Order order, bool checkDuplicatedID = false)
        {
            if (checkDuplicatedID)
            {
                var tempOrder = orderRepository.GetOrdersByOrderID(order.OrderId);
                if (tempOrder is not null)
                    return "Duplicated Order ID";
            }
            var tempMember = memberRepository.GetMemberByID(order.MemberId);
            if (tempMember is null)
                return "Member ID does not exist";
            int requiredDate = DateTime.Compare(order.RequiredDate.Value, order.OrderDate);
            int shippedDate = DateTime.Compare(order.ShippedDate.Value, order.OrderDate);
            if (requiredDate <= 0 && shippedDate <= 0)
                return "Required Date and Shipped Date are earlier than Order Date";
            if (requiredDate <= 0 && shippedDate > 0)
                return "Required Date is earlier than Order Date";
            if (requiredDate > 0 && shippedDate <= 0)
                return "Shipped Date is earlier than Order Date";
            return null;
        }
        //GET: OrdersController/AddProduct
        public async Task<IActionResult> AddProduct(string? searchProductName, string? SelectedUnitPriceRange, int? page)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user)) 
            {
                Order order = HttpContext.Session.GetComplexData<Order>("NewOrder");
                if (order != null)
                {
                    if (page == null)
                        page = 1;
                    int pageSize = 5;
                    var List = productRepository.getUnitPrice();
                    ViewBag.UnitPriceList = new SelectList(List);
                    @ViewBag.SearchProductName = searchProductName;
                    // HttpContext.Session.GetComplexData
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
                return RedirectToAction(nameof(AddProductError));
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        public ActionResult AddProductError() => View();

        public ActionResult updateProduct(int? productId, int? quantityToCart,
            double? DiscountToAdd, bool removeCart = false)
        {
            //HttpContext.Session.SetComplexData(productList)
            if (productId == null 
                && quantityToCart == null 
                && DiscountToAdd == null)
                return NotFound();
            var product = productRepository.GetProductByID(productId.Value);
            if (product == null)
                return NotFound();
            try 
            {
                if (updateCart(product, quantityToCart.Value,DiscountToAdd.Value, removeCart))
                {
                    if (removeCart)
                        product.UnitsInStock += quantityToCart.Value;
                    else
                        product.UnitsInStock -= quantityToCart.Value;
                    productRepository.UpdateProduct(product);
                }
                if (removeCart)
                    return RedirectToAction(nameof(ViewCart));
                else
                    return RedirectToAction(nameof(AddProduct));
                //HttpContext.Session.SetComplexData(cart)
            } catch (Exception ex) 
            { 
                ViewBag.Message = ex.Message;
            }
                return View();
        }

        public bool updateCart(Product product, int quantity, double Discount, bool removeCart = false)
        {
            bool result = false;
            try
            {
                Order order = HttpContext.Session.GetComplexData<Order>("NewOrder");
                // order exists
                if (order != null)
                {
                    List<OrderDetail> cart = HttpContext.Session.GetComplexData<List<OrderDetail>>("Cart");
                    //Cart does not exist
                    if (cart is null || cart.Count() == 0)
                    {
                        cart = new List<OrderDetail>();
                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = product.ProductId,
                            Quantity = quantity,
                            UnitPrice = product.UnitPrice,
                            Discount = Discount
                        };
                        cart.Add(orderDetail);
                        HttpContext.Session.SetComplexData("Cart", cart);
                        result = true;
                    }//End of null cart
                    //Cart exists
                    else
                    {
                        var productTemp = cart.SingleOrDefault(temp => temp.ProductId == product.ProductId);
                        //product exists in cart
                        if (productTemp != null)
                        {
                            //remove from Cart
                            if (removeCart)
                            {
                                productTemp.Quantity -= quantity;
                                cart.Remove(productTemp);
                                // quantity
                                if (productTemp.Quantity > 0)
                                    cart.Add(productTemp);
                                HttpContext.Session.SetComplexData("Cart", cart);
                                result = true;
                            }
                            //Add to Cart
                            else
                            {
                                productTemp.Quantity += quantity;
                                cart.Remove(productTemp);
                                cart.Add(productTemp);
                                HttpContext.Session.SetComplexData("Cart", cart);
                                result = true;
                            }
                        }//End of existed product in cart
                        //product does not exist in cart
                        else
                        {
                            OrderDetail orderDetail = new OrderDetail
                            {
                                OrderId = order.OrderId,
                                ProductId = product.ProductId,
                                Quantity = quantity,
                                UnitPrice = product.UnitPrice,
                                Discount = Discount
                            };
                            cart.Add(orderDetail);
                            HttpContext.Session.SetComplexData("Cart", cart);
                            result = true;
                        }// End of existed product in cart
                    }//End of existed cart
                } //end of existed order
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                result = false;
            }
            return result;
        }

        public ActionResult ViewCart()
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                Order order = HttpContext.Session.GetComplexData<Order>("NewOrder");
                if (order == null)
                    return NotFound();
                List<OrderDetail> cart = HttpContext.Session.GetComplexData<List<OrderDetail>>("Cart");
                if (cart is null || cart.Count() == 0)
                    return View(order);
                decimal total = GetTotalOrder(order, cart);
                order.OrderDetails = cart.ToList();
                ViewData["Total"] = total;
                return View(order);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        public decimal GetTotalOrder(Order order, List<OrderDetail> cart)
        {
            decimal total = 0;
            if (cart != null)
            {
                foreach (var temp in cart)
                {
                    if (temp.Discount > 0)
                        total += temp.UnitPrice * temp.Quantity - (1 - Convert.ToDecimal(temp.Discount / 100));
                    else
                        total += temp.UnitPrice * temp.Quantity;
                }
                if (order.Freight != null && order.Freight > 0)
                    total += (decimal)order.Freight;
            }
            return total;
        }

        public ActionResult RemoveCart(int? productId, int? quantity, double Discount = 0)
        {
            if (productId == null && quantity == null)
                return NotFound();
            return updateProduct(productId,quantity, Discount, true);
        }

        public ActionResult CheckOut()
        {
            try
            {
                Order order = HttpContext.Session.GetComplexData<Order>("NewOrder");
                if (order == null)
                    return NotFound();
                List<OrderDetail> cart = HttpContext.Session.GetComplexData<List<OrderDetail>>("Cart");
                if (cart is null || cart.Count() == 0)
                    return NotFound();
                orderRepository.AddOrder(order);
                foreach (var item in cart)
                    orderDetailRepository.AddOrderDetail(item);
                HttpContext.Session.Remove("NewOrder");
                HttpContext.Session.Remove("Cart");
            } catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var order = orderRepository.GetOrdersByOrderID(id.Value);
                if (order == null)
                    return NotFound();
                return View(order);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            try
            {
                if (id != order.OrderId)
                    return NotFound();
                if (ModelState.IsValid)
                {
                    string error = checkOrder(order);
                    if (error is not null)
                        ViewBag.Message = error;
                    else
                    {
                        orderRepository.UpdateOrder(order);
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(order);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(order);
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var order = orderRepository.GetOrdersByOrderID(id.Value);
                if (order == null)
                    return NotFound();
                string error = checkDeleteOrder(order.OrderId);
                if (error is not null)
                {
                    ViewBag.Message = error;
                    return RedirectToAction(nameof(Index));
                }
                return View(order);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                orderDetailRepository.DeleteOrderDetailByOrderID(id);
                orderRepository.DeleteOrderByOrderID(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public string checkDeleteOrder(int orderID)
        {
            var orderDetails = orderDetailRepository.GetOrderDetailListByOrderID(orderID);
            if (orderDetails is not null)
                return "You cannot delete this order";
            return null;
        }
    }
}
