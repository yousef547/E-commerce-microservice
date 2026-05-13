using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commends
{
    public class DeleteBasketByUserNameCommend :IRequest<Unit>
    {
        public string UserName { get; set; }

        public DeleteBasketByUserNameCommend(string userName)
        {
			UserName = userName;
		}
    }
}
