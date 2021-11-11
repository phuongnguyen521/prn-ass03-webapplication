using BusinessObject.Object;
using System;
using System.Collections.Generic;

namespace DataAccess.Repository.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProduct();
        public string GetProductNameByID(int ProductID);
        public Product GetProductByID(int ProductID);
        void InsertProduct(Product product);
        void UpdateProduct(Product product);
        public void UpdateProductOrder(int productID, int unitsInStock);
        public Product checkDeleteProduct(int productID);
        void DeleteProduct(int productID);
        IEnumerable<Product> SearchProductByProductName(string productName);
        public IEnumerable<Product> SearchProductByUnitPrice(decimal FromUnitPrice, decimal ToUnitPrice);
        public IEnumerable<Product> SearchProductByUnitInStock(int FromUnitInStock, int ToUnitInStock);
        public IEnumerable<Product> SearchProductByNameAndID(string productName, string id);
        public IEnumerable<Product> GetProductListForOrder();
        public List<String> getUnitPrice();
        public IEnumerable<Product> SearchProductByProductNameAndUnitPrice(string productName, string UnitPrice);
    }
}
