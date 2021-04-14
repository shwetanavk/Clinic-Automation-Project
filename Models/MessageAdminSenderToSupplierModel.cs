using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class MessageAdminSenderToSupplierModel
    {
        [Required(ErrorMessage = "Select Receiver")]
        public int ReceiverSupplierMemberID { get; set; }
        public int SenderAdminMemberID { get; set; }

        [Required(ErrorMessage = "Enter Subject")]
        public string MessageSubject { get; set; }

        [Required(ErrorMessage = "Type Message")]
        public string MessageDescription { get; set; }
        public DateTime MessageDate { get; set; }
        public List<SelectListItem> SupplierNameList { get; set; }

        public string AdminName { get; set; }
        public string SupplierName { get; set; }

        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}