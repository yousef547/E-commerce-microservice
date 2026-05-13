using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.Infrastructure.Repositories
{
	public class ProductRepository : IBrandRepository, IProductRepository, ITypeRepository
	{
		public ICatalogContext _context { get; set; }
		public ProductRepository(ICatalogContext context)
		{
			_context = context;
		}
		public async Task<Product> GetProductById(string id)
		{
			return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
		}
		public async Task<Pagination<Product>> GetAllProduct(CatalogSpecsParams catalogSpecsParams)
		{
			var builder = Builders<Product>.Filter;
			var filter = builder.Empty;
			if (!string.IsNullOrEmpty(catalogSpecsParams.Search)) {
				filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecsParams.Search.ToLower()));
			}
			if (!string.IsNullOrEmpty(catalogSpecsParams.BrandId))
			{
				var builderFilter =  builder.Eq(p => p.Brand.Id ,catalogSpecsParams.BrandId);
				filter &= builderFilter;
			}


			if (!string.IsNullOrEmpty(catalogSpecsParams.TypeId))
			{
				var builderFilter = builder.Eq(p => p.Type.Id, catalogSpecsParams.TypeId);
				filter &= builderFilter;
			}

			var totalItems = await _context.Products.CountDocumentsAsync(filter);
			var data = await DataFilter(catalogSpecsParams, filter);
			return new Pagination<Product>(
				catalogSpecsParams.PageIndex,
				catalogSpecsParams.PageSize,
				(int)totalItems,data
				);

		}

		public async Task<IEnumerable<Product>> GetAllProductsByBrand(string name)
		{
			return await _context.Products.Find(x => x.Brand.Name == name).ToListAsync();

		}

		public async Task<IEnumerable<Product>> GetAllProductsByName(string name)
		{
			return await _context.Products.Find(x => x.Name == name).ToListAsync();

		}

		public async Task<IEnumerable<ProductType>> GetAllTypes()
		{
		
			return await _context.Types.Find(x => true).ToListAsync();

		}

		public async Task<Product> CreateProduct(Product product)
		{
			await _context.Products.InsertOneAsync(product);
			return product;
		}

		public async Task<bool> DeleteProduct(string id)
		{
			var deletedProduct = await _context.Products.DeleteOneAsync(x => x.Id == id);
			return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
		}

		public async Task<IEnumerable<ProductBrand>> GetAllBrands()
		{
			return await _context.Brands.Find(x => true).ToListAsync();

		}

		public async Task<bool> UpdateProduct(Product product)
		{
			var updateProduct = await _context.Products.ReplaceOneAsync(x => x.Id == product.Id,product);
			return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
		}

		private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecsParams catalogSpecsParams,FilterDefinition<Product> filter)
		{
			var sortDef = Builders<Product>.Sort.Ascending("Name");
			if (!string.IsNullOrEmpty(catalogSpecsParams.Sort)) {
				switch (catalogSpecsParams.Sort)
				{
					case "priceAsc":
						sortDef = Builders<Product>.Sort.Ascending(p=>p.Price);
						break;
					case "priceDesc":
						sortDef = Builders<Product>.Sort.Descending(p => p.Price);
						break;
					default:
						sortDef = Builders<Product>.Sort.Ascending("Name");
						break;				
				}
			}
			return await _context.Products.Find(filter).Sort(sortDef)
				.Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex-1))
				 .Limit(catalogSpecsParams.PageSize).ToListAsync();
		}
	}
}
