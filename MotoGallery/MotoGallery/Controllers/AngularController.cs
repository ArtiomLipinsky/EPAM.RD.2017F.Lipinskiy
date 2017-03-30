using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotoGallery.Controllers
{
    public class AngularController : Controller
    {
        // GET: Angular
        public ActionResult Index(string url)
        {
            return View("Index");
        }
    }
}