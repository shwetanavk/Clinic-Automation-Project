using ClinicAutomationProject.GenericCode;
using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClinicAutomationProject.Controllers
{

    [CustomAuthorizeForRoles(Roles = "Admin")]//, Doctor,    Supplier, Patient")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }




        public ActionResult ViewProductInventory()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<ProductInventoryModel> productInventories  = new List<ProductInventoryModel>();

                var productData = from p in db.Products
                                  //join o in db.Orders on p.ProductId equals o.ProductId
                                  select new
                                  {
                                      p.ProductId,
                                      p.ProductName,
                                      p.ProductPrice,
                                      p.ProductStock,
                                  };




                foreach (var item in productData)
                {
                    //var getOrderStatus = db.Orders.SingleOrDefault(a => a.ProductId == item.ProductId);
                    //string status = "";
                    //if (getOrderStatus != null)
                    //{
                    //    if(getOrderStatus.OrderStatus == "Requested")
                    //        status = getOrderStatus.OrderStatus;
                    //    else
                    //    {
                    //        status = "";
                    //    }
                    //}


                    productInventories.Add(new ProductInventoryModel
                    {
                        ProductID = item.ProductId,
                        ProductName = item.ProductName,
                        ProductPrice = item.ProductPrice,
                        ProductStock = item.ProductStock,
                        //OrderStatus = status

                    });
                }
                return View(productInventories);
            }
        }

        

        [HttpGet]
        public ActionResult PlaceOrder(int productID)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                ProductInventoryModel model = new ProductInventoryModel();
                var getdata = db.Products.FirstOrDefault(a => a.ProductId == productID);
                model.ProductID = getdata.ProductId;
                model.ProductName = getdata.ProductName;
                model.ProductPrice = getdata.ProductPrice;
                model.ProductStock = getdata.ProductStock;

                model.OrderNumber = GenericCode.MethodToGetRandomOrderNumber.GenerateOrderNumber();
                model.SupplierList = GenericCode.MethodtoGetSupplierList.GetSupplierListNames();
                model.OrderQuantityList = GenericCode.MethodToGetQuantity.GetQuantityList();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult PlaceOrder(ProductInventoryModel model)
        {

            if (!ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    var getdata = db.Products.FirstOrDefault(a => a.ProductId == model.ProductID);
                    model.ProductID = getdata.ProductId;
                    model.ProductName = getdata.ProductName;
                    model.ProductPrice = getdata.ProductPrice;
                    model.ProductStock = getdata.ProductStock;


                    model.OrderNumber = GenericCode.MethodToGetRandomOrderNumber.GenerateOrderNumber();
                    model.SupplierList = GenericCode.MethodtoGetSupplierList.GetSupplierListNames();
                    model.OrderQuantityList = GenericCode.MethodToGetQuantity.GetQuantityList();
                    return View(model);
                }
            }

            int membID = Convert.ToInt32(Session["MemberId"]);

            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getData = db.Admins.FirstOrDefault(a => a.MemberId == membID);

                Order ord = new Order();
                ord.ProductId = model.ProductID;
                ord.OrderQuantityID = model.OrderQuantityID;
                ord.AdminId = getData.AdminId;
                ord.OrderDate = DateTime.Now;
                ord.OrderStatus = "Requested";
                ord.SupplierId = model.SupplierID;
                ord.OrderNumber = model.OrderNumber;

                db.Orders.Add(ord);
                db.SaveChanges();
                return RedirectToAction("OrderPlacedSuccessfully", "Admin");
            }
        }


        public ActionResult OrderPlacedSuccessfully()
        {
            return View();
        }




        public ActionResult ViewOrders()
        {
            int membID = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var dataget = db.Admins.FirstOrDefault(a => a.MemberId == membID);
                List<ProductInventoryModel> viewOrderToSuppliers = new List<ProductInventoryModel>();

                var placedOrderData = from p in db.Products
                                      join o in db.Orders on p.ProductId equals o.ProductId
                                      join q in db.QuantityTables on o.OrderQuantityID equals q.OrderQuantityID
                                      join s in db.Suppliers on o.SupplierId equals s.SupplierId
                                      select new
                                  {
                                      p.ProductName,
                                      q.OrderQuantity,
                                      dataget.AdminName,
                                      o.OrderDate,
                                      o.OrderStatus,
                                      o.OrderNumber,
                                      s.SupplierName
                                  };

                foreach (var item in placedOrderData)
                {
                    viewOrderToSuppliers.Add(new ProductInventoryModel
                    {
                        ProductName = item.ProductName,
                        OrderQuantityValue = item.OrderQuantity,
                        AdminName = dataget.AdminName,
                        OrderDate = item.OrderDate,
                        OrderStatus = item.OrderStatus,
                        OrderNumber = item.OrderNumber,
                        supplierName = item.SupplierName
                    }); ; ;
                    ; ;
                }
                return View(viewOrderToSuppliers);
            }
        }






        [HttpGet]
        public ActionResult CreateMessage() // changing this
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                MessageAdminSenderToSupplierModel model = new MessageAdminSenderToSupplierModel();
                var getdata = GenericCode.MethodToGetSupplierNameListWithMemberID.GetSupplierNameListWithMemberID();
                model.SupplierNameList = getdata;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateMessage(MessageAdminSenderToSupplierModel model)
        {
            if (!ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    var getdata = GenericCode.MethodToGetSupplierNameListWithMemberID.GetSupplierNameListWithMemberID();
                    model.SupplierNameList = getdata;
                    return View(model);
                }
            }

            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);

                Message obj = new Message();
                obj.SenderId = membID;
                obj.ReceiverId = model.ReceiverSupplierMemberID;
                obj.MessageSubject = model.MessageSubject;
                obj.MessageDescription = model.MessageDescription;
                obj.MessageDate = DateTime.Now;

                db.Messages.Add(obj);
                db.SaveChanges();

                var getSupplier = db.Suppliers.FirstOrDefault(a => a.MemberId == model.ReceiverSupplierMemberID);
                ViewBag.MessageSentTo = " " + getSupplier.SupplierName + " ";
                return View("MessageSent");
            }
        }
        public ActionResult MessageSent()
        {
            return View();
        }

        public ActionResult ViewMessage()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<MessageSupplierSenderToAdminRecevierModel> viewMessagesFromSupplier = new List<MessageSupplierSenderToAdminRecevierModel>();
                var getMessageReceived = from m in db.Messages
                                         join a in db.Admins on m.ReceiverId equals a.MemberId
                                         join s in db.Suppliers on m.SenderId equals s.MemberId
                                         orderby m.MessageDate descending
                                         where (a.MemberId == MemberId)
                                         select new
                                         {
                                             a.AdminName,
                                             s.SupplierName,
                                             m.MessageSubject,
                                             m.MessageDescription,
                                             m.MessageDate
                                         };
                foreach (var item in getMessageReceived)
                {
                    viewMessagesFromSupplier.Add(new MessageSupplierSenderToAdminRecevierModel
                    {
                        ReceiverName = item.AdminName,
                        SenderName = item.SupplierName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = (DateTime)item.MessageDate
                    });
                }

                var getMessageSent = from m in db.Messages
                                         join a in db.Admins on m.SenderId equals a.MemberId
                                         join s in db.Suppliers on m.ReceiverId equals s.MemberId
                                         orderby m.MessageDate descending
                                         where (a.MemberId == MemberId)
                                         select new
                                         {
                                             a.AdminName,
                                             s.SupplierName,
                                             m.MessageSubject,
                                             m.MessageDescription,
                                             m.MessageDate
                                         };
                foreach (var item in getMessageSent)
                {
                    viewMessagesFromSupplier.Add(new MessageSupplierSenderToAdminRecevierModel
                    {
                        SenderName = item.AdminName,
                        ReceiverName = item.SupplierName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = (DateTime)item.MessageDate
                    });
                }
                return View(viewMessagesFromSupplier);
            }
        }

        /// //////////////////////////////////////////////////////////////////////////////     DOCTOR    ////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public ActionResult ManageDoctor()
        {
            DoctorModel model = new DoctorModel();
            model.SpecTypes = GenericCode.MethodToGetSpecialization.GetSpecializationNames();
            return View(model);
        }

        //public ActionResult ManageDoctor(DoctorModel model)
        //{
        //    model.SpecTypes = GenericCode.MethodToGetSpecialization.GetSpecializationNames();
        //    return View(model);
        //}

        [HttpPost]
        public ActionResult InsertDoctor(DoctorModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SpecTypes = GenericCode.MethodToGetSpecialization.GetSpecializationNames();
                return View("ManageDoctor", model);
            }

            if (model.DoctorId > 0)
            {
                //Update
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    var getdata = db.Doctors.FirstOrDefault(a => a.DoctorId == model.DoctorId);
                    getdata.DoctorName = model.DoctorName;
                    getdata.SpecializationId = model.SpecializationId;
                    getdata.Login.Email = model.Email;
                    getdata.Login.Password = model.Password;
                    getdata.Login.RoleId = 2;
                    db.SaveChanges();

                    return RedirectToAction("ViewDoctor", "Admin");
                }
            }
            else
            {   //Insert
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    Login obj = new Login();
                    obj.Email = model.Email;
                    obj.Password = model.Password;
                    obj.RoleId = 2;

                    obj.Doctors.Add(new Doctor { DoctorName = model.DoctorName, SpecializationId = model.SpecializationId });

                    db.Logins.Add(obj);
                    db.SaveChanges();

                    return RedirectToAction("InsertSucessfully", "Admin");
                }
            }
        }

        public ActionResult InsertSucessfully()
        {
            return View();
        }

        public ActionResult ViewDoctor()
        {
            using(ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<DoctorModel> model = new List<DoctorModel>();
                var getdata = db.Doctors.Include("Login").Include("Specialization").Select(a => new {a.DoctorId, a.DoctorName, a.SpecializationId, a.Specialization.SpecialzationName, a.Login.Email, a.Login.Password, a.MemberId, a.Login.RoleId});

                foreach (var item in getdata)
                {
                    model.Add(new DoctorModel
                    {
                        DoctorId = item.DoctorId,
                        DoctorName = item.DoctorName,
                        SpecializationId = item.SpecializationId,
                        SpecialzationName = item.SpecialzationName,
                        Email = item.Email,
                        Password = item.Password,
                        MemberId = item.MemberId,
                        RoleId = item.RoleId
                    }); ;
                }
                return View(model);
            }
        }


        public ActionResult EditDoctor(int docId)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                DoctorModel model = new DoctorModel();
                var getdata = db.Doctors.FirstOrDefault(a => a.DoctorId == docId);
                model.DoctorId = getdata.DoctorId;
                model.DoctorName = getdata.DoctorName;
                model.SpecializationId = getdata.SpecializationId;
                model.Email = getdata.Login.Email;
                model.Password = getdata.Login.Password;
                model.MemberId = getdata.MemberId;
                model.SpecTypes = GenericCode.MethodToGetSpecialization.GetSpecializationNames();

                return View("ManageDoctor", model);
            }
        }

        public ActionResult DeleteDoctor(int docId)
        {
            using(ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Doctors.SingleOrDefault(a => a.DoctorId == docId);
                db.Doctors.Remove(getdata);

                var getLogin = db.Logins.SingleOrDefault(a => a.MemberId == getdata.MemberId);
                db.Logins.Remove(getLogin);

                db.SaveChanges();
                return RedirectToAction("ViewDoctor");
            }
        }


        /// //////////////////////////////////////////////////////////////////////////////     PATIENT    ////////////////////////////////////////////////////////////////////////////


        [HttpGet]
        public ActionResult ManagePatient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertPatient(PatientModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManagePatient", model);
            }
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                Login obj = new Login();
                obj.Email = model.Email;
                obj.Password = model.Password;
                obj.RoleId = 4;

                obj.Patients.Add(new Patient { PatientName = model.PatientName, Age = model.PatientAge, Gender = model.PatientGender });

                db.Logins.Add(obj);
                db.SaveChanges();


                return RedirectToAction("InsertSucessfullyPatient", "Admin");
            }
        }
    

        public ActionResult InsertSucessfullyPatient()
        {
            return View();
        }
        public ActionResult ViewPatient()
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    List<PatientModel> model = new List<PatientModel>();
                    var getdata = db.Patients.Include("Login").Select(a => new { a.PatientId, a.PatientName, a.Age, a.Gender, a.Login.Email, a.Login.Password, a.MemberId, a.Login.RoleId });

                    foreach (var item in getdata)
                    {
                        model.Add(new PatientModel
                        {
                            Id = item.PatientId,
                            PatientName = item.PatientName,
                            PatientAge = item.Age,
                            PatientGender = item.Gender,
                            Email = item.Email,
                            Password = item.Password,
                            MemberId = item.MemberId,
                            RoleId = item.RoleId
                        }); ;
                    }
                    return View(model);
                }
            }


        public ActionResult EditPatient(int patId)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                PatientModel model = new PatientModel();
                var getdata = db.Patients.FirstOrDefault(a => a.PatientId == patId);
                model.Id = getdata.PatientId;
                model.PatientName = getdata.PatientName;
                model.PatientAge = getdata.Age;
                model.PatientGender = getdata.Gender;
                model.Email = getdata.Login.Email;
                model.Password = getdata.Login.Password;
                model.MemberId = getdata.MemberId;
                //return View("ManagePatient", model);
                return View("ManagePatientEdit", model);
            }
        }

        [HttpGet]
        public ActionResult ManagePatientEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManagePatientEdit(PatientModel model)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Patients.FirstOrDefault(a => a.PatientId == model.Id);
                getdata.PatientName = model.PatientName;
                getdata.Age = model.PatientAge;
                getdata.Gender = model.PatientGender;
                getdata.Login.Email = model.Email;
                getdata.Login.Password = model.Password;
                getdata.Login.RoleId = 4;
                db.SaveChanges();

                return RedirectToAction("ViewPatient", "Admin");
            }

        }
        public ActionResult DeletePatient(int patId)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Patients.SingleOrDefault(a => a.PatientId == patId);
                db.Patients.Remove(getdata);

                var getLogin = db.Logins.SingleOrDefault(a => a.MemberId == getdata.MemberId);
                db.Logins.Remove(getLogin);
                db.SaveChanges();
                return RedirectToAction("ViewPatient");
            }
        }



        /// //////////////////////////////////////////////////////////////////////////////     SUPPLIER    ////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public ActionResult ManageSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertSupplier(SupplierModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageSupplier", model);
            }


            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                Login obj = new Login();
                obj.Email = model.Email;
                obj.Password = model.Password;
                obj.RoleId = 3;

                obj.Suppliers.Add(new Supplier { SupplierName = model.SupplierName});

                db.Logins.Add(obj);
                db.SaveChanges();

                return RedirectToAction("InsertSucessfullySupplier", "Admin");
            }
        }
                

        public ActionResult InsertSucessfullySupplier()
        {
            return View();
        }

         /// <summary>
         /// ///////////////////
         /// </summary>
         /// <returns></returns>
        public ActionResult ViewSupplier()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<SupplierModel> model = new List<SupplierModel>();
                var getdata = db.Suppliers.Include("Login").Select(a => new { a.SupplierId, a.SupplierName, a.Login.Email, a.Login.Password, a.MemberId, a.Login.RoleId });

                foreach (var item in getdata)
                {
                    model.Add(new SupplierModel
                    {
                        Id = item.SupplierId,
                        SupplierName = item.SupplierName,
                        Email = item.Email,
                        Password = item.Password,
                        MemberId = item.MemberId,
                        RoleId = item.RoleId
                    }); ;
                }
                return View(model);
            }
        }


        public ActionResult EditSupplier(int supplierId)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                SupplierModel model = new SupplierModel();
                var getdata = db.Suppliers.FirstOrDefault(a => a.SupplierId == supplierId);
                model.Id = getdata.SupplierId;
                model.SupplierName = getdata.SupplierName;
                model.Email = getdata.Login.Email;
                model.Password = getdata.Login.Password;
                model.MemberId = getdata.MemberId;
                return View("ManageSupplierEdit", model);
            }
        }

        [HttpGet]
        public ActionResult ManageSupplierEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageSupplierEdit(SupplierModel model)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Suppliers.FirstOrDefault(a => a.SupplierId == model.Id);
                getdata.SupplierName = model.SupplierName;
                getdata.Login.Email = model.Email;
                getdata.Login.Password = model.Password;
                getdata.Login.RoleId = 3;
                db.SaveChanges();

                return RedirectToAction("ViewSupplier", "Admin");
            }

        }
        public ActionResult DeleteSupplier(int supplierId)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Suppliers.SingleOrDefault(a => a.SupplierId == supplierId);
                db.Suppliers.Remove(getdata);

                var getLogin = db.Logins.SingleOrDefault(a => a.MemberId == getdata.MemberId);
                db.Logins.Remove(getLogin);
                db.SaveChanges();
                return RedirectToAction("ViewSupplier");
            }
        }




    }
}