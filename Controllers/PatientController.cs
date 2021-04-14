using ClinicAutomationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Controllers
{


    [CustomAuthorizeForRoles(Roles = "Patient")]
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientLogin()
        {
            int membID = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var dataget = db.Patients.FirstOrDefault(a => a.MemberId == membID);

                PatientModel myPatient = new PatientModel();
                myPatient.MemberId = dataget.MemberId;
                myPatient.RoleId = dataget.Login.RoleId;
                myPatient.Id = dataget.PatientId;
                myPatient.PatientName = dataget.PatientName;
                myPatient.PatientAge = dataget.Age;
                myPatient.Email = dataget.Login.Email;
                myPatient.Password = dataget.Login.Password;
                myPatient.PatientGender = dataget.Gender;
                myPatient.ConfirmPassword = dataget.Login.Password;
                return View(myPatient);
            }
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            int membID = Convert.ToInt32(Session["MemberId"]);
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                var dataget = db.Patients.FirstOrDefault(a => a.MemberId == membID);

                PatientModel myPatient = new PatientModel();
                myPatient.MemberId = dataget.MemberId;
                myPatient.RoleId = dataget.Login.RoleId;
                myPatient.Id = dataget.PatientId;
                myPatient.PatientName = dataget.PatientName;
                myPatient.PatientAge = dataget.Age;
                myPatient.Email = dataget.Login.Email;
                myPatient.Password = dataget.Login.Password;
                myPatient.PatientGender = dataget.Gender;
                myPatient.ConfirmPassword = dataget.Login.Password;
                return View(myPatient);
            }
        }

        [HttpPost]
        public ActionResult EditProfile(PatientModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {

                int membID = Convert.ToInt32(Session["MemberId"]);
                var getdata = db.Patients.FirstOrDefault(a => a.MemberId == membID);
                getdata.PatientName = model.PatientName;
                getdata.Age = model.PatientAge;
                getdata.Gender = model.PatientGender;
                getdata.Login.Email = model.Email;
                getdata.Login.Password = model.Password;
                getdata.Login.RoleId = 4;
                db.SaveChanges();

                return RedirectToAction("ProfileUpdateSuccess", "Patient");
            }

        }

        public ActionResult ProfileUpdateSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ScheduleAppointment()
        {
            using(ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                var getdata = db.Patients.FirstOrDefault(a => a.MemberId == membID);

                var appointmentData = db.Appointments.FirstOrDefault(a => a.PatientId == getdata.PatientId);
                if(appointmentData == null)
                {

                    PatientAppointmentModel APPModel = new PatientAppointmentModel();
                    APPModel.SlotTime = GenericCode.MethodToGetSlotTime.GetSlotTime();
                    APPModel.DoctorNames = GenericCode.MethodToGetDoctorNameAndSpecialization.GetDocNamesAndSpec();
                    return View(APPModel);
                }
            }

            return RedirectToAction("AlreadyBookedAppointment","Patient");
        }

        public ActionResult AlreadyBookedAppointment()
        {
            ViewBag.res = "You Have a Booked Appointment";
            return View();
        }


        [HttpPost]
        public ActionResult ScheduleAppointment(PatientAppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SlotTime = GenericCode.MethodToGetSlotTime.GetSlotTime();
                model.DoctorNames = GenericCode.MethodToGetDoctorNameAndSpecialization.GetDocNamesAndSpec();
                return View(model);
            }

            using(ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                var getdata = db.Patients.FirstOrDefault(a => a.MemberId == membID);

                Appointment obj = new Appointment();
                //obj.AppoinmentId = model.
                obj.AppointmentSubject = model.AppointmentSubject;
                obj.AppointmentDescription = model.AppointmentDescription;
                obj.AppointmentDate = Convert.ToDateTime( model.AppointmentDate);
                obj.SlotId = model.SlotID;
                obj.AppointmentStatus = "Requested";
                obj.DoctorId = model.DoctorId;
                obj.PatientId = getdata.PatientId;

                db.Appointments.Add(obj);
                db.SaveChanges();

                return RedirectToAction("AppointmentSuccess","Patient");
            }
        }

        public ActionResult AppointmentSuccess()
        {
            return View();
        }

        public ActionResult CheckAppointment()
        {
            using(ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                int membID = Convert.ToInt32(Session["MemberId"]);
                var getPatientData = db.Patients.FirstOrDefault(a => a.MemberId == membID);
                var getAppointmentData = db.Appointments.FirstOrDefault(a => a.PatientId == getPatientData.PatientId);
                
                if (getAppointmentData == null)
                {
                    ViewBag.res = new string[] { "No Booked Appointment!" };
                    return View();
                }
                else
                {
                    if (getAppointmentData.AppointmentStatus.Equals("Accepted"))
                    {

                        ViewBag.res = new string[] { "Your Appointment is SCHEDULED!",
                                                    "",
                                                    "With Doctor:   " + "   " + getAppointmentData.Doctor.DoctorName,
                                                    "On:            " + "   " +getAppointmentData.AppointmentDate ,
                                                    "At:            " + "   " +getAppointmentData.Slot.SlotTime
                                                };

                        return View();
                    }
                    else if (getAppointmentData.AppointmentStatus.Equals("Rejected"))
                    {

                        ViewBag.res = new string[] { "Your Appointment is REJECTED!",
                                                    "",
                                                    "With Doctor:   " + "   " + getAppointmentData.Doctor.DoctorName,
                                                    "On:            " + "   " +getAppointmentData.AppointmentDate ,
                                                    "At:            " + "   " +getAppointmentData.Slot.SlotTime, 
                                                    "",
                                                    "",
                                                    "BOOK ANOTHER APPOINTMENT!"
                                                };

                        db.Appointments.Remove(getAppointmentData);
                        db.SaveChanges();
                        return View();
                    }
                    else if (getAppointmentData.AppointmentStatus.Equals("Requested"))
                    {

                        ViewBag.res = new string[] { "Your Appointment is REQUESTED!",
                                                    "",
                                                    "With Doctor:   " + "   " + getAppointmentData.Doctor.DoctorName,
                                                    "On:            " + "   " +getAppointmentData.AppointmentDate ,
                                                    "At:            " + "   " +getAppointmentData.Slot.SlotTime
                                                };

                        return View();
                    }

                    return View();
                }

            }
        }


        [HttpGet]
        public ActionResult CreateMessage()
        {
            using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
            {
                MessageSenderPatientToDoctorModel createMessagePatientDoctorModel = new MessageSenderPatientToDoctorModel();
                var getdata = GenericCode.MethodtoGetDoctorNameListWithMemberId.GetDoctorNameListWithMemberId();
                createMessagePatientDoctorModel.DoctorNameList = getdata;
                return View(createMessagePatientDoctorModel);
            }
        }
        [HttpPost]
        public ActionResult CreateMessage(MessageSenderPatientToDoctorModel Model)
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
                Message obj = new Message();
                obj.SenderId = membID;
                obj.ReceiverId = Model.ReceiverDoctorMemberID;
                obj.MessageSubject = Model.MessageSubject;
                obj.MessageDescription = Model.MessageDescription;
                obj.MessageDate = DateTime.Now;
                db.Messages.Add(obj);
                db.SaveChanges();
                var getDoctor = db.Doctors.FirstOrDefault(x => x.MemberId == Model.ReceiverDoctorMemberID);
                ViewBag.MessageSend = getDoctor.DoctorName;
                return View("MessageSent");
            }
            //return View();
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
                List<MessageDoctorSenderToPatientModel> viewMessagesFromDoctor = new List<MessageDoctorSenderToPatientModel>();
                var getMessageReceived = from m in db.Messages
                                         join p in db.Patients on m.ReceiverId equals p.MemberId
                                         join d in db.Doctors on m.SenderId equals d.MemberId
                                         orderby m.MessageDate descending
                                         where (p.MemberId == MemberId)
                                         select new
                                         {
                                             p.PatientName,
                                             d.DoctorName,
                                             m.MessageSubject,
                                             m.MessageDescription,
                                             m.MessageDate
                                         };


                foreach (var item in getMessageReceived)
                {
                    viewMessagesFromDoctor.Add(new MessageDoctorSenderToPatientModel
                    {
                        ReceiverName = item.PatientName,
                        SenderName = item.DoctorName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = item.MessageDate
                    });
                }
                var getMessageSent = from m in db.Messages
                                         join p in db.Patients on m.SenderId equals p.MemberId
                                         join d in db.Doctors on m.ReceiverId equals d.MemberId
                                         orderby m.MessageDate descending
                                         where (p.MemberId == MemberId)
                                         select new
                                         {
                                             p.PatientName,
                                             d.DoctorName,
                                             m.MessageSubject,
                                             m.MessageDescription,
                                             m.MessageDate
                                         };


                foreach (var item in getMessageSent)
                {
                    viewMessagesFromDoctor.Add(new MessageDoctorSenderToPatientModel
                    {
                        SenderName = item.PatientName,
                        ReceiverName = item.DoctorName,
                        MessageSubject = item.MessageSubject,
                        MessageDescription = item.MessageDescription,
                        MessageDate = item.MessageDate
                    });
                }
                return View(viewMessagesFromDoctor);
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------

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

    }
}