using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
    public interface ITransitionCounterService
    {
        Task ResetCounter();
        Task AddPath(string path);
        IDictionary<string, int> GetCounter();
    }
}
