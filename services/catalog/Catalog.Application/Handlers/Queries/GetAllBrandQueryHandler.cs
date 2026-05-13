using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responces;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        public GetAllBrandQueryHandler(IMapper mapper, IBrandRepository brandRepository)
        {
			_mapper = mapper;
            _brandRepository = brandRepository;
		}

        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllBrands();
			return _mapper.Map< IList<ProductBrand> ,IList <BrandResponseDto>>(brands.ToList());
        }
    }
}
