using System;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Web.Models
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "{0} can not be empty")]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email address can not be empty")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} can not be empty")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
