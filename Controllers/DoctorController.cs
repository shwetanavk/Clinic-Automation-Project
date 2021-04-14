using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Controllers
{
    [CustomAuthorizeForRoles(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DoctorLogin()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var dataget = db.Doctors.SingleOrDefault(a => a.MemberId == MemberId);
                DoctorModel myDoctor = new DoctorModel();
                myDoctor.MemberId = dataget.MemberId;
                myDoctor.RoleId = dataget.Login.RoleId;
                myDoctor.DoctorId = dataget.DoctorId;
                myDoctor.DoctorName = dataget.DoctorName;
                myDoctor.SpecializationId = dataget.Specialization.SpecializationId;
                myDoctor.SpecialzationName = dataget.Specialization.SpecialzationName;
                myDoctor.Email = dataget.Login.Email;
                myDoctor.Password = dataget.Login.Password;
                myDoctor.ConfirmPassword = dataget.Login.Password;
                return View(myDoctor);
            }
        }
        public ActionResult ViewAppointment()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<DoctorAppointmentModel> doctorAppointment = new List<DoctorAppointmentModel>();

                var appointmentData = from a in db.Appointments
                                      join p in db.Patients on a.PatientId equals p.PatientId
                                      join st in db.Slots on a.SlotId equals st.SlotID
                                      join d in db.Doctors on a.DoctorId equals d.DoctorId
                                      orderby a.AppointmentDate
                                      where (d.MemberId == MemberId)
                                      select new
                                      {
                                          a.AppoinmentId,
                                          a.AppointmentDate,
                                          st.SlotTime,
                                          a.AppointmentSubject,
                                          a.AppointmentDescription,
                                          p.PatientName,
                                          p.Age,
                                          p.Gender,
                                          a.AppointmentStatus,
                                          d.DoctorId
                                      };
                                      

                foreach (var item in appointmentData)
                {
                    doctorAppointment.Add(new DoctorAppointmentModel
                    {
                        AppointmentID = item.AppoinmentId,
                        Date = item.AppointmentDate,
                        SlotTime = item.SlotTime,
                        Subject = item.AppointmentSubject,
                        Description = item.AppointmentDescription,
                        PatientName = item.PatientName,
                        PatientAge = item.Age,
                        PatientGender = item.Gender,
                        AppointmentStatus = item.AppointmentStatus,
                        DoctorID = item.DoctorId
                    }); ;
                }
                return View(doctorAppointment);
            }
        }
        public ActionResult AcceptAppointment(int appointID)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Appointments.FirstOrDefault(a => a.AppoinmentId == appointID);
                getdata.AppointmentStatus = "Accepted";
                db.SaveChanges();
                return View();
            }
        }
        public ActionResult RejectAppointment(int appointID)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var getdata = db.Appointments.FirstOrDefault(a => a.AppoinmentId == appointID);
                getdata.AppointmentStatus = "Rejected";
                db.SaveChanges();
                return View();
            }
        }

        public ActionResult ViewAllPatientHistory()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<DoctorAppointmentModel> doctorAppointment = new List<DoctorAppointmentModel>();

                var appointmentData = from a in db.Appointments
                                      join p in db.Patients on a.PatientId equals p.PatientId
                                      join st in db.Slots on a.SlotId equals st.SlotID
                                      join d in db.Doctors on a.DoctorId equals d.DoctorId
                                      orderby p.PatientName
                                      orderby a.AppointmentDate

                                      select new
                                      {
                                          p.PatientName,
                                          p.Age,
                                          p.Gender,
                                          d.DoctorName,
                                          a.AppointmentSubject,
                                          a.AppointmentDescription,
                                          a.AppointmentDate,
                                          a.AppointmentStatus
                                      };

                foreach (var item in appointmentData)
                {
                    doctorAppointment.Add(new DoctorAppointmentModel
                    {
                        PatientName = item.PatientName,
                        PatientAge = item.Age,
                        PatientGender = item.Gender,
                        DoctorName = item.DoctorName,
                        Subject = item.AppointmentSubject,
                        Description = item.AppointmentDescription,
                        Date = item.AppointmentDate,
                        AppointmentStatus = item.AppointmentStatus
                    }); ;
                }
                return View(doctorAppointment);
            }
        }


        [HttpGet]
        public ActionResult CreateMessage()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                MessageDoctorSenderToPatientModel model = new MessageDoctorSenderToPatientModel();
                var getdata = GenericCode.MethodToGetPatientNameListWithMemberID.GetPatientNameListWithMemberID();
                model.PatientNameList = getdata;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateMessage(MessageDoctorSenderToPatientModel model)
        {
            if (!ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    var getdata = GenericCode.MethodToGetPatientNameListWithMemberID.GetPatientNameListWithMemberID();
                    model.PatientNameList = getdata;
                    return View(model);
                }
            }

            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                //var getdata = db.Doctors.FirstOrDefault(a => a.MemberId == membID);

                Message obj = new Message();
                obj.SenderId = membID;
                obj.ReceiverId = model.ReceiverPatientMemberID;
                obj.MessageSubject = model.MessageSubject;
                obj.MessageDescription = model.MessageDescription;
                obj.MessageDate = DateTime.Now;

                db.Messages.Add(obj);
                db.SaveChanges();

                var getPatient = db.Patients.FirstOrDefault(a => a.MemberId == model.ReceiverPatientMemberID);
                ViewBag.MessageSentTo = " " + getPatient.PatientName + " ";
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
                List<MessageSenderPatientToDoctorModel> viewMessagesFromPatient = new List<MessageSenderPatientToDoctorModel>();
                var getReceivedMessages = from m in db.Messages
                                        join d in db.Doctors on m.ReceiverId equals d.MemberId 
                                        join p in db.Patients on m.SenderId equals p.MemberId
                                        orderby m.MessageDate descending
                                        where (m.SenderId == MemberId || m.ReceiverId == MemberId )
                                        select new
                                        {
                                            p.PatientName,
                                            d.DoctorName,
                                            m.MessageSubject,
                                            m.MessageDescription,
                                            m.MessageDate,
                                            m.MessageId
                                        };


                var getSentMessages = from m in db.Messages
                                             join d in db.Doctors on m.SenderId equals d.MemberId
                                             join p in db.Patients on m.ReceiverId equals p.MemberId
                                             orderby m.MessageDate descending
                                             where (m.SenderId == MemberId || m.ReceiverId == MemberId)
                                             select new
                                             {
                                                 p.PatientName,
                                                 d.DoctorName,
                                                 m.MessageSubject,
                                                 m.MessageDescription,
                                                 m.MessageDate,
                                                 m.MessageId
                                             };

                foreach (var item in getReceivedMessages)
                {
                    viewMessagesFromPatient.Add(new MessageSenderPatientToDoctorModel
                    {
                        SenderName = item.PatientName,
                        ReceiverName = item.DoctorName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = (DateTime)item.MessageDate,
                        MessageID = item.MessageId
                        
                    });
                }

                foreach (var item in getSentMessages)
                {
                    viewMessagesFromPatient.Add(new MessageSenderPatientToDoctorModel
                    {
                        SenderName = item.DoctorName,
                        ReceiverName = item.PatientName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = (DateTime)item.MessageDate,
                        MessageID = item.MessageId
                    });
                }

                return View(viewMessagesFromPatient);
            }
        }

        [HttpGet]
        public ActionResult ReplyToAMessage(int messageID)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                MessageDoctorReplyToAMessage model = new MessageDoctorReplyToAMessage();
                var getdata = db.Messages.FirstOrDefault(a => a.MessageId == messageID);
                model.MessageSubject = getdata.MessageSubject;
                model.MessageDescription = getdata.MessageDescription;
                model.ReceiverPatientMemberID = getdata.SenderId;
                model.MessageID = getdata.MessageId;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult ReplyToAMessage(MessageDoctorReplyToAMessage model)
        {
            if (!ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    var getdata = db.Messages.FirstOrDefault(a => a.MessageId == model.MessageID);
                    model.MessageSubject = getdata.MessageSubject;
                    model.MessageDescription = getdata.MessageDescription;
                    return View(model);
                }
            }

                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                //var getdata = db.Doctors.FirstOrDefault(a => a.MemberId == membID);

                ReplyMessage obj = new ReplyMessage();
                obj.ReplySenderID = membID;
                obj.ReplyRecevierID = model.ReceiverPatientMemberID;
                obj.MessageSubject = model.ReplyMessageSubject;
                obj.MessageDescription = model.ReplyMessageDescription;
                obj.MessageDate = DateTime.Now;
                obj.MessageID = model.MessageID;
                db.ReplyMessages.Add(obj);
                db.SaveChanges();

                var getPatient = db.Patients.FirstOrDefault(a => a.MemberId == model.ReceiverPatientMemberID);
                ViewBag.MessageSentTo = " " + getPatient.PatientName + " ";
                return View("MessageSent");
            }
        }






        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>








        [HttpGet]
        public ActionResult CreateNewChatMessage()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                ZmsgPatientSendMessage patientSender = new ZmsgPatientSendMessage();
                var getdata = GenericCode.MethodtoGetDoctorNameListWithMemberId.GetDoctorNameListWithMemberId();
                patientSender.DoctorNameList = getdata;
                return View(patientSender);
            }
        }
        [HttpPost]
        public ActionResult CreateNewChatMessage(ZmsgPatientSendMessage Model)
        {
            if (!ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    var getdata = GenericCode.MethodtoGetDoctorNameListWithMemberId.GetDoctorNameListWithMemberId();
                    Model.DoctorNameList = getdata;
                    return View(Model);
                }
            }
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                ChatMessage obj = new ChatMessage();
                obj.SenderId = membID;
                obj.ReceiverId = Model.ReceiverDoctorMemberID;
                obj.MessageSubject = Model.MessageSubject;
                obj.MessageDescription = Model.MessageDescription;
                obj.MessageDate = DateTime.Now;
                obj.SubjectID = GenericCode.MethodToGetRandomOrderNumber.GenerateOrderNumber();
                db.ChatMessages.Add(obj);
                db.SaveChanges();

                var getDoctor = db.Doctors.FirstOrDefault(x => x.MemberId == Model.ReceiverDoctorMemberID);
                ViewBag.MessageSend = getDoctor.DoctorName;
                return View("MessageSent");
            }
        }






        public ActionResult ViewMessageNameList()
        {
            int MemberId = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<ZmsgPatientSendMessage> viewList = new List<ZmsgPatientSendMessage>();
                var getPatientList = from cm in db.ChatMessages
                                     join d in db.Doctors on cm.ReceiverId equals d.MemberId
                                     join p in db.Patients on cm.SenderId equals p.MemberId
                                     orderby cm.MessageDate descending
                                     where (cm.ReceiverId == MemberId)
                                     select new
                                     {
                                         p.PatientName,
                                         cm.SubjectID,
                                         cm.MessageSubject,
                                         cm.MessageDate
                                     };
                foreach (var item in getPatientList)
                {
                    viewList.Add(new ZmsgPatientSendMessage
                    {
                        //SenderName = item.PatientName,
                        //ReceiverName = item.DoctorName,
                        PatientName = item.PatientName,
                        MessageSubject = item.MessageSubject,
                        SubjectID = item.SubjectID,
                        
                        //MessageDescription = item.MessageDescription,
                        MessageDate = (DateTime)item.MessageDate,
                        //MessageID = item.MessageId

                    });
                }
                return View(viewList);
            }
        }

        public ActionResult OpenConversation(string subID)
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                List<ZmsgPatientSendMessage> viewList = new List<ZmsgPatientSendMessage>();
                var getPatientList = from cm in db.ChatMessages
                                     join d in db.Doctors on cm.ReceiverId equals d.MemberId
                                     join p in db.Patients on cm.SenderId equals p.MemberId
                                     orderby cm.MessageDate descending
                                     where (cm.SubjectID == subID)
                                     select new
                                     {
                                         p.PatientName,
                                         cm.SubjectID,
                                         cm.MessageSubject,
                                         cm.MessageDescription,
                                         cm.MessageDate
                                     };
                foreach (var item in getPatientList)
                {
                    viewList.Add(new ZmsgPatientSendMessage
                    {
                        //SenderName = item.PatientName,
                        //ReceiverName = item.DoctorName,
                        PatientName = item.PatientName,
                        MessageSubject = item.MessageSubject,
                        SubjectID = item.SubjectID,

                        MessageDescription = item.MessageDescription,
                        MessageDate = (DateTime)item.MessageDate,
                        //MessageID = item.MessageId

                    });
                }
                return View(viewList);
            }
        }

        public ActionResult ReplyToThisConveration()
        {
            return View();
        }
        //{
        //List<ZmsgPatientSendMessage> viewMessagesFromPatient = new List<ZmsgPatientSendMessage>();
        //var getReceivedMessages = from m in db.Messages
        //                          join d in db.Doctors on m.ReceiverId equals d.MemberId
        //                          join p in db.Patients on m.SenderId equals p.MemberId
        //                          orderby m.MessageDate descending
        //                          where (m.SenderId == MemberId || m.ReceiverId == MemberId)
        //                          select new
        //                          {
        //                              p.PatientName,
        //                              d.DoctorName,
        //                              m.MessageSubject,
        //                              m.MessageDescription,
        //                              m.MessageDate,
        //                              m.MessageId
        //                          };


        //var getSentMessages = from m in db.Messages
        //                      join d in db.Doctors on m.SenderId equals d.MemberId
        //                      join p in db.Patients on m.ReceiverId equals p.MemberId
        //                      orderby m.MessageDate descending
        //                      where (m.SenderId == MemberId || m.ReceiverId == MemberId)
        //                      select new
        //                      {
        //                          p.PatientName,
        //                          d.DoctorName,
        //                          m.MessageSubject,
        //                          m.MessageDescription,
        //                          m.MessageDate,
        //                          m.MessageId
        //                      };

        //foreach (var item in getReceivedMessages)
        //{
        //    viewMessagesFromPatient.Add(new MessageSenderPatientToDoctorModel
        //    {
        //        SenderName = item.PatientName,
        //        ReceiverName = item.DoctorName,
        //        MessageSubject = item.MessageSubject,
        //        MessageDescription = item.MessageDescription,
        //        MessageDate = (DateTime)item.MessageDate,
        //        MessageID = item.MessageId

        //    });
        //}

        //foreach (var item in getSentMessages)
        //{
        //    viewMessagesFromPatient.Add(new MessageSenderPatientToDoctorModel
        //    {
        //        SenderName = item.DoctorName,
        //        ReceiverName = item.PatientName,
        //        MessageSubject = item.MessageSubject,
        //        MessageDescription = item.MessageDescription,
        //        MessageDate = (DateTime)item.MessageDate,
        //        MessageID = item.MessageId
        //    });
        //}

        //return View(viewMessagesFromPatient);

        //}

    }
}