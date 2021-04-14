using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicAutomationProject.Models
{
    public class PatientModel
    {
        [Required(ErrorMessage = "Enter Name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Enter Age")]
        public int PatientAge { get; set; }


        [Required(ErrorMessage = "Enter Gender")]
        public string PatientGender { get; set; }
        public int MemberId { get; set; }


        [Required(ErrorMessage = "Enter your email.")]
        [EmailAddress(ErrorMessage = "InvalidFormat")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password should be same as password")]
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }

        public int Id { get; set; }


    }
}