using bazargharnext.Models;
using bazargharnext.ModelsView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class GetAllProducts
    {
        DataContext _dal;
        public async Task<List<Product>> GetAllProduct()
        {
            _dal = new DataContext();
            if (_dal.Products.Any())
            {
                var products =await _dal.Products.Select(product => new Product()
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

                _dal.Dispose();
                return products;

            }
            var pro = new List<Product>();
            return pro;
        }
    }
}
