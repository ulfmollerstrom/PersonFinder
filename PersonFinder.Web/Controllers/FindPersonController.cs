using System;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using RatsitPersonFinder;

namespace PersonFinder.Web.Controllers
{
    public class FindPersonController : BaseController
    {
        //http://localhost:XXXXX/FindPerson/?fname=nils&lname=andersson&yyyymmdd=19700501
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public JsonResult Index(string fName, string lName, string yyyymmdd = "")
        {
            try
            {
                var persons = Ratsit.FindPersons(fName, lName, yyyymmdd);
                return Json(new { Antal = persons.Count, Resultat = persons }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new {Fel = exception.Message});
            }
        }
    }
}