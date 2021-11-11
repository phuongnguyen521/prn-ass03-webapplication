using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.Object
{
    public partial class OrderDetail
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Unit Price is required")]
        public decimal UnitPrice { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Discount is required")]
        public double Discount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
