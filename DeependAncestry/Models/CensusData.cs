using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeependAncestry.Models
{
    public class CensusData
    {
        [JsonProperty(PropertyName = "people")]
        public HashSet<Person> People { get; set; }

        [JsonProperty(PropertyName = "places")]
        public HashSet<Place> Places { get; set; }
    }
}