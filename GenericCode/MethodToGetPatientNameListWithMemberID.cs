using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetPatientNameListWithMemberID
    {
        public static List<SelectListItem> GetPatientNameListWithMemberID()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> patientList = new List<SelectListItem>();
                var getAllPatients = db.Patients.ToList();
                foreach (var item in getAllPatients)
                {
                    patientList.Add(new SelectListItem { Text = item.PatientName, Value = item.MemberId.ToString() });
                }
                return patientList;
            }
        }
    }
}