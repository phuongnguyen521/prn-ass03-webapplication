using BusinessObject.Object;
using DataAccess.Repository.Implement;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eStore.Controllers
{
    public class AuthorityController : Controller
    {
        private readonly string USER_PROFILE = "/Members/Profile";
        private readonly string USER_ORDER = "/Orders/HistoryOrders";
        
        public bool checkAuthority(string path, string user)
        {
            bool result = true;
            if (user != null)
            {
                if (!user.Equals("Admin"))
                {
                    string USER_EDIT_PROFILE = "/Members/Edit/" + user;
                    if (path.Equals(USER_PROFILE))
                        result = true;
                    else if (path.Equals(USER_ORDER))
                        result = true;
                    else if (path.Equals(USER_EDIT_PROFILE))
                        result = true;
                    else
                        result = false;
                }
            }
            return result;
        }

        public bool checkorderDetailAuthority(string path, string user)
        {
            bool result = true;
            if (user != null)
            {
                if (!user.Equals("Admin"))
                {
                    try
                    {
                        IOrderRepository orderRepository = new OrderRepository();
                        IEnumerable<Order> orders = orderRepository.GetOrdersByMemberID(Convert.ToInt32(user));
                        if (orders != null)
                        {
                            int index = path.LastIndexOf("/");
                            int orderId = 0;
                            if (path.Length == index + 2)
                                orderId = Convert.ToInt32(path.Substring(index + 1));
                            else
                                orderId = Convert.ToInt32(path.Substring(index + 1, path.Length));
                            var order = orders.SingleOrDefault(temp => temp.OrderId == orderId);
                            if (order == null)
                                result = false;
                        }
                        else
                            result = false;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                    }
                }
            }
            return result;
        }
    }
}
