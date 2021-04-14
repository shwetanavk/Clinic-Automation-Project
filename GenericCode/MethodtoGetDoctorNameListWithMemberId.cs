using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodtoGetDoctorNameListWithMemberId
    {
        public static List<SelectListItem> GetDoctorNameListWithMemberId()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> DoctorList = new List<SelectListItem>();
                var getAllDoctorList = db.Doctors.ToList();
                foreach (var item in getAllDoctorList)
                {
                    DoctorList.Add(new SelectListItem { Text = item.DoctorName, Value = item.MemberId.ToString() });
                }
                return DoctorList;
            }
        }
    }
}