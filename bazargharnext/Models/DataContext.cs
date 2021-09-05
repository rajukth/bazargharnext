using bazargharnext.ModelsView;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bazargharnext.Models
{
	public class DataContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			var configuration = builder.Build();
			
			optionsBuilder.UseMySql(configuration
				["ConnectionStrings:DefaultConnection"]
				//["ConnectionStrings:bazargharnextContextConnection"]
				
				);

		}

		//Need to register your models here
		public DbSet<User> Users { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<Product> Products { get; set; }

		public DbSet<Product_Details> Product_Details { get; set; }
		
		public DbSet<GalleryModel> Gallery { get; set; }
		public DbSet<Comments> Comments { get; set; }
		public DbSet<BusinessUser> BusinessUsers { get; set; }
		public DbSet<BusinessUserRequest> BusinessUserRequests { get; set; }


	}

}
