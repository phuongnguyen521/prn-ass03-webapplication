using BusinessObject.Object;
using DataAccess.DAO;
using DataAccess.Repository.Interface;
using System.Collections.Generic;

namespace DataAccess.Repository.Implement
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetail order)
        => OrderDetailDAO.Instance.AddOrderDetail(order);

        public void DeleteOrderDetailByOrderID(int orderId)
        => OrderDetailDAO.Instance.DeleteOrderDetailByOrderID(orderId);

        public void DeleteOrderDetailByProductID(int productID)
        => OrderDetailDAO.Instance.DeleteOrderDetailByProductID(productID);

        public IEnumerable<OrderDetail> GetOrderDetailListByOrderID(int orderId)
        => OrderDetailDAO.Instance.GetOrderDetailListByOrderID(orderId);

        public IEnumerable<OrderDetail> GetOrderDetailListByProductID(int productId)
        => OrderDetailDAO.Instance.GetOrderDetailListByProductID(productId);

        public decimal GetTotalOrder(Order order)
        => OrderDetailDAO.Instance.GetTotalOrder(order);
    }
}
