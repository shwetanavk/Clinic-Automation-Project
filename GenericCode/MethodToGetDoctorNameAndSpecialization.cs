using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetDoctorNameAndSpecialization
    {
        public static List<SelectListItem> GetDocNamesAndSpec()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> dAndS = new List<SelectListItem>();
                var getAllRoles = from s in db.Specializations
                                  join d in db.Doctors on s.SpecializationId equals d.SpecializationId
                                  orderby d.DoctorName
                                  select new { s.SpecialzationName, d.DoctorName, d.DoctorId };
                foreach (var item in getAllRoles)
                {
                    string dname = item.DoctorName + "(" + item.SpecialzationName + ")";
                    dAndS.Add(new SelectListItem { Text = dname, Value = item.DoctorId.ToString() });
                }
                return dAndS;
            }
        }
    }
}