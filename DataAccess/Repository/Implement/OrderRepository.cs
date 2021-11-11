using BusinessObject.Object;
using DataAccess.DAO;
using DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;

namespace DataAccess.Repository.Implement
{
    public class OrderRepository : IOrderRepository
    {
        public void AddOrder(Order order)
        => OrderDAO.Instance.AddOrder(order);

        public void DeleteOrderByMemberID(int memberId)
        => OrderDAO.Instance.DeleteOrderByMemberID(memberId);

        public void DeleteOrderByOrderID(int orderId)
        => OrderDAO.Instance.DeleteOrderByOrderID(orderId);

        public IEnumerable<Order> GetOrdersByMemberID(int memberId)
        => OrderDAO.Instance.GetOrdersByMemberID(memberId);

        public IEnumerable<Order> GetOrdersByMemberIDAndTime(int memberId, DateTime startDate, DateTime endDate)
        => OrderDAO.Instance.GetOrdersByMemberIDAndTime(memberId, startDate, endDate);

        public Order GetOrdersByOrderID(int orderId)
        => OrderDAO.Instance.GetOrdersByOrderID(orderId);

        Order IOrderRepository.checkDeleteOrderByOrderDetail(int orderID)
        => OrderDAO.Instance.checkDeleteOrderByOrderDetail(orderID);

        IEnumerable<Order> IOrderRepository.GetOrders()
        => OrderDAO.Instance.GetOrders();

        void IOrderRepository.UpdateOrder(Order order)
        => OrderDAO.Instance.UpdateOrder(order);
    }
}
