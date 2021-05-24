using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TattooProject.Models;

namespace TattooProject.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            using (TattooProjectEntities dbModel = new TattooProjectEntities())
            {
                var artists = (from a in dbModel.Artists
                               select a).ToList();
                
                return View("Index", artists);
            }
        }
    }
}