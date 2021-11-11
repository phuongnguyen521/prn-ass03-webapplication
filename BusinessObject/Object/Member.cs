using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;

#nullable disable

namespace BusinessObject.Object
{
    public partial class Member
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Member ID is required")]
        //[Remote("IsExistedMemberID", "Members",ErrorMessage ="Member ID already Existed")]
        public int MemberId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(dataType:DataType.EmailAddress,ErrorMessage ="Valid format Email")]
        [StringLength(100, ErrorMessage = "Length of email shall be within 100 characters")]
        //[Remote("IsExistedEmail", "Members", ErrorMessage = "Email already Existed")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Company name is required")]
        [StringLength(40,ErrorMessage ="Length of company name shall be within 40 characters")]
        public string CompanyName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        [StringLength(15, ErrorMessage = "Length of city shall be within 15 characters")]
        [DataType(dataType: DataType.Text, ErrorMessage = "City shall be consist only characters and valid whitespace")]
        public string City { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required")]
        [StringLength(15, ErrorMessage = "Length of country shall be within 15 characters")]
        [DataType(dataType:DataType.Text,ErrorMessage ="Country shall be consist only characters and valid whitespace")]
        public string Country { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Length of password shall be within 30 characters")]
        [DataType(dataType: DataType.Password, ErrorMessage = "Valid password")]
        public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
