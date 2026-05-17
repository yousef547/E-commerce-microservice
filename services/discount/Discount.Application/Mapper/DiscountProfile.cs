using AutoMapper;
using Discount.Application.Commends;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mapper
{
    public class DiscountProfile:Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
            CreateMap<Coupon, CreateDiscountCommend>().ReverseMap();
            CreateMap<Coupon, UpdateDiscountCommend>().ReverseMap();
        }
    }
}
