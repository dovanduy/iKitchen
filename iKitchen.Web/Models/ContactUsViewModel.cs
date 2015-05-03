using System;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Web.Models
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
