using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodtoGetSupplierList
    {
        public static List<SelectListItem> GetSupplierListNames()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> SupplierList = new List<SelectListItem>();
                var getAllSuppliers = db.Suppliers.ToList();
                foreach (var item in getAllSuppliers)
                {
                    SupplierList.Add(new SelectListItem { Text = item.SupplierName, Value = item.SupplierId.ToString() });
                }
                return SupplierList;
            }
        }
    }
}