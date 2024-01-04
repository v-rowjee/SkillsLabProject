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
            return View();
        }
        // GET: Error/Unauthorized
        public ActionResult Unauthorized()
        {
            ViewBag.ErrorCode = 401;
            ViewBag.ErrorName = "Unauthorized";
            ViewBag.ErrorDetail = "The request lacks valid authentication credentials for the target resource.";
            return View("Index");
        }
        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            ViewBag.ErrorCode = 404;
            ViewBag.ErrorName = "Page Not Found";
            ViewBag.ErrorDetail = "The page you're looking for doesn't exist.";
            return View("Index");
        }
        // GET: Error/InternalServerError
        public ActionResult InternalServerError()
        {
            ViewBag.ErrorCode = 500;
            ViewBag.ErrorName = "Internal Error";
            ViewBag.ErrorDetail = "The server has encountered an error, please contact system administrator.";
            return View("Index");
        }
        // GET: Error/BadGateway
        public ActionResult BadGateway()
        {
            ViewBag.ErrorCode = 502;
            ViewBag.ErrorName = "Internal Server Error";
            ViewBag.ErrorDetail = "The server was acting as a gateway or proxy and received an invalid response from the upstream server.";
            return View("Index");
        }
    }
}