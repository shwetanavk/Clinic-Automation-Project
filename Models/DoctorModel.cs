using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace ClinicAutomationProject.Models
{
    public class DoctorModel
    {

        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Enter name.")]
        public string DoctorName { get; set; }


        [Required(ErrorMessage = "Select Specialization!")]
        public int SpecializationId { get; set; }
        public int MemberId { get; set; }


        [Required(ErrorMessage = "Enter your email.")]
        [EmailAddress(ErrorMessage = "InvalidFormat")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm Password should be same as password")]
        
        public string ConfirmPassword { get; set; }
        public List<SelectListItem> SpecTypes { get; set; }

        public string SpecialzationName { get; set; }
        public int RoleId { get; set; }
    }
}