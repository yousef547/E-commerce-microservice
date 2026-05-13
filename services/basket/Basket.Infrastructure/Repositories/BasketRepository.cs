using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        public BasketRepository(IDistributedCache redisCache)
        {
			_redisCache = redisCache;
		}

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basked = await _redisCache.GetStringAsync(userName);
            if ((string.IsNullOrEmpty(basked)))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basked);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
			var basked = await _redisCache.GetStringAsync(cart.UserName);

            if (basked != null)
            {
                return await GetBasket(cart.UserName);
            }
            else
            {
                await _redisCache.SetStringAsync(cart.UserName, JsonConvert.SerializeObject(cart));
				return await GetBasket(cart.UserName);

			}

		}

		public async Task DeleteBasket(string userName)
		{
			var basked = await _redisCache.GetStringAsync(userName);
            if (basked != null) 
            {
                await _redisCache.RemoveAsync(userName);
            }

        }
	}
}
