using Catalog.Application.Responces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllTypeQuery:IRequest<IList<TypeResponseDto>>
    {
    }
}
