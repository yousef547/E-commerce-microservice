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

namespace Catalog.Application.Handlers.Commends
{
    public class CreateProductCommendHandler : IRequestHandler<CreateProductCommend, ProductResponseDto>
    {

		private readonly IMapper _mapper;
		private readonly IProductRepository _productRepository;
		public CreateProductCommendHandler(IMapper mapper, IProductRepository productRepository)
		{
			_mapper = mapper;
			_productRepository = productRepository;
		}
		public async Task<ProductResponseDto> Handle(CreateProductCommend request, CancellationToken cancellationToken)
        {
			var product = _mapper.Map<Product>(request);
			var result=  await _productRepository.CreateProduct(product);
			return _mapper.Map<ProductResponseDto>(result);
        }
    }
}
