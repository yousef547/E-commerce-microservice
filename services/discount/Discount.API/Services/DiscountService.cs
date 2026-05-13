using Discount.Application.Commends;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
        private IMediator _mediator;
        public DiscountService(IMediator mediator)
        {
			_mediator = mediator;
		}

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery (request.ProductName);
            var result = await _mediator.Send(query);
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new CreateDiscountCommend
            {
                ProductName = request.Coupon.ProductName,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
            };
			var result = await _mediator.Send(cmd);
			return result;
		}

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
			var cmd = new UpdateDiscountCommend
			{
				Id = request.Coupon.Id,
				ProductName = request.Coupon.ProductName,
				Amount = request.Coupon.Amount,
				Description = request.Coupon.Description,
			};
			var result = await _mediator.Send(cmd);
			return result;
		}

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
			var result = await _mediator.Send(new DeleteDiscountCommend(request.ProductName));
            return new DeleteDiscountResponse
            {
                Success = result
            };
		}
    }
}
