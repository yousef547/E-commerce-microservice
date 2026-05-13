using Catalog.Application.Responces;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllProductQuery:IRequest<Pagination<ProductResponseDto>>
    {
        public CatalogSpecsParams Spec { get; set; }
        public GetAllProductQuery(CatalogSpecsParams spec)
        {
            Spec = spec;  
        }
    }
}
