using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicAutomationProject.Models
{
    public class DoctorAppointmentModel
    {
        public DateTime Date { get; set; }
        public string SlotTime { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string PatientName { get; set; }
        public int  PatientAge { get; set; }
        public string PatientGender { get; set; }

        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public string AppointmentStatus { get; set; }

        public int MemberIDofDoctor { get; set; }

        public string DoctorName { get; set; }
    }
}