using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetSupplierNameListWithMemberID
    {
        public static List<SelectListItem> GetSupplierNameListWithMemberID()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> SupplierList = new List<SelectListItem>();
                var getAllSupplierss = db.Suppliers.ToList();
                foreach (var item in getAllSupplierss)
                {
                    SupplierList.Add(new SelectListItem { Text = item.SupplierName, Value = item.MemberId.ToString() });
                }
                return SupplierList;
            }
        }
    }
}