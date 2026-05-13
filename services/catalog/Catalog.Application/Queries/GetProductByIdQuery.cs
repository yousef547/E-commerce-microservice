using Catalog.Application.Responces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetProductByIdQuery:IRequest<ProductResponseDto>
    {
        public GetProductByIdQuery(string id )
        {
			Id = id;
        }
        public string Id { get; set; }
    }
}
