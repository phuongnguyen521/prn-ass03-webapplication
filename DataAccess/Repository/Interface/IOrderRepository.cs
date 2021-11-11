using BusinessObject.Object;
using System;
using System.Collections.Generic;

namespace DataAccess.Repository.Interface
{
    public interface IOrderRepository
    {
        public IEnumerable<Order> GetOrdersByMemberID(int memberId);
        public IEnumerable<Order> GetOrdersByMemberIDAndTime(int memberId, DateTime startDate, DateTime endDate);
        public Order GetOrdersByOrderID(int orderId);
        void AddOrder(Order order);
        public Order checkDeleteOrderByOrderDetail(int orderID);
        void DeleteOrderByMemberID(int memberId);
        void DeleteOrderByOrderID(int orderId);
        public IEnumerable<Order> GetOrders();
        public void UpdateOrder(Order order);
    }
}
