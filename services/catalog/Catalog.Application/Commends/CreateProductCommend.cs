using Catalog.Application.Responces;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;


namespace Catalog.Application.Queries
{
    public class CreateProductCommend:IRequest<ProductResponseDto>
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public string Summart { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
		public string ImageFile { get; set; }
		public decimal Price { get; set; }

	}
}
