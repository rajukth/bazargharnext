using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using bazargharnext.AllFunction;
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
        DataContext _dal;
        GetAllProducts getAllProducts=new GetAllProducts();

        [Route("")]
        [Route("index")]
        [Route("~/")]
        
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("userAs", "user");
            _dal = new DataContext();
            ViewBag.Categories = _dal.Category.ToList();
            var Products = await getAllProducts.GetAllProduct();
            ViewBag.Products = Products;
            List<string> ProductName =new List<string>();
            foreach (var Prod in Products) {
                ProductName.Add(Prod.Product_name);
            }
            ViewBag.ProductName = ProductName;
            _dal.Dispose();
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