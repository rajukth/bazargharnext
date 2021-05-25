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

namespace bazargharnext.Controllers
{
    public class AddProductController : Controller
    {
        DataContext _dal = new DataContext();
        GetSingleProductById GBI = new GetSingleProductById();
        //
        // GET: /addition/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool Add([FromForm] MyProductView mpv   /*string Product,string customers,string Gallery*/)
        {

            var product_Details_json = Request.Form["Product_Details"];
            if (product_Details_json.Count > 0)
            {
                mpv.Product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(product_Details_json);
            }

            var gallery = Request.Form.Files;

            if (gallery != null)
            {
                for (int i = 0; i < gallery.Count; i++)
                {
                    var logo = HttpContext.Request.Form.Files["file"];
                    mpv.GalleryFiles.Append(logo);
                }
            }
           

            return true;
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<ViewResult> Update(int Id)
        {
            var myProductView =await GBI.GetProductById(Id);
            ViewBag.Data = myProductView;
            return View();
        }
        [HttpPost]
        [Route("update/{id}")]
        public ViewResult Update(int id,[FromForm] MyProductView myProductView)
        {
            var product_Details_json = Request.Form["Product_Details"];
            if (product_Details_json.Count > 0)
            {
               myProductView.Product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(product_Details_json);
            }

            ViewBag.Data = myProductView;

            return View();
        }



        /*public async Task<MyProductView> GetProductById(int id)
        {
            return await _dal.Products.Where(x => x.Product_Id == id).Select(product => new MyProductView()
            {
                Product_Id = product.Product_Id,
                Product_name = product.Product_name,
                Price=product.Price,
                Category_id = product.Category_id,
                Category_name = _dal.Category.Where(x => x.Category_id == product.Category_id).First().Category_name,
                Date = product.Date,
                Description = product.Description,
                Gallery = product.GalleryModel.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList(),
                Product_Details = product.Product_Details.Select(p => new Product_Details()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description
                }).ToList(),



            }).FirstOrDefaultAsync();

        }*/
    }
}
