using Catalog.Application.Responces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetProductByBrandQuery : IRequest<ProductResponseDto>
	{
        public GetProductByBrandQuery(string brand)
        {
			Brand = brand;
        }
        public string Brand { get; set; }
	}
}
