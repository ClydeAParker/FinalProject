using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSWDFinalProject.UI.MVC.Models
{
    public class UserDetailsViewModels
    {
        [Display(Name = "Location ID")]
        [Required(ErrorMessage = "Location ID is required")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public Nullable<int> LocationId { get; set; }
        public System.DateTime ReservationDate { get; set; }
    }
}