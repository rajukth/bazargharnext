using bazargharnext.Models;
using bazargharnext.ModelsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class GetAllProducts
    {
        DataContext _dal;
        public IEnumerable<MyProductView> GetAllProduct()
        {
            _dal = new DataContext();
            if (_dal.Products.Any())
            {
                var myProductView = _dal.Products.Select(product => new MyProductView()
                {
                    Product_Id = product.Product_Id,
                    Product_name = product.Product_name,
                    Price = product.Price,
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
                    }).ToList()

                }).ToList();

                _dal.Dispose();
                return myProductView;
            }
            return new List<MyProductView>();
        }
    }
}
