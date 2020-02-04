using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMProject.Web.Controllers
{
    public class ServerApiController : Controller
    {
        //
        // GET: /ServerApi/
        
        public JsonResult Product()
        {
            var pro = new { };

            return Json(pro);

        }

    }
}
