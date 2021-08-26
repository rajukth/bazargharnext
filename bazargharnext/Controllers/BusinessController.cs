using bazargharnext.AllFunction;
using bazargharnext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Controllers
{
    public class BusinessController : Controller
    {
        private DataContext _dal = new DataContext();
        GetAllProductsByUserId getAllProductsByUserId = new GetAllProductsByUserId();
        User user;
        public IActionResult Index()
        {
            HttpContext.Session.SetString("userAs", "business");
            if (HttpContext.Session.GetString("User") != null)
            {
                user = JsonConvert.DeserializeObject<User>(value: HttpContext.Session.GetString("User"));

                var data = getAllProductsByUserId.GetProductByUserId(user.Userid);
                ViewBag.TotalProduct = data.Count();
                ViewBag.Product = data;

                return View();
            }
            else {
                return RedirectToAction("Login");
            }
        }
        public IActionResult Sales() 
        {
            return View();
        }
        public IActionResult Purchase()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
