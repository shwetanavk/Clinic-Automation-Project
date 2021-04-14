using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class PatientAppointmentModel
    {
        public int AppoinmentId { get; set; }


        [Required(ErrorMessage = "Enter Subject")]
        public String AppointmentSubject { get; set; }


        [Required(ErrorMessage = "Enter Description")]
        public String AppointmentDescription { get; set; }


        [Required(ErrorMessage = "Enter Date")]
        public DateTime AppointmentDate { get; set; }


        [Required(ErrorMessage = "Enter Time Slot")]
        public int SlotID { get; set; }
        public List<SelectListItem> SlotTime { get; set; }

        public String AppointmentStatus { get; set; }



        [Required(ErrorMessage = "Select Doctor")]
        public int DoctorId { get; set; }
        public List<SelectListItem> DoctorNames { get; set; }


        public int PatientId { get; set; }
        public string DoctorSpecialization { get; set; }


        //[Required(ErrorMessage = "Select Specialization!")]
        public int DoctorSpecializationId { get; set; }


        public List<SelectListItem> SpecializationName { get; set; }

    }
}