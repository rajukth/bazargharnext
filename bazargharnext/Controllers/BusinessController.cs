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
            if (HttpContext.Session.GetString("isLoggedin") != "true")
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                if (user.UserRole == "business")
                {
                    HttpContext.Session.SetString("userAs", "business");
                    var data = getAllProductsByUserId.GetProductByUserId(user.Userid);
                    ViewBag.TotalProduct = data.Count();
                    ViewBag.Product = data;

                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            } 
                   
          
          
        }
        public IActionResult Sales() 
        {
            if (HttpContext.Session.GetString("isLoggedin") != "true")
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                if (user.UserRole == "business")
                {
                   
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }
        public IActionResult Purchase()
        {
            if (HttpContext.Session.GetString("isLoggedin") != "true")
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                if (user.UserRole == "business")
                {
                 
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Profile() {

            if (HttpContext.Session.GetString("isLoggedin") != "true")
            {
                return RedirectToAction("Login");
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
                if (user.UserRole == "business")
                {
                   

                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

    }
}
