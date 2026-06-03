using Catalog.Application.Commends;
using Catalog.Application.Queries;
using Catalog.Application.Responces;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(
            IMediator mediator,
            ILogger<CatalogController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetProductById(string id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{brandName}", Name = "GetProductByBrandName")]
        [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetProductByBrandName(string brandName)
        {
            var result = await _mediator.Send(new GetProductByBrandQuery(brandName));

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetProductByProductName")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProductByProductName(string productName)
        {
            var result = await _mediator.Send(new GetProductByName(productName));
            _logger.LogInformation($"Product with {productName} is featched");
            return Ok(result);
        }



        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllProducts([FromQuery] CatalogSpecsParams specs)
        {
            var result = await _mediator.Send(new GetAllProductQuery(specs));
            return Ok(result);
        }


        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllBrands()
        {
            var result = await _mediator.Send(new GetAllBrandsQuery());
            return Ok(result);
        }


        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(IList<TypeResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllTypes()
        {
            var result = await _mediator.Send(new GetAllTypeQuery());
            return Ok(result);
        }


        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] CreateProductCommend dto)
        {
            var result = await _mediator.Send<ProductResponseDto>(dto);
            return Ok(result);
        }


        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommend dto)
        {
            var result = await _mediator.Send<bool>(dto);
            return Ok(result);
        }


        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteProduct(string id)
        {
            var result = await _mediator.Send<bool>(new DeleteProductCommend(id));
            return Ok(result);
        }
    }
}
