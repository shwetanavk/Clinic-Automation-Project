using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class LoginModel
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please enter email.")] //Server side validation
        [EmailAddress(ErrorMessage = "Invalid format for email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }
        public List<SelectListItem> RoleTypes { get; set; }
    }
}