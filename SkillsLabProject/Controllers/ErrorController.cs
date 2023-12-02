using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.ErrorName = "Not Found";
            ViewBag.ErrorCode = 404;
            return View();
        }
        // GET: Error/Unauthorized
        public ActionResult Unauthorized()
        {
            ViewBag.ErrorName = "Unauthorized";
            ViewBag.ErrorCode = 401;
            return View("Index");
        }
        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            ViewBag.ErrorName = "Not Found";
            ViewBag.ErrorCode = 404;
            return View("Index");
        }
        // GET: Error/InternalServerError
        public ActionResult InternalServerError()
        {
            ViewBag.ErrorName = "Internal Server Error";
            ViewBag.ErrorCode = 500;
            return View("Index");
        }
    }
}