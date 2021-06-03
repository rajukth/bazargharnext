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

namespace bazargharnext.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        readonly DataContext dal = new DataContext();
        GetSingleProductById getSingleProductById = new GetSingleProductById();
        User users;
        UploadImageFunction uploadImage;

        public ProductController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
            uploadImage = new UploadImageFunction(webHostEnvironment);
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


        [HttpGet]
        [Route("viewProduct/{id}")]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var products= await getSingleProductById.GetProductById(id);
            ViewBag.Products = products;
            GetProductByCategory getProductByCategory = new GetProductByCategory();
            var related = getProductByCategory.GetProductByCategory_id(products.Category_id);
           
            
            ViewBag.Related = related;
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
                            URL = await uploadImage.UploadImage(folder, file)
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
            var data = await getSingleProductById.GetProductById(id);

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
                        URL = await uploadImage.UploadImage(folder, file)
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