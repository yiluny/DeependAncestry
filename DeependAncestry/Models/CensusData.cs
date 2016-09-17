using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeependAncestry.Models
{
    public class CensusData
    {
        [JsonProperty(PropertyName = "people")]
        public List<Person> People { get; set; }

        [JsonProperty(PropertyName = "places")]
        public List<Place> Places { get; set; }
    }
}