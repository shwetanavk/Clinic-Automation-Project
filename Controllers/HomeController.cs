using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() //Added this vide
        {
            return View();
        }

        public ActionResult About() //Added this vide
        {
            return View();
        }

        public ActionResult ViewProducts()
        {

            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<ProductInventoryModel> productInventories = new List<ProductInventoryModel>();

                var productData = from p in db.Products
                                  select new
                                  {
                                      p.ProductId,
                                      p.ProductName,
                                      p.ProductPrice,
                                  };

                foreach (var item in productData)
                {
                    productInventories.Add(new ProductInventoryModel
                    {
                        ProductID = item.ProductId,
                        ProductName = item.ProductName,
                        ProductPrice = item.ProductPrice
                    });
                }
                return View(productInventories);
            }
        }
    }
}