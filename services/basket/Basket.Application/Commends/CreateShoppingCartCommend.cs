using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commends
{
    public class CreateShoppingCartCommend:IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public CreateShoppingCartCommend(string userName, List<ShoppingCartItem> items)
        {
			UserName = userName;
			Items=items;

		}
    }
}
