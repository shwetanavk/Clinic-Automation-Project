using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class ProductInventoryModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public int OrderId { get; set; }

        [Required (ErrorMessage = "Select Order Quantity")]
        public int OrderQuantityID { get; set; }
        public List<SelectListItem> OrderQuantityList { get; set; }
        public int AdminId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Select Supplier")]
        public int SupplierID { get; set; }
        public List<SelectListItem> SupplierList { get; set; }

        public int OrderQuantityValue { get; set; }

        public string AdminName { get; set; }
        public string supplierName { get; set; }
    }
}