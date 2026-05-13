using AutoMapper;
using Discount.Application.Commends;
using Discount.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers.Commends
{
	public class DeleteDiscountCommendHandler : IRequestHandler<DeleteDiscountCommend, bool>
	{
		private readonly IDiscountRepository _discountRepository;
		public DeleteDiscountCommendHandler(IDiscountRepository discountRepository)
		{
			_discountRepository = discountRepository;

		}
		public async Task<bool> Handle(DeleteDiscountCommend request, CancellationToken cancellationToken)
		{
			var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
			return deleted;
		}
	}
}
