using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsynchronousMVC.Controllers
{
    public class NetCoreAPIController : Controller
    {
        // GET: NetCoreAPI
        public ActionResult Index()
        {
            return View("Asynchronous");
        }
    }
}