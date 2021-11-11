using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDetailDAO
    {
        #region Initialized Objects
        //Using Singleton Pattern
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Function
        public IEnumerable<OrderDetail> GetOrderDetailListByOrderID(int orderId)
        {
            IEnumerable<OrderDetail> orderDetail = null;
            try
            {
                using (var context = new SaleManagementContext())
                {
                    if (orderId > 0)
                    {
                        IEnumerable<OrderDetail> List = context.OrderDetails.ToList();
                        if (List != null)
                        {
                            orderDetail = List.Where(temp => temp.OrderId == orderId);
                            if (orderDetail.Any() == false)
                                orderDetail = null;
                        }
                    }
                    else
                        throw new Exception("Order Id shall be larger than 0");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetail;
        }

        public IEnumerable<OrderDetail> GetOrderDetailListByProductID(int productId)
        {
            IEnumerable<OrderDetail> orderDetail = null;
            try
            {
                using (var context = new SaleManagementContext())
                {
                    if (productId > 0)
                    {
                        orderDetail = context.OrderDetails.Where(temp => temp.ProductId == productId);
                        if (orderDetail.Any() == false)
                            orderDetail = null;
                    }
                    else
                        throw new Exception("Product Id shall be larger than 0");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetail;
        }
        public void AddOrderDetail(OrderDetail order)
        {
            try
            {
                using (var context = new SaleManagementContext())
                {
                    context.OrderDetails.Add(order);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrderDetailByProductID(int productID)
        {
            try
            {
                IEnumerable<OrderDetail> orderDetail = GetOrderDetailListByProductID(productID);
                if (orderDetail != null)
                {
                    using (var context = new SaleManagementContext()) 
                    {
                        context.OrderDetails.RemoveRange(orderDetail);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrderDetailByOrderID(int orderId)
        {
            try
            {
                IEnumerable<OrderDetail> orderDetail = GetOrderDetailListByOrderID(orderId);
                if (orderDetail != null)
                {
                    using (var context = new SaleManagementContext())
                    {
                        context.OrderDetails.RemoveRange(orderDetail);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal GetTotalOrder(Order order)
        {
            decimal total = 0;
            try
            {
                IEnumerable<OrderDetail> orderDetail = GetOrderDetailListByOrderID(order.OrderId);
                if (orderDetail != null)
                {
                    foreach (var temp in orderDetail)
                    {
                        if (temp.Discount > 0)
                            total += temp.UnitPrice * temp.Quantity - (1 - Convert.ToDecimal(temp.Discount / 100));
                        else
                            total += temp.UnitPrice * temp.Quantity;
                    }
                    if (order.Freight != null && order.Freight > 0)
                        total += (decimal)order.Freight;
                }
                else
                    throw new Exception("There is no result match with order ID to sum the total");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Math.Round(total, 2);
        }
        #endregion
    }
}
