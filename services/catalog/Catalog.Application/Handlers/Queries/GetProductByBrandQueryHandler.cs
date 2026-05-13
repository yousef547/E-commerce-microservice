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
    internal class GetProductByBrandQueryHandler : IRequestHandler<GetProductByBrandQuery, ProductResponseDto>
	{
		private readonly IMapper _mapper;
		private readonly IProductRepository _productRepository;
		public GetProductByBrandQueryHandler(IMapper mapper, IProductRepository productRepository)
		{
			_mapper = mapper;
			_productRepository = productRepository;
		}
		public async Task<ProductResponseDto> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetAllProductsByBrand(request.Brand);
			return _mapper.Map<ProductResponseDto>(product);
		}
	}
}
