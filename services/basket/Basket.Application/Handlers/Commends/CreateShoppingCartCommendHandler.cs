using AutoMapper;
using Basket.Application.Commends;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Commends
{
    public class CreateShoppingCartCommendHandler:IRequestHandler<CreateShoppingCartCommend, ShoppingCartResponse> 
    {
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;
		public CreateShoppingCartCommendHandler(IMapper mapper, IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;

		}

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommend request, CancellationToken cancellationToken)
        {
			var shoppingCart = await _basketRepository.UpdateBasket(new Core.Entities.ShoppingCart()
			{
				UserName = request.UserName,
				Items = request.Items,
			});
			return _mapper.Map<ShoppingCartResponse>(shoppingCart);
		}
    }
}
