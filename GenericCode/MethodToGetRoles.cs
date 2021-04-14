using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicAutomationProject.Models;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetRoles
    {
        public static List<SelectListItem> GetRoleId()
        {
            using(ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> roles = new List<SelectListItem>();
                var getAllRoles = db.RoleTables.ToList();
                foreach (var item in getAllRoles)
                {
                    roles.Add(new SelectListItem {Text = item.RoleName, Value = item.RoleId.ToString()});
                }
                return roles;
            }
        }
    }
}