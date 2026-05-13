using AutoMapper;
using Catalog.Application.Responces;
using Catalog.Core.Entities;
using Catalog.Core.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mappers
{
    public class ProductMappingProfile:Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product,ProductResponseDto>().ReverseMap();
            CreateMap<ProductBrand,BrandResponseDto>().ReverseMap();
            CreateMap<ProductType,TypeResponseDto>().ReverseMap();
            CreateMap<Pagination<Product>,Pagination<ProductResponseDto>>().ReverseMap();
        }
    }
}
