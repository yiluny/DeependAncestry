using System.Collections.Generic;

namespace DeependAncestry.Models.Response
{
    public class SearchedResponse
    {
        public List<Person> Results { get; set; }

        public int TotalCount { get; set; }
    }
}