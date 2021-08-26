using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
