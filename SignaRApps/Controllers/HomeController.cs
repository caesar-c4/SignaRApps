using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignaRApps.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chatbox()
        {
            return View();
        }
        public ActionResult Ngchat()
        {
            return View();
        }
    }
}