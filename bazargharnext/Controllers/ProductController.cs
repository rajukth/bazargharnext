using bazargharnext.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bazargharnext.ModelsView;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace bazargharnext.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        readonly DataContext dal = new DataContext();
        User users;
        public ProductController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }

        
        public ViewResult AddProduct(bool isSuccess = false, int Product_Id = 0)
        {

            List<Category> cat = new List<Category>();
            cat = (from c in dal.Category select c).ToList();
            cat.Insert(0, new Category { Category_id = 0, Category_name = "-- Select Category --", Category_description = "Select" });
            ViewBag.Category = cat;

            ViewBag.isSuccess = isSuccess;
            ViewBag.Product_Id = Product_Id;
            return View();
        }
        public IActionResult ViewProduct()
        {

            return View();
        }

       


        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm]MyProductView myProductView ) {
            users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            MyProductView product = new MyProductView();
            if (ModelState.IsValid)
            {
                product.Product_name = myProductView.Product_name;
                product.Price = myProductView.Price;
                product.Description = myProductView.Description;
                product.Category_id = myProductView.Category_id;

                product.Date = DateTime.Now;
                product.Userid = users.Userid;
                var product_Details_json = Request.Form["Product_Details"];
                if (product_Details_json.Count > 0)
                {
                    product.Product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(product_Details_json);
                }
               
                product.Gallery = new List<GalleryModel>();

                var gallery = Request.Form.Files;

                if (gallery != null)
                    {
                    string folder = "images/products/";
                    for (int i = 0; i < gallery.Count; i++)
                    {
                        var file = HttpContext.Request.Form.Files["GalleryFiles"];
                 
                        var galleryModel = new GalleryModel()
                        {
                            Name = file.FileName,
                            URL = await UploadImage(folder, file)
                        };
                        product.Gallery.Add(galleryModel);
                       }
                    }
                int id = await AddProductData(product);
                if (id > 0) {
                    return RedirectToAction(nameof(AddProduct), new { isSuccess = true, Product_Id = id });
                }
               
            }

            return View();
        }
        //Add Product Details to product
        public List<Product_Details> PD(string DetailsString) {
            
                List<Product_Details> product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(DetailsString);
            return product_Details;
        }



        //

        [Route("product-details/{id:int:min(1)}", Name = "productDetailsRoute")]
        public async Task<ViewResult> Update(int id)
        {
            var data = await GetProductById(id);

            return View(data);
        }
        
        [HttpGet]
        [Route("ImageUpdate/{id}")]
        public IActionResult ImageUpdate(int id)
        {
            return View();
        }
        [HttpPost]
        [Route("ImageUpdate/{id}")]
        public async Task<IActionResult> ImageUpdate(int id, MyProductView myProductView)
        {
            bool isSuccess=false;
            string folder = "images/products/";
            if (myProductView.GalleryFiles != null)
            {
                foreach (var file in myProductView.GalleryFiles)
                {
                    GalleryModel galleryModel = new GalleryModel()
                    {
                        Id=id,
                        Name = file.FileName,
                        URL = await UploadImage(folder, file)
                    };


                    dal.Entry(galleryModel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                   dal.SaveChanges();
                    isSuccess = true;
               
                }
            }
            if (isSuccess)
            {
                return RedirectToAction("Index","MyProfile");
            }
            return View();
        }


        //repository
        public async Task<int> AddProductData(MyProductView myProductView) {
            var newProduct = new Product()
            {

                Userid=myProductView.Userid,
                Product_name = myProductView.Product_name,
                Category_id=myProductView.Category_id,
                Price= myProductView.Price ?? 0,
                Description=myProductView.Description,
                Date=myProductView.Date,
            };
            newProduct.GalleryModel = new List<GalleryModel>();
            foreach (var file in myProductView.Gallery) {
                newProduct.GalleryModel.Add(new GalleryModel() { 
                Name=file.Name,
                URL=file.URL
                });
            }
            newProduct.Product_Details = new List<Product_Details>();
            foreach (var product_details in myProductView.Product_Details) {
                newProduct.Product_Details.Add(new Product_Details()
                {
                    Title=product_details.Title,
                    Description=product_details.Description
                });
            }

            await dal.Products.AddAsync(newProduct);
            await dal.SaveChangesAsync();

            return newProduct.Product_Id;
        }

        public async Task<MyProductView> GetProductById(int id) {
            return await dal.Products.Where(x => x.Product_Id == id).Select(product => new MyProductView()
            {
                Product_Id=product.Product_Id,
                Product_name=product.Product_name,
                Category_id=product.Category_id,
                Date=product.Date,
                Description=product.Description,
                Gallery=product.GalleryModel.Select(g=>new GalleryModel(){
                Id=g.Id,
                Name=g.Name,
                URL=g.URL
                }).ToList(),
                Product_Details = product.Product_Details.Select(p => new Product_Details()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description
                }).ToList(),



            }).FirstOrDefaultAsync();

        }
        

        //



        private async Task<string> UploadImage(String folderPath, IFormFile file) { 
        
        folderPath+= Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }
    }
}