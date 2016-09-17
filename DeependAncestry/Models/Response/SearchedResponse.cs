using DeependAncestry.ViewModels;
using System.Collections.Generic;

namespace DeependAncestry.Models.Response
{
    public class SearchedResponse
    {
        public List<PersonViewModel> Results { get; set; }

        public int TotalCount { get; set; }
    }
}