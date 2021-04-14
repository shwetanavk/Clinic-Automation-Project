using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetSlotTime
    {
        public static List<SelectListItem> GetSlotTime()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SelectListItem> SlotTime = new List<SelectListItem>();
                var getAllSlots = db.Slots.ToList();
                foreach (var item in getAllSlots)
                {
                    SlotTime.Add(new SelectListItem { Text = item.SlotTime, Value = item.SlotID.ToString() });
                }
                return SlotTime;
            }
        }
    }
}