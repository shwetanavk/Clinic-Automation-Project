using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetSpecialization
    {
        public static List<SelectListItem> GetSpecializationNames()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> spec = new List<SelectListItem>();
                var getAllRoles = db.Specializations.ToList();
                foreach (var item in getAllRoles)
                {
                    spec.Add(new SelectListItem { Text = item.SpecialzationName, Value = item.SpecializationId.ToString() });
                }
                return spec;
            }
        }
    }
}