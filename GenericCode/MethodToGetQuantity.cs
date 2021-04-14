using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetQuantity
    {
        public static List<SelectListItem> GetQuantityList()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> QuantityList = new List<SelectListItem>();
                var getAllQuantity = db.QuantityTables.ToList();
                foreach (var item in getAllQuantity)
                {
                    QuantityList.Add(new SelectListItem { Text = item.OrderQuantity.ToString(), Value = item.OrderQuantityID.ToString() });
                }
                return QuantityList;
            }
        }
    }
}
