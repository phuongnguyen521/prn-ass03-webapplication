using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interface
{
    public interface IOrderDetailRepository
    {
        public IEnumerable<OrderDetail> GetOrderDetailListByOrderID(int orderId);
        public IEnumerable<OrderDetail> GetOrderDetailListByProductID(int productId);
        void AddOrderDetail(OrderDetail order);
        void DeleteOrderDetailByProductID(int productID);
        void DeleteOrderDetailByOrderID(int orderId);
        decimal GetTotalOrder(Order order);
    }
}
