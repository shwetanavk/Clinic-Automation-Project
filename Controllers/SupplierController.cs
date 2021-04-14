using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Controllers
{

    [CustomAuthorizeForRoles(Roles = "Supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SupplierLogin()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var dataget = db.Suppliers.SingleOrDefault(a => a.MemberId == MemberId);
                SupplierModel mySupplier = new SupplierModel();
                mySupplier.MemberId = dataget.MemberId;
                mySupplier.RoleId = dataget.Login.RoleId;
                mySupplier.Id = dataget.SupplierId;
                mySupplier.SupplierName = dataget.SupplierName;
                mySupplier.Email = dataget.Login.Email;
                mySupplier.Password = dataget.Login.Password;
                mySupplier.ConfirmPassword = dataget.Login.Password;
                return View(mySupplier);
            }
        }

        public ActionResult ViewOrderRequests()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<ProductInventoryModel> supplierOrder = new List<ProductInventoryModel>();
                var getSupplier = db.Suppliers.SingleOrDefault(a => a.MemberId == MemberId);

                var orderData = from o in db.Orders
                                join p in db.Products on o.ProductId equals p.ProductId
                                join a in db.Admins on o.AdminId equals a.AdminId
                                join q in db.QuantityTables on o.OrderQuantityID equals q.OrderQuantityID
                                join s in db.Suppliers on o.SupplierId equals s.SupplierId
                                where (s.SupplierId == getSupplier.SupplierId)
                                select new
                                {
                                    o.OrderId,
                                    p.ProductName,
                                    q.OrderQuantity,
                                    a.AdminName,
                                    o.OrderDate,
                                    o.OrderStatus,
                                    o.OrderNumber,
                                };


                foreach (var item in orderData)
                {
                    supplierOrder.Add(new ProductInventoryModel
                    {
                        OrderId = item.OrderId,
                        ProductName = item.ProductName,
                        OrderQuantityValue = item.OrderQuantity,
                        AdminName = item.AdminName,
                        OrderDate = item.OrderDate,
                        OrderStatus = item.OrderStatus,
                        OrderNumber = item.OrderNumber
                    });
                }
                return View(supplierOrder);
            }
        }
        public ActionResult AcceptOrder(int OrdID)
        {
            int MemId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                //have to update delivery table too
                var getOrders = db.Orders.FirstOrDefault(a => a.OrderId == OrdID);
                getOrders.OrderStatus = "Accepted";

                var getProduct = db.Products.FirstOrDefault(a => a.ProductId == getOrders.ProductId);
                getProduct.ProductStock += getOrders.QuantityTable.OrderQuantity;

                var getSupplier = db.Suppliers.FirstOrDefault(a => a.MemberId == MemId);

                DateTime myDeliveryDate = DateTime.Now.AddDays(10);

                db.Deliveries.Add(new Delivery { OrderId = OrdID, SupplierId = getSupplier.SupplierId, DeliveryDate = myDeliveryDate });

                ViewBag.deliveryDate = myDeliveryDate.ToString("dd-MMM-yyyy");
                db.SaveChanges();
                return View();
            }
        }
        public ActionResult RejectOrder(int OrdID)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Orders.FirstOrDefault(a => a.OrderId == OrdID);
                getdata.OrderStatus = "Rejected";
                db.SaveChanges();
                return View();
            }
        }


        [HttpGet]
        public ActionResult CreateMessage()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                MessageSupplierSenderToAdminRecevierModel messageSupplierSenderToAdminRecevierModel = new MessageSupplierSenderToAdminRecevierModel();
                var getdata = GenericCode.MethodtoGetAdminNameListWithMemberId.GetAdminNameListWithMemberId();
                messageSupplierSenderToAdminRecevierModel.AdminNameList = getdata;
                return View(messageSupplierSenderToAdminRecevierModel);
            }
        }
        [HttpPost]
        public ActionResult CreateMessage(MessageSupplierSenderToAdminRecevierModel Model)
        {
            if (!ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    MessageSupplierSenderToAdminRecevierModel messageSupplierSenderToAdminRecevierModel = new MessageSupplierSenderToAdminRecevierModel();
                    var getdata = GenericCode.MethodtoGetAdminNameListWithMemberId.GetAdminNameListWithMemberId();
                    messageSupplierSenderToAdminRecevierModel.AdminNameList = getdata;
                    return View(messageSupplierSenderToAdminRecevierModel);
                }
            }
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                //var getdata = db.Doctors.FirstOrDefault(a => a.MemberId == membID);
                Message obj = new Message();
                obj.SenderId = membID;
                obj.ReceiverId = Model.ReceiverAdminMemberId;
                obj.MessageSubject = Model.MessageSubject;
                obj.MessageDescription = Model.MessageDescription;
                obj.MessageDate = DateTime.Now;
                db.Messages.Add(obj);
                db.SaveChanges();
                var getpatient = db.Admins.FirstOrDefault(x => x.MemberId == Model.ReceiverAdminMemberId);
                ViewBag.MessageSend = getpatient.AdminName;
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
                List<MessageAdminSenderToSupplierModel> viewMessagesFromAdmin = new List<MessageAdminSenderToSupplierModel>();
                var getMessageReceived = from m in db.Messages
                                         join s in db.Suppliers on m.ReceiverId equals s.MemberId
                                         join a in db.Admins on m.SenderId equals a.MemberId
                                         orderby m.MessageDate descending
                                         where (s.MemberId == MemberId)
                                         select new
                                         {
                                             s.SupplierName,
                                             a.AdminName,
                                             m.MessageSubject,
                                             m.MessageDescription,
                                             m.MessageDate
                                         };


                foreach (var item in getMessageReceived)
                {
                    viewMessagesFromAdmin.Add(new MessageAdminSenderToSupplierModel
                    {
                        ReceiverName = item.SupplierName,
                        SenderName = item.AdminName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = item.MessageDate
                    });
                }

                var getMessageSent = from m in db.Messages
                                         join s in db.Suppliers on m.SenderId equals s.MemberId
                                         join a in db.Admins on m.ReceiverId equals a.MemberId
                                         orderby m.MessageDate descending
                                         where (s.MemberId == MemberId)
                                         select new
                                         {
                                             s.SupplierName,
                                             a.AdminName,
                                             m.MessageSubject,
                                             m.MessageDescription,
                                             m.MessageDate
                                         };


                foreach (var item in getMessageSent)
                {
                    viewMessagesFromAdmin.Add(new MessageAdminSenderToSupplierModel
                    {
                        SenderName = item.SupplierName,
                        ReceiverName = item.AdminName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = item.MessageDate
                    });
                }
                return View(viewMessagesFromAdmin);
            }
        }
    }
}