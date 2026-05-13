using AutoMapper;
using Discount.Application.Commends;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers.Commends
{
    public class UpdateDiscountCommendHandler : IRequestHandler<UpdateDiscountCommend, CouponModel>
    {
		private readonly IDiscountRepository _discountRepository;
		private readonly IMapper _mapper;
		public UpdateDiscountCommendHandler(IDiscountRepository discountRepository, IMapper mapper)
		{
			_discountRepository = discountRepository;
			_mapper = mapper;

		}
		public async Task<CouponModel> Handle(UpdateDiscountCommend request, CancellationToken cancellationToken)
        {
			var coupon = _mapper.Map<Coupon>(request);
			await _discountRepository.UpdateDiscount(coupon);
			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}
    }
}
