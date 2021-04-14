using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class MessageDoctorReplyToAMessage
    {
        public int ReceiverPatientMemberID { get; set; }
        public int SenderDoctorMemberID { get; set; }
        public string MessageSubject { get; set; }
        public string MessageDescription { get; set; }
        public DateTime MessageDate { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }

        public string SenderName { get; set; }
        public string ReceiverName { get; set; }

        public int MessageID { get; set; }
        [Required(ErrorMessage = "Enter Subject")]
        public string ReplyMessageSubject { get; set; }
        [Required(ErrorMessage = "Type Message")]
        public string ReplyMessageDescription { get; set; }
    }
}