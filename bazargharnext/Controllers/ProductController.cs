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
using bazargharnext.AllFunction;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace bazargharnext.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        DataContext dal;
        GetSingleProductById GetSingleProductById = new GetSingleProductById();
        GetProductByCategory GetProductByCategory = new GetProductByCategory();
        GetAllProducts GetAllProducts = new GetAllProducts();
        User Users;
        UploadImageFunction UploadImage;

        public ProductController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
            UploadImage = new UploadImageFunction(webHostEnvironment);
        }

        
        public ViewResult AddProduct(bool isSuccess = false, int Product_Id = 0)
        {
            dal = new DataContext();
            List<Category> cat = new List<Category>();
            cat = (from c in dal.Category select c).ToList();
            cat.Insert(0, new Category { Category_id = 0, Category_name = "-- Select Category --", Category_description = "Select" });
            ViewBag.Category = cat;
            ViewBag.isSuccess = isSuccess;
            ViewBag.Product_Id = Product_Id;
            dal.Dispose();
            return View();
        }

        [HttpGet]
        [Route("viewProduct/{id}")]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var products= await GetSingleProductById.GetProductById(id);
            ViewBag.Products = products;
           
            
            var related = await GetProductByCategory.GetProductByCategory_id(products.Category_id);
           ViewBag.Related = related;
            return View();
        }

       


        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm]MyProductView myProductView ) {
            Users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            MyProductView product = new MyProductView();
            if (ModelState.IsValid)
            {
                product.Product_name = myProductView.Product_name;
                product.Price = myProductView.Price;
                product.Description = myProductView.Description;
                product.Category_id = myProductView.Category_id;

                product.Date = DateTime.Now;
                product.Userid = Users.Userid;
                var product_Details_json = Request.Form["Product_Details"];
                if (product_Details_json.Count > 0)
                {
                    product.Product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(product_Details_json);
                }

                if (Request.Form.Files.Count > 0)
                {
                    product.Gallery = new List<GalleryModel>();

                    var gallery = Request.Form.Files;

                    if (gallery != null)
                    {
                        string folder = "images/products/";
                        for (int i = 0; i < gallery.Count; i++)
                        {
                            var file = gallery[i];//HttpContext.Request.Form.Files["GalleryFiles"];

                            var galleryModel = new GalleryModel()
                            {
                                Name = file.FileName,
                                URL = await UploadImage.UploadImage(folder, file)
                            };
                            product.Gallery.Add(galleryModel);
                        }
                    }
                }
                 int id = await AddProductData(product);
                if (id > 0) {
                    return RedirectToAction(nameof(AddProduct), new { isSuccess = true, Product_Id = id });
                }
               
            }

            return View();
        }
     

        /*[Route("product-details/{id:int:min(1)}", Name = "productDetailsRoute")]
        public async Task<ViewResult> Update(int id)
        {
            var data = await getSingleProductById.GetProductById(id);

            return View(data);
        }*/
        
        [HttpGet]
        [Route("ImageUpdate/{id}")]
        public IActionResult ImageUpdate(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        //[Route("ImageUpdate/{id}")]
        public async Task<bool> ImageUpdate(MyProductView myProductView)
        {
             dal = new DataContext();
            var id = int.Parse(Request.Form["id"]);
            GalleryModel img = dal.Gallery.Single(a => a.Id == id);
            var oldURL = img.URL;
            bool isSuccess=false;
            string folder = "images/products/";
            if (myProductView.GalleryFiles != null)
            {

                img.Id = id;
                img.Name = myProductView.GalleryFiles[0].FileName;
                img.URL = await UploadImage.UploadImage(folder, myProductView.GalleryFiles[0]);


                //dal.Entry(galleryModel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
              int returnId= await dal.SaveChangesAsync();
                dal.Dispose();
                //removing from folder
                var oldFile = Path.Combine(_webHostEnvironment.WebRootPath, oldURL.Remove(0, 1));
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                isSuccess = true;
               
                }
            
            if (isSuccess)
            {
                return isSuccess;
            }
            return false;
        }
        [HttpGet]
        public async Task<bool> Delete_Product_Details(int id)
        {
            dal = new DataContext();
            int ret = 0;

            dal.Remove(dal.Product_Details.Single(a => a.Id == id));
            ret = await dal.SaveChangesAsync();
            if (ret != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        [HttpGet]
        public async Task<bool> DeleteProduct(int id)
        {
            dal = new DataContext();
            int ret = 0;
            MyProductView product = await GetSingleProductById.GetProductById(id);
            try {
                if (product.Gallery != null)
                {

                    foreach (var img in product.Gallery) {
                        //removing from folder
                        var oldFile = Path.Combine(_webHostEnvironment.WebRootPath, img.URL.Remove(0, 1)); ;
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }

                    dal.Remove(dal.Products.Single(a => a.Product_Id == id));



                    ret = await dal.SaveChangesAsync();

                }
               
            }
            catch (DbUpdateConcurrencyException ex) {
                ex.Entries.Single().Reload();
            }
            if (ret != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<bool> Delete_Image(int id)
        {
            dal = new DataContext();
            int ret = 0;
            var img = dal.Gallery.Single(a => a.Id == id);

            dal.Remove(img);
            //removing from folder
            var oldFile = Path.Combine(_webHostEnvironment.WebRootPath, img.URL.Remove(0, 1));
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            ret = await dal.SaveChangesAsync();
            if (ret != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //search by category

      
        [HttpGet]
        public async Task<JsonResult> searchProduct(int id,string search,int min,int max)
        {
            dal = new DataContext();
            List<Product> product=new List<Product>();
            if (id == 0 && search == null && min == 0 && max == 0) {
                product = await GetAllProducts.GetAllProduct();
            }

            else if (id != 0 && search == null && max == 0) {
                product = await GetProductByCategory.GetProductByCategory_id(id);
            }
            else if (id == 0 && search != null && max == 0)
            {

                product = await dal.Products.Where(x => x.Product_name.Contains(search)).Select(product => new Product()
                {
                    Product_Id = product.Product_Id,
                    Product_name = product.Product_name,
                    Price = product.Price,
                    Category_id = product.Category_id,

                    GalleryModel = product.GalleryModel.Select(g => new GalleryModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList(),



                }).ToListAsync();
            }
            else if (id == 0 && search != null && max != 0)
            {

                product = await dal.Products.Where(x => x.Product_name.Contains(search) && x.Price >= min && x.Price <= max).Select(product => new Product()
                {
                    Product_Id = product.Product_Id,
                    Product_name = product.Product_name,
                    Price = product.Price,
                    Category_id = product.Category_id,

                    GalleryModel = product.GalleryModel.Select(g => new GalleryModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList(),



                }).ToListAsync();
            }
            else if (id != 0 && search != null && max != 0)
            {
                product = await dal.Products.Where(products => products.Category_id == id && products.Product_name.Contains(search) && products.Price >= min && products.Price <= max).Select(product => new Product()
                {
                    Product_Id = product.Product_Id,
                    Product_name = product.Product_name,
                    Price = product.Price,
                    Category_id = product.Category_id,

                    GalleryModel = product.GalleryModel.Select(g => new GalleryModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList(),
                }).ToListAsync();
            }
            else if (id == 0 && search == null &&  max != 0)
            {
                product = await dal.Products.Where(products=> products.Price >= min&& products.Price <= max).Select(product => new Product()
                {
                    Product_Id = product.Product_Id,
                    Product_name = product.Product_name,
                    Price = product.Price,
                    Category_id = product.Category_id,

                    GalleryModel = product.GalleryModel.Select(g => new GalleryModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList(),
                }).ToListAsync();
            }

            dal.Dispose();

            return Json(product);
        }
        //Search by caregory
        /*update*/
        [HttpGet]
        [Route("update/{id}")]
        public async Task<ViewResult> Update_Product(int Id)
        {
            var myProductView = await GetSingleProductById.GetProductById(Id);

            ViewBag.Data = myProductView;
            return View();
        }
        [HttpPost]
        /*   [Route("AddProduct/Update/{id}")]*/
        public async Task<int> Update([FromForm] MyProductView myProductView)
        {
            dal = new DataContext();
            var product_Details_json = Request.Form["Product_Details"];
            if (product_Details_json.Count > 0)
            {
                myProductView.Product_Details = JsonConvert.DeserializeObject<List<Product_Details>>(product_Details_json);
            }
            if (Request.Form.Files.Count > 0)
            {
                myProductView.Gallery = new List<GalleryModel>();

                var gallery = Request.Form.Files;

                if (gallery != null)
                {
                    string folder = "images/products/";
                    for (int i = 0; i < gallery.Count; i++)
                    {
                        var file = gallery[i];//HttpContext.Request.Form.Files["GalleryFiles"];

                        var galleryModel = new GalleryModel()
                        {
                            Name = file.FileName,
                            URL = await UploadImage.UploadImage(folder, file)
                        };
                        myProductView.Gallery.Add(galleryModel);
                    }
                }
            }
                /**/
                int returnId = 0;
            // users = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            Product product = dal.Products.First(c => c.Product_Id == myProductView.Product_Id);


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
                    product.GalleryModel = myProductView.Gallery;
                 


                    returnId = await dal.SaveChangesAsync();
                   
                    }
                catch (DbUpdateConcurrencyException ex)
                {
                    SaveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }
            } while (SaveFailed);
            //


            dal.Dispose();

            /**/
            return returnId;
        }
        /*update*/
        //repository
        public async Task<int> AddProductData(MyProductView myProductView) {
            dal = new DataContext();
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
            if (myProductView.Gallery!=null && myProductView.Gallery.Count > 0)
            {
                foreach (var file in myProductView.Gallery)
                {
                    newProduct.GalleryModel.Add(new GalleryModel()
                    {
                        Name = file.Name,
                        URL = file.URL
                    });
                }
            }

            newProduct.Product_Details = new List<Product_Details>();
            if (myProductView.Product_Details!=null && myProductView.Product_Details.Count > 0)
            {
                foreach (var product_details in myProductView.Product_Details)
                {
                    newProduct.Product_Details.Add(new Product_Details()
                    {
                        Title = product_details.Title,
                        Description = product_details.Description
                    });
                }
            }

            await dal.Products.AddAsync(newProduct);
            await dal.SaveChangesAsync();

            return newProduct.Product_Id;
        }
//



       
    }
}