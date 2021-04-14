using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodtoGetAdminNameListWithMemberId
    {
        public static List<SelectListItem> GetAdminNameListWithMemberId()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> AdminList = new List<SelectListItem>();
                var getAllAdminList = db.Admins.ToList();
                foreach (var item in getAllAdminList)
                {
                    AdminList.Add(new SelectListItem { Text = item.AdminName, Value = item.MemberId.ToString() });
                }
                return AdminList;
            }
        }
    }
}