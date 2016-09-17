using DeependAncestry.Interfaces;
using DeependAncestry.Models.Request;
using DeependAncestry.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeependAncestry.Factories
{
    public class CensusSearchFactory : ISearch<SearchedRequest, SearchedResponse>
    {
        public SearchedResponse GetSearchResultByName(SearchedRequest req)
        {
            throw new NotImplementedException();
        }
    }
}