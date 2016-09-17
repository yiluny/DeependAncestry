using DeependAncestry.Factories;
using DeependAncestry.Interfaces;
using DeependAncestry.Models.Request;
using DeependAncestry.Models.Response;
using System.Web.Http;

namespace DeependAncestry.Controllers.API
{
    public class SearchController : ApiController
    {
        // POST api/search/
        public SearchedResponse Search(SearchedRequest req)
        {
            ISearch<SearchedRequest, SearchedResponse> searchFactory = new CensusSearchFactory();
            SearchedResponse res = searchFactory.GetSearchResultByName(req);
            return res;
        }
    }
}