using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responces;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllTypeQueryHandler : IRequestHandler<GetAllTypeQuery, IList<TypeResponseDto>>
    {

		private readonly IMapper _mapper;
		private readonly ITypeRepository _typeRepository;
		public GetAllTypeQueryHandler(IMapper mapper, ITypeRepository typeRepository)
		{
			_mapper = mapper;
			_typeRepository = typeRepository;
		}
		public async Task<IList<TypeResponseDto>> Handle(GetAllTypeQuery request, CancellationToken cancellationToken)
        {
			var types = await _typeRepository.GetAllTypes();
			return _mapper.Map<IList<ProductType>, IList<TypeResponseDto>>(types.ToList());
		}
    }
}
