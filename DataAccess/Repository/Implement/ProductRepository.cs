using BusinessObject.Object;
using DataAccess.DAO;
using DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Implement
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(int productID)
        => ProductDAO.Instance.DeleteProduct(productID);

        public IEnumerable<Product> GetProduct()
        => ProductDAO.Instance.GetProductList();

        public void InsertProduct(Product product)
        => ProductDAO.Instance.AddProduct(product);

        public IEnumerable<Product> SearchProductByProductName(string productName)
        => ProductDAO.Instance.SearchProductByProductName(productName);

        public void UpdateProduct(Product product)
        => ProductDAO.Instance.UpdateProduct(product);

        IEnumerable<Product> IProductRepository.GetProductListForOrder()
        => ProductDAO.Instance.GetProductListForOrder();

        string IProductRepository.GetProductNameByID(int ProductID)
        => ProductDAO.Instance.GetProductNameByID(ProductID);

        Product IProductRepository.GetProductByID(int ProductID)
        => ProductDAO.Instance.GetProductByID(ProductID);

        IEnumerable<Product> IProductRepository.SearchProductByNameAndID(string productName, string id)
        => ProductDAO.Instance.SearchProductByNameAndID(productName, id);

        IEnumerable<Product> IProductRepository.SearchProductByUnitInStock(int FromUnitInStock, int ToUnitInStock)
        => ProductDAO.Instance.SearchProductByUnitInStock(FromUnitInStock, ToUnitInStock);

        IEnumerable<Product> IProductRepository.SearchProductByUnitPrice(decimal FromUnitPrice, decimal ToUnitPrice)
        => ProductDAO.Instance.SearchProductByUnitPrice(FromUnitPrice, ToUnitPrice);

        public void UpdateProductOrder(int productID, int unitsInStock)
        => ProductDAO.Instance.UpdateProductOrder(productID, unitsInStock);

        Product IProductRepository.checkDeleteProduct(int productID)
        => ProductDAO.Instance.checkDeleteProduct(productID);

        List<string> IProductRepository.getUnitPrice()
        => ProductDAO.Instance.getUnitPrice();

        IEnumerable<Product> IProductRepository.SearchProductByProductNameAndUnitPrice(string productName, string UnitPrice)
        => ProductDAO.Instance.SearchProductByProductNameAndUnitPrice(productName, UnitPrice);
    }
}
