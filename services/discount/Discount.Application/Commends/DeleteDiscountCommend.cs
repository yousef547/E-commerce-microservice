using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Commends
{
    public class DeleteDiscountCommend:IRequest<bool>
    {
        public string ProductName { get; set; }
        public DeleteDiscountCommend(string productName)
		{
			ProductName = productName;

		}
    }
}
