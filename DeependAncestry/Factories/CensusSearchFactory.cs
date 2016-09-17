using DeependAncestry.Interfaces;
using DeependAncestry.Models;
using DeependAncestry.Models.Request;
using DeependAncestry.Models.Response;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace DeependAncestry.Factories
{
    public class CensusSearchFactory : ISearch<SearchedRequest, SearchedResponse>
    {
        private string jsonFilePath = ConfigurationManager.AppSettings["CensusDataPath"];

        public SearchedResponse GetSearchResultByName(SearchedRequest req)
        {
            CensusData data = this.getCensusDataFromJson();
            return null;
        }

        private CensusData getCensusDataFromJson()
        {
            using (StreamReader sr = new StreamReader(jsonFilePath))
            {
                string jsonStr = sr.ReadToEnd();
                CensusData censusData = JsonConvert.DeserializeObject<CensusData>(jsonStr);
                return censusData;
            }
        }
    }
}