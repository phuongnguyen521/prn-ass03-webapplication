using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.Object
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Member ID is required")]
        public int MemberId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "Required date is required")]
        [DataType(dataType:DataType.DateTime, ErrorMessage ="Required date shall consist date and time")]
        public DateTime? RequiredDate { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "Shipped date is required")]
        [DataType(dataType: DataType.DateTime, ErrorMessage = "Shipped date shall consist date and time")]
        public DateTime? ShippedDate { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = "Freight is required")]
        public decimal? Freight { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
