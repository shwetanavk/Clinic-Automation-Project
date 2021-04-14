using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicAutomationProject.Models
{
    public class CustomAuthorizeForRoles : AuthorizeAttribute //from using System.Web.Mvc;
    {
        public override void OnAuthorization(AuthorizationContext filterContext)  
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);  ///Authorized user loged in
            }
            else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Login/LoginPage"); 
            }
            else
            {
                filterContext.Result = new RedirectResult("~/MyCustomError/UnAuthorized");//data in db, but not authori
            }
        }
    }
}