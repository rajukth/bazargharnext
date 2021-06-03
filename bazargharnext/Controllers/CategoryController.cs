using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bazargharnext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var data = dal.Category.ToList();
            ViewBag.Category = data;
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category ) {
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
            return View("Update", dal.Category.Find(id));

        }
        [HttpPost]
        [Route("update/{id}")]
        public IActionResult Update(int id, Category category)
        {
            dal.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dal.SaveChanges();

            return RedirectToAction("Index");
        }


      /*  public async Task<List<Category>> GetCategory()
        {
            return await dal.Category.Select(x => new Category()
            {
                Category_id = x.Category_id,
                Category_name=x.Category_name,
                Category_description=x.Category_description
            }).ToListAsync();
        }

        public string GetCategoryNameById(int id) {

            string name = dal.Category.Where(c => c.Category_id == id).First().Category_name;
            return name;
        }*/
    }
}