using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeependAncestry.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "father_id")]
        public int? FatherId { get; set; }

        [JsonProperty(PropertyName = "mother_id")]
        public int? MotherId { get; set; }

        [JsonProperty(PropertyName = "place_id")]
        public int? PlaceId { get; set; }

        [JsonProperty(PropertyName = "level")]
        public int Level { get; set; }
    }
}