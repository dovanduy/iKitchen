using System;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Web.Models
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "EventTime")]
        public DateTime EventTime { get; set; }

        [Required(ErrorMessage = "{0}can not be empty")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
