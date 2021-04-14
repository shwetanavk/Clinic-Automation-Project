using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicAutomationProject.Models
{
    public class SupplierModel
    {

        [Required(ErrorMessage ="Enter Name")]
        public string SupplierName { get; set; }
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage ="Invalid Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm Password should be same as password")]
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }

        public int Id { get; set; }
    }
}