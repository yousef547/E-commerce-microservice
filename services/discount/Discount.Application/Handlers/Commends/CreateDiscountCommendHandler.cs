using AutoMapper;
using Discount.Application.Commends;
using Discount.Application.Handlers.Queries;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers.Commends
{
    public class CreateDiscountCommendHandler : IRequestHandler<CreateDiscountCommend, CouponModel>
    {

		private readonly IDiscountRepository _discountRepository;
		private readonly IMapper _mapper;
		public CreateDiscountCommendHandler(IDiscountRepository discountRepository, IMapper mapper)
		{
			_discountRepository = discountRepository;
			_mapper = mapper;

		}
		public async Task<CouponModel> Handle(CreateDiscountCommend request, CancellationToken cancellationToken)
        {
			var coupon = _mapper.Map<Coupon>(request);
			await _discountRepository.CreateDiscount(coupon);
			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}
    }
}
