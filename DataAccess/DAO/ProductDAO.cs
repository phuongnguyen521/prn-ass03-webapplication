using BusinessObject.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        #region Intialized Objects
        // Using Singleton Pattern
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Function
        public IEnumerable<Product> GetProductList()
        {
            IEnumerable<Product> products = null;
            try
            {
                using var context = new SaleManagementContext();
                products = context.Products.ToList();
                products = products.Where(temp => temp.UnitsInStock > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public IEnumerable<Product> GetProductListForOrder()
        {
            IEnumerable<Product> products = null;
            try
            {
                using var context = new SaleManagementContext();
                products = context.Products.ToList();
                if (products != null)
                    products = products.Where(temp => temp.UnitsInStock > 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public Product GetProductByID(int ProductID)
        {
            Product product = null;
            try
            {
                using var context = new SaleManagementContext();
                product = context.Products.SingleOrDefault(temp => temp.ProductId == ProductID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public void UpdateProductOrder(int productID, int unitsInStock)
        {
            try
            {
                Product product = GetProductByID(productID);
                if (product != null)
                {
                    product.UnitsInStock += unitsInStock;
                    UpdateProduct(product);
                }
                else
                    throw new Exception("Product does not exist");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetProductNameByID(int ProductID)
        {
            Product product = null;
            String ProductName = null;
            try
            {
                using var context = new SaleManagementContext();
                product = context.Products.SingleOrDefault(temp => temp.ProductId == ProductID);
                if (product != null)
                    ProductName = product.ProductName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ProductName;
        }

        public void AddProduct(Product product)
        {
            try
            {
                Product _product = GetProductByID(product.ProductId);
                if (_product == null)
                {
                    using var context = new SaleManagementContext();
                    context.Products.Add(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Product exists!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                Product _product = GetProductByID(product.ProductId);
                if (_product != null)
                {
                    using var context = new SaleManagementContext();
                    context.Products.Update(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Product does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteProduct(int ProductID)
        {
            try
            {
                Product product = GetProductByID(ProductID);
                if (product != null)
                {
                    using var context = new SaleManagementContext();
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Product does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Consideration
        public Product checkDeleteProduct(int productID)
        {
            Product product = null;
            try
            {
                using var context = new SaleManagementContext();
                IQueryable<Product> products = context.Products.Include(c => c.OrderDetails);
                products = products.Where(temp => temp.OrderDetails.Count > 0);
                product = products.SingleOrDefault(temp => temp.ProductId == productID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public IEnumerable<Product> SearchProductByProductName(string productName)
        {
            IEnumerable<Product> searchResult = null;
            try
            {
                using (var context = new SaleManagementContext()) 
                {
                    List<Product> list = context.Products.ToList();
                    searchResult = list.Where(temp => temp.ProductName.ToLower().Trim()
                    .Contains(productName.Trim().ToLower()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return searchResult;
        }

        public IEnumerable<Product> SearchProductByUnitPrice(decimal FromUnitPrice, decimal ToUnitPrice)
        {
            IEnumerable<Product> searchResult = null;
            try
            {
                if (FromUnitPrice > 0 && ToUnitPrice > 0)
                {
                    using var context = new SaleManagementContext();
                    List<Product> list = context.Products.ToList();
                    List<Product> tempList = new List<Product>();
                    foreach (var product in list)
                    {
                        if (product.UnitPrice >= FromUnitPrice && product.UnitPrice <= ToUnitPrice)
                            tempList.Add(product);
                    }
                    if (tempList.Any())
                        searchResult = tempList;
                }
                else
                {
                    throw new Exception("Unit Price shall be larger than 0");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return searchResult;
        }

        public IEnumerable<Product> SearchProductByUnitInStock(int FromUnitInStock, int ToUnitInStock)
        {
            IEnumerable<Product> searchResult = null;
            try
            {
                if (FromUnitInStock > 0 && ToUnitInStock > 0)
                {
                    using var context = new SaleManagementContext();
                    List<Product> list = context.Products.ToList();
                    List<Product> tempList = new List<Product>();
                    foreach (var product in list)
                    {
                        if (product.UnitsInStock >= FromUnitInStock && product.UnitsInStock <= ToUnitInStock)
                            tempList.Add(product);
                    }
                    if (tempList.Any())
                        searchResult = tempList;
                }
                else
                {
                    throw new Exception("Unit In Stock shall be larger than 0");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return searchResult;
        }

        public IEnumerable<Product> SearchProductByNameAndID(string productName, string id)
        {
            IEnumerable<Product> searchResult = null;
            int ProductId = Int32.Parse(id);
            try
            {
                using var context = new SaleManagementContext();
                List<Product> list = context.Products.ToList();
                List<Product> TempList = new List<Product>();
                foreach (var product in list)
                {
                    if (product.ProductId == ProductId
                        && product.ProductName.ToLower().Trim()
                        .Contains(productName.Trim().ToLower()))
                        TempList.Add(product);
                }
                if (TempList.Any())
                    searchResult = TempList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return searchResult;
        }

        public List<String> getUnitPrice()
        {
            List<String> list = new List<string>();
            decimal counter1 = 0;
            decimal counter2 = 50000;
            for (int number = 0; number < 5; number++)
            {
                string line = $"{counter1} - {counter2}";
                list.Add(line);
                counter1 = counter2 + 1000;
                counter2 += 50000;
            }
            return list;
        }

        public IEnumerable<Product> SearchProductByProductNameAndUnitPrice(string productName, string UnitPrice)
        {
            IEnumerable<Product> searchResult = null;
            decimal from = 0;
            decimal to = 0;
            ConvertStringToDecimalUnitPrice(UnitPrice, ref from, ref to);
            try
            {
                using (var context = new SaleManagementContext())
                {
                    List<Product> list = context.Products.ToList();
                    searchResult = list.Where(temp => temp.ProductName.ToLower().Trim()
                    .Contains(productName.Trim().ToLower())
                    && temp.UnitPrice >= from
                    && temp.UnitPrice <= to);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return searchResult;
        }

        public void ConvertStringToDecimalUnitPrice(string UnitPrice, ref decimal from, ref decimal to)
        {
            string[] array = UnitPrice.Split("-");
            from = Convert.ToDecimal(array[0].Trim());
            to = Convert.ToDecimal(array[1].Trim());
        }
        #endregion
    }
}
