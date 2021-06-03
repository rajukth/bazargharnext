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
        DataContext _dal;
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
            _dal = new DataContext();
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
     /*   [Route("AddProduct/Update/{id}")]*/
        public async Task<int> Update([FromForm] MyProductView myProductView)
        {
            _dal = new DataContext();
            var product_Details_json = Request. Form["Product_Details"];
            if (product_Details_json.Count > 0)
            {
               myProductView.Product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(product_Details_json);
            }
            /**/
            int returnId = 0 ;
           // users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            Product product = _dal.Products.First(c => c.Product_Id == myProductView.Product_Id);
           

            //code to update database tables 
            bool SaveFailed;
            do
            {
                SaveFailed = false;
                try
                {
                    product.Product_name = myProductView.Product_name;
                    product.Price = (int)myProductView.Price;
                    product.Description = myProductView.Description;
                    product.Date = product.Date;
                    product.Category_id = myProductView.Category_id;
                    product.Product_Details = myProductView.Product_Details;


                    var origins = (from pd in _dal.Product_Details where pd.Product_Id == product.Product_Id select pd).ToList();
                    _dal.Product_Details.RemoveRange(origins);



                    returnId = await _dal.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    SaveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }
            } while (SaveFailed);
            //


            _dal.Dispose();
           
            /**/
            return returnId;
        }
        [HttpGet]
        public async Task<bool> Delete_Product_Details(int id) {
            _dal = new DataContext();
            int ret = 0;
            
            _dal.Remove(_dal.Product_Details.Single(a => a.Id == id));
             ret=await _dal.SaveChangesAsync();
            if (ret != 0)
            {
                return true;
            }
            else {
                return false;
            }

        }


    }
}
