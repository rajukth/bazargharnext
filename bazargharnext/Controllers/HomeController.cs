using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using bazargharnext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bazargharnext.Controllers
{
    
    [Route("home")]
    public class HomeController : Controller
    {

        //DataContext _dbContext = new DataContext();
        
        [Route("")]
        [Route("index")]
        [Route("~/")]
        
        public IActionResult Index()
        {
            return View();
        }


        [Route("about")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "If you want some support please contact us.";

            return View();
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

      

    }
}