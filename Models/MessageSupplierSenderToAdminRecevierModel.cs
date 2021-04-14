using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class MessageSupplierSenderToAdminRecevierModel
    {
        
        [Required(ErrorMessage = "Select Receiver!")]
        public int ReceiverAdminMemberId { get; set; }
        //  [Required(ErrorMessage = "Select Receiver!")]
        public int SenderSupplierMemberId { get; set; }
        [Required(ErrorMessage = "Enter Subject")]
        public String MessageSubject { get; set; }
        [Required(ErrorMessage = "Type a message")]
        public string MessageDescription { get; set; }
        public DateTime MessageDate { get; set; }
        public List<SelectListItem> AdminNameList { get; set; }
        //public List<SelectListItem> DoctorNameList { get; set; }
        public string SupplierName { get; set; }
        public string AdminName { get; set; }

        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}