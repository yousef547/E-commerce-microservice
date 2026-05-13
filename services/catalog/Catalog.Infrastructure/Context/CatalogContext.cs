using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Context
{
	public class CatalogContext : ICatalogContext
	{
		public IMongoCollection<Product> Products { get; }

		public IMongoCollection<ProductType> Types { get; }

		public IMongoCollection<ProductBrand> Brands { get; }

		public CatalogContext(IConfiguration configuration)
		{
			var client = new MongoClient(configuration["DatabaseSetting:ConnectionString"]);
			var database = client.GetDatabase(configuration["DatabaseSetting:DatabaseName"]);

			Brands = database.GetCollection<ProductBrand>(configuration["DatabaseSetting:BrandsCollection"]);
			Types = database.GetCollection<ProductType>(configuration["DatabaseSetting:TypesCollection"]);
			Products = database.GetCollection<Product>(configuration["DatabaseSetting:ProductsCollection"]);


			_ = BrandContextSeed.SeedDataAsync(Brands);
			_ = TypeContextSeed.SeedDataAsync(Types);
			_ = CatalogContextSeed.SeedDataAsync(Products);
		}
	}
}
