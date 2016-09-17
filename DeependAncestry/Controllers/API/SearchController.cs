using DeependAncestry.Factories;
using DeependAncestry.Interfaces;
using DeependAncestry.Models.Request;
using DeependAncestry.Models.Response;
using System.Web.Http;

namespace DeependAncestry.Controllers.API
{
    public class SearchController : ApiController
    {
        // GET api/search/
        [HttpGet]
        public SearchedResponse Search(string Name, string Gender, string Family, int PageIndex, int ItemPerPage)
        {
            SearchedRequest req = new SearchedRequest()
            {
                Name = Name,
                Gender = Gender,
                Family = Family,
                PageIndex = PageIndex,
                ItemPerPage = ItemPerPage
            };
            ISearch<SearchedRequest, SearchedResponse> searchFactory = new CensusSearchFactory();
            SearchedResponse res = searchFactory.GetSearchResult(req);
            return res;
        }

    }
}