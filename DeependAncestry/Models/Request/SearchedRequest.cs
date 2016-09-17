namespace DeependAncestry.Models.Request
{
    public class SearchedRequest
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Family { get; set; }

        public int PageIndex { get; set; }

        public int ItemPerPage { get; set; }
    }
}