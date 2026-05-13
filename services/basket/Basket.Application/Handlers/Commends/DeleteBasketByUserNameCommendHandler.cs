using AutoMapper;
using Basket.Application.Commends;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Commends
{
    public class DeleteBasketByUserNameCommendHandler : IRequestHandler<DeleteBasketByUserNameCommend, Unit>
    {

		private readonly IBasketRepository _basketRepository;
		public DeleteBasketByUserNameCommendHandler(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;

		}

		public async Task<Unit> Handle(DeleteBasketByUserNameCommend request, CancellationToken cancellationToken)
        {
			await _basketRepository.DeleteBasket(request.UserName);
			return Unit.Value;
		}
    }
}
