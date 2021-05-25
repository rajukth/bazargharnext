using bazargharnext.Models;
using bazargharnext.ModelsView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.AllFunction
{
    public class GetSingleProductById
    {
        DataContext _dal = new DataContext();

        public async Task<MyProductView> GetProductById(int id)
        {
            return await _dal.Products.Where(x => x.Product_Id == id).Select(product => new MyProductView()
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
                }).ToList(),



            }).FirstOrDefaultAsync();

        }

    }
}
