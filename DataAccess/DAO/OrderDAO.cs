using BusinessObject.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDAO
    {
        #region Initialized Objects
        //Use Singleton Pattern
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Function
        public IEnumerable<Order> GetOrders()
        {
            IEnumerable<Order> orderList = null;
            try
            {
                using (var context = new SaleManagementContext())
                {
                    orderList = context.Orders
                        .ToList()
                        .OrderBy(temp => temp.OrderId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderList;
        }

        public IEnumerable<Order> GetOrdersByMemberID(int memberId)
        {
            IEnumerable<Order> orders = null;
            try
            {
                using (var context = new SaleManagementContext()) 
                {
                    if (memberId > 0)
                    {
                        orders = context.Orders.Where(temp => temp.MemberId == memberId)
                            .OrderByDescending(temp => temp.OrderId);
                        var list = GetOrders();
                        if (list != null)
                            orders = list.Where(temp => temp.MemberId == memberId)
                                .OrderByDescending(temp => temp.OrderId);
                    }
                    else
                    {
                        orders = context.Orders.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public IEnumerable<Order> GetOrdersByMemberIDAndTime(int memberId, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Order> orders = null;
            IEnumerable<Order> tempOrders = null;
            try
            {
                using (var context = new SaleManagementContext()) 
                {
                    tempOrders = context.Orders.ToList();
                    if (memberId > 0)
                    {
                        orders = tempOrders.Where(temp =>
                        temp.MemberId == memberId &&
                        // find equal date or later date comparing to start date
                        DateTime.Compare(temp.OrderDate, startDate) >= 0 &&
                        // find earlier date or equal date comparing to end date
                        DateTime.Compare(temp.OrderDate, endDate) <= 0)
                            .OrderByDescending(temp => temp.OrderId);
                    }
                    else
                    {
                        orders = tempOrders.Where(temp =>
                        // find equal date or later date comparing to start date
                        DateTime.Compare(temp.OrderDate, startDate) >= 0 &&
                        // find earlier date or equal date comparing to end date
                        DateTime.Compare(temp.OrderDate, endDate) <= 0)
                            .OrderByDescending(temp => temp.OrderId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public Order GetOrdersByOrderID(int orderId)
        {
            Order orders = null;
            try
            {
                using (var context = new SaleManagementContext()) 
                {
                    orders = context.Orders.SingleOrDefault(temp => temp.OrderId == orderId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public void AddOrder(Order order)
        {
            try
            {
                Order _temp = GetOrdersByOrderID(order.OrderId);
                if (_temp == null)
                {
                    using (var context = new SaleManagementContext()) 
                    {
                        context.Orders.Add(order);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Duplicated order Id");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order checkDeleteOrderByOrderDetail(int orderID)
        {
            Order product = null;
            try
            {
                using (var context = new SaleManagementContext())
                {
                    IQueryable<Order> orders = context.Orders.Include(c => c.OrderDetails);
                    orders = orders.Where(temp => temp.OrderDetails.Count > 0);
                    product = orders.SingleOrDefault(temp => temp.OrderId == orderID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public void DeleteOrderByMemberID(int memberId)
        {
            try
            {
                var order = GetOrdersByMemberID(memberId);
                if (order != null)
                {
                    using (var context = new SaleManagementContext()) 
                    {
                        context.Orders.RemoveRange(GetOrdersByMemberID(memberId));
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Member Id does not exist in order");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrderByOrderID(int orderId)
        {
            try
            {
                Order order = GetOrdersByOrderID(orderId);
                if (order != null)
                {
                    using (var context = new SaleManagementContext()) 
                    {
                        context.Orders.Remove(order);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Order Id does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                Order _order = GetOrdersByOrderID(order.OrderId);
                if (_order != null)
                {
                    using (var context = new SaleManagementContext())
                    {
                        context.Orders.Update(order);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("Order Id does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
