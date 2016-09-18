using DeependAncestry.Interfaces;
using DeependAncestry.Models;
using DeependAncestry.Models.Request;
using DeependAncestry.Models.Response;
using DeependAncestry.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

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

            var query = data.People.AsQueryable()
               .Where(d => d.Name.Contains(req.Name));

            if (req.Gender!="all")
            {
                query = query.Where(d => d.Gender.ToLower() == req.Gender);
            }
            if (req.Family.ToLower() == "ancestors")
            {
                query = query.SelectMany(p => getParentsById(p.ID, p.Level, new List<Person>(), data.People));
            }

            if (req.Family.ToLower() == "descendants")
            {
                query = query.SelectMany(p => getChildrenById(p.ID, p.Gender, p.Level, new List<Person>(), data.People));
            }

            List<PersonViewModel> vmPeople = query.Join(data.Places,
                people => people.PlaceId,
                place => place.Id,
                (people, place) => new PersonViewModel() { ID = people.ID, Name = people.Name, Gender = people.Gender, BirthPlace = place.Name, Level = people.Level })
                .OrderBy(p => p.Level).ThenBy(p=>p.ID).ToList();

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
            try
            {
                string serverFilePath = HttpContext.Current.Server.MapPath(jsonFilePath);
                using (StreamReader sr = new StreamReader(serverFilePath))
                {
                    string jsonStr = sr.ReadToEnd();
                    CensusData censusData = JsonConvert.DeserializeObject<CensusData>(jsonStr);
                    return censusData;
                }
            }
            catch (Exception ex)
            {
                var exc = ex;
            }
            return null;
        }

        /// <summary>
        /// Get parents based on monther id and father id
        /// </summary>
        /// <param name="personId">Current person ID</param>
        /// <param name="currentFamilies">Current family members found for current person</param>
        /// <param name="data">Total data set</param>
        /// <returns>Totle family members found</returns>
        private List<Person> getParentsById(int personId, int lvl, List<Person> currentFamilies, IEnumerable<Person> data)
        {
            List<Person> families = currentFamilies;

            data = data.Where(p => p.Level <= lvl); // find only ancestors

            Person familyLookup = data.SingleOrDefault(p => p.ID == personId);
            if (familyLookup != null)
            {
                families.Add(familyLookup);

                if (familyLookup.MotherId != null && !families.Any(m => m.ID == familyLookup.MotherId))
                {
                    families = getParentsById(familyLookup.MotherId.Value, familyLookup.Level, families, data);
                }

                if (familyLookup.FatherId != null && !families.Any(m => m.ID == familyLookup.FatherId))
                {
                    families = getParentsById(familyLookup.FatherId.Value, familyLookup.Level, families, data);
                }
            }
                return families;
        }

        /// <summary>
        /// Get children based on monther id and father id
        /// </summary>
        /// <param name="personId">Current person ID</param>
        /// <param name="currentFamilies">Current family members found for current person</param>
        /// <param name="data">Total data set</param>
        /// <returns>Totle family members found</returns>
        private List<Person> getChildrenById(int personId, string gender, int lvl, List<Person> currentFamilies, IEnumerable<Person> data)
        {
            List<Person> families = currentFamilies;

            data = data.Where(p => p.Level >= lvl); // find only decendents
            List<Person> familyLookups = data.Where(p => p.FatherId == personId || p.MotherId == personId).ToList();

            if (familyLookups != null && familyLookups.Count > 0 && !families.Contains(familyLookups.First()))
            {
                families.AddRange(familyLookups);

                foreach (var member in familyLookups)
                {

                    families = getChildrenById(member.ID, member.Gender, member.Level, families, data);
                }

            }
            return families;
        }
    }
}
