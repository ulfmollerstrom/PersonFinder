using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using RatsitPersonFinder;

namespace PersonFinder.Web.Controllers
{
    public class FindPersonController : Controller
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public JsonResult Index(string fName, string lName, string yyyymmdd = "")
        {
            return Json(Ratsit.FindPersons(fName, lName, yyyymmdd),JsonRequestBehavior.AllowGet);
        }
    }
}
