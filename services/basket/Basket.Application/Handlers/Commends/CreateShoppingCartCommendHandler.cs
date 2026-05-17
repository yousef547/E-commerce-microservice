using AutoMapper;
using Basket.Application.Commends;
using Basket.Application.GrpcServices;
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
        private readonly DiscountGrpcService _discountGrpcService;

        public CreateShoppingCartCommendHandler(IMapper mapper, IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
            _discountGrpcService = discountGrpcService;
        }

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommend request, CancellationToken cancellationToken)
        {

            foreach (var item in request.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                if (coupon is not null)
                {
                    item.Price -= coupon.Amount;
                }
            }
            var shoppingCart = await _basketRepository.UpdateBasket(new Core.Entities.ShoppingCart()
			{
				UserName = request.UserName,
				Items = request.Items,
			});
			return _mapper.Map<ShoppingCartResponse>(shoppingCart);
		}
    }
}
