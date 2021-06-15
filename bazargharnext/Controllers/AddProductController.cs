using bazargharnext.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using bazargharnext.ModelsView;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bazargharnext.AllFunction;
using Microsoft.AspNetCore.Hosting;

namespace bazargharnext.Controllers
{
    public class AddProductController : Controller
    {
        DataContext _dal = new DataContext();
        GetProductByCategory GPC = new GetProductByCategory();
        GetAllProducts gap = new GetAllProducts();
        //
        // GET: /addition/
        public ActionResult Index()
        {
            ViewBag.Categories = _dal.Category.ToList();
            ViewBag.products = gap.GetAllProduct();
            
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetByCategory(int id) {

            var product =await GPC.GetProductByCategory_id(id);

            return Json(product); ;
        } 

    }
}
