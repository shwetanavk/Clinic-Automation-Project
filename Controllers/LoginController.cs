using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicAutomationProject.Models;
using ClinicAutomationProject.GenericCode;
using System.Web.Security;

namespace ClinicAutomationProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult LoginPage()
        {
            if (Request.IsAuthenticated && Session["RoleId"] != null)
            {
                int roleid = Convert.ToInt32(Session["RoleId"]);
                switch (roleid)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        return RedirectToAction("Index", "Doctor");
                    case 3:
                        return RedirectToAction("Index", "Supplier");
                    case 4:
                        return RedirectToAction("Index", "Patient");
                    default:
                        break;
                }
            }
            LoginModel log = new LoginModel();
            log.RoleTypes = MethodToGetRoles.GetRoleId();
            return View(log);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPage(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (ClinicalAutomationSystemEntities db = new ClinicalAutomationSystemEntities())
                {
                    List<LoginModel> modelfromTable = new List<LoginModel>();
                    //var getdata = db.Logins.Include("RoleTable").Select(a => new { a.MemberId, a.Email, a.Password, a.RoleId, a.RoleTable.RoleName}).ToList();

                    var checkdata = db.Logins.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password && a.RoleId.Equals(model.RoleId));
                    if (checkdata != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, false);

                        var authticket = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.AddMinutes(30), false, checkdata.RoleTable.RoleName);
                        string encryptticket = FormsAuthentication.Encrypt(authticket);
                        var authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptticket);
                        HttpContext.Response.Cookies.Add(authcookie);

                        Session["RoleId"] = checkdata.RoleId;
                        Session["MemberId"] = checkdata.MemberId;

                        if (checkdata.RoleId == 1)
                        {
                            return RedirectToAction("AdminLogin", "Admin");
                        }
                        else if (checkdata.RoleId == 2)
                        {
                            return RedirectToAction("DoctorLogin", "Doctor");
                        }
                        else if (checkdata.RoleId == 3)
                        {
                            return RedirectToAction("SupplierLogin", "Supplier");
                        }
                        else if (checkdata.RoleId == 4)
                        {
                            return RedirectToAction("PatientLogin", "Patient");
                        }
                        else
                        {
                            ViewBag.Message = "http post ka error";
                            model.RoleTypes = MethodToGetRoles.GetRoleId();
                            return View(model);
                        }
                    }

                    else
                    {
                        ViewBag.Message = "Invalid Role, EmailId or Password.";
                        model.RoleTypes = MethodToGetRoles.GetRoleId();
                        return View(model);
                    }
                }
            }
            model.RoleTypes = MethodToGetRoles.GetRoleId();
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginPage", "Login");
        }
    }
}