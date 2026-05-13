using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responces;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.Queries
{
    internal class GetProductByNameQueryHandler : IRequestHandler<GetProductByName, ProductResponseDto>
	{
		private readonly IMapper _mapper;
		private readonly IProductRepository _productRepository;
		public GetProductByNameQueryHandler(IMapper mapper, IProductRepository productRepository)
		{
			_mapper = mapper;
			_productRepository = productRepository;
		}
		public async Task<ProductResponseDto> Handle(GetProductByName request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetAllProductsByName(request.Name);
			return _mapper.Map<ProductResponseDto>(product);
		}
	}
}
