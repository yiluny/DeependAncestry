﻿using DeependAncestry.Interfaces;
using DeependAncestry.Models;
using DeependAncestry.Models.Request;
using DeependAncestry.Models.Response;
using DeependAncestry.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DeependAncestry.Factories
{
    public class CensusSearchFactory : ISearch<SearchedRequest, SearchedResponse>
    {
        private string jsonFilePath = ConfigurationManager.AppSettings["CensusDataPath"];

        /// <summary>
        /// Get searched results based on the name and other conditions from user request
        /// </summary>
        /// <param name="req">User request</param>
        /// <returns>Search result</returns>
        public SearchedResponse GetSearchResult(SearchedRequest req)
        {
            SearchedResponse res = new SearchedResponse();
            CensusData data = this.getCensusDataFromJson();
            List<PersonViewModel> vmPeople = data.People.AsQueryable()
                .Where(d => d.Name.Contains(req.Name))
                .SelectMany(p => getParentsById(p.ID, new List<Person>(), data.People))
                .Join(data.Places,
                people => people.PlaceId,
                place => place.Id,
                (people, place) => new PersonViewModel() { ID = people.ID, Name = people.Name, Gender = people.Gender, BirthPlace = place.Name,Level = people.Level })
                .OrderBy(p=>p.Level).ToList();

            if (vmPeople != null && vmPeople.Count > 0)
            {
                res.TotalCount = vmPeople.Count;
                res.Results = vmPeople.Skip(req.PageIndex * req.ItemPerPage).Take(req.ItemPerPage).ToList();
            }
            return res;
        }

        /// <summary>
        /// Get census data from json file
        /// </summary>
        /// <returns>Census data</returns>
        private CensusData getCensusDataFromJson()
        {
            using (StreamReader sr = new StreamReader(jsonFilePath))
            {
                string jsonStr = sr.ReadToEnd();
                CensusData censusData = JsonConvert.DeserializeObject<CensusData>(jsonStr);
                return censusData;
            }
        }

        /// <summary>
        /// Get parents based on monther id and father id
        /// </summary>
        /// <param name="personId">Current person ID</param>
        /// <param name="currentFamilies">Current family members found for current person</param>
        /// <param name="data">Total data set</param>
        /// <returns>Totle family members found</returns>
        private List<Person> getParentsById(int personId, List<Person> currentFamilies, HashSet<Person> data)
        {
            List<Person> families = currentFamilies;

            Person familyLookup = data.SingleOrDefault(p => p.ID == personId);
            families.Add(familyLookup);

            if (familyLookup.MotherId!= null)
            {
                families = getParentsById(familyLookup.MotherId.Value, families, data);
            }

            if (familyLookup.FatherId != null)
            {
                families = getParentsById(familyLookup.FatherId.Value, families, data);
            }
            return families;
        }
    }
}