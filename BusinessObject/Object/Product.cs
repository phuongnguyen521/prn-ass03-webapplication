using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.Object
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Produdct Name is required")]
        [StringLength(40, ErrorMessage ="Length of product name shall be within 40 characters")]
        public string ProductName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Weight is required")]
        [StringLength(20, ErrorMessage = "Length of product name shall be within 20 characters")]
        public string Weight { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Unit Price is required")]
        public decimal UnitPrice { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Units In Stock is required")]
        public int UnitsInStock { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
