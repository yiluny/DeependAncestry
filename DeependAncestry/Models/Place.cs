﻿using Newtonsoft.Json;
namespace DeependAncestry.Models
{
    public class Place
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}