using Catalog.Application.Responces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetProductByName:IRequest<IList<ProductResponseDto>>
    {

        public GetProductByName(string name)
        {
			Name = name;

		}
        public string Name { get; set; }
    }
}
