using Catalog.Application.Responces;
using Catalog.Core.Entities;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class UpdateProductCommend : IRequest<bool>
    {
		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Summart { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
		public string ImageFile { get; set; }
		public decimal Price { get; set; }
		public ProductBrand Brand { get; set; }
		public ProductType Type { get; set; }

	}
}
