using AutoMapper;
using Catalog.Application.Commends;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.Commends
{
    public class DeleteProductCommendHandler : IRequestHandler<DeleteProductCommend, bool>
    {
		private readonly IProductRepository _productRepository;
		public DeleteProductCommendHandler( IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}
		public async Task<bool> Handle(DeleteProductCommend request, CancellationToken cancellationToken)
        {
			return await _productRepository.DeleteProduct(request.Id);
        }
    }
}
