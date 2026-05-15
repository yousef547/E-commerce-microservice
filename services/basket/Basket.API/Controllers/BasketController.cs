using Basket.Application.Commends;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    
    public class BasketController : BaseApiController
    {
        private readonly IMediator _mediator;
        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCartItemResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartItemResponse>> GetBasket([FromRoute] string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("UpdateBasket")]
        [ProducesResponseType(typeof(ShoppingCartItemResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommend commend)
        {
            var basket = await _mediator.Send(commend);
            return Ok(basket);
        }

        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> DeleteBasket(string userName)
        {
            var basket = await _mediator.Send(new DeleteBasketByUserNameCommend(userName));
            return Ok(basket);
        }


        [HttpGet]
        [Route("[action]/{userName}", Name = "GetName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> GetName(string userName)
        {
            return Ok(userName);
        }
    }
}
