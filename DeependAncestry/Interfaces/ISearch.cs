using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeependAncestry.Interfaces
{
    public interface ISearch <T,R>
    {
        R GetSearchResultByName(T req);
    }
}
