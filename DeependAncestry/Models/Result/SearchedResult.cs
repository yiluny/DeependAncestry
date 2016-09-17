using System.Collections.Generic;

namespace DeependAncestry.Models.Result
{
    public class SearchedResult
    {
        public List<Person> Results { get; set; }

        public int TotalCount { get; set; }
    }
}