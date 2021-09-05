using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bazargharnext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace bazargharnext.Controllers
{
    [Route("category")]
    public class CategoryController : Controller
    {
        /*
        // GET
        public IActionResult Index()
        {
            return View();
        }
        */
        private readonly DataContext dal = new DataContext();
        
        public IActionResult Index()
        {
            var userid= JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User")).Userid;
            var data = dal.Category.Where(x=>x.userid==userid).ToList();
            ViewBag.Category = data;
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category ) {
            var userid = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User")).Userid;
            category.userid = userid;

            dal.Category.Add(category);
            dal.SaveChanges();
            return RedirectToAction("Index");
                }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var data = dal.Category.Find(id);
            dal.Category.Remove(data);
            dal.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("update/{id}")]

        public IActionResult Update(int id)
        {
            var category = dal.Category.Find(id);
            ViewBag.Category = category;
            return View("Update");

        }
        [HttpPost]
        [Route("update/{id}")]
        public IActionResult Update(int id, Category category)
        {
            dal.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dal.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}