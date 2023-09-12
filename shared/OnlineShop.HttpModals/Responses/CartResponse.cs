using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModals.Responses
{
    public record CartResponse
    {
        public List<ItemResponse> Items { get; set; } = new List<ItemResponse>();
    };
    public record ItemResponse(Guid Id, string ProductName, double Quantity);
}
