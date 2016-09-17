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
                .OrderBy(p => p.Level).ToList();

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
        private List<Person> getParentsById(int personId, int lvl, List<Person> currentFamilies, IEnumerable<Person> data)
        {
            List<Person> families = currentFamilies;

            data = data.Where(p => p.Level <= lvl); // find only ancestors

            Person familyLookup = data.SingleOrDefault(p => p.ID == personId);
            families.Add(familyLookup);

            if (familyLookup.MotherId != null)
            {
                families = getParentsById(familyLookup.MotherId.Value, familyLookup.Level, families, data);
            }

            if (familyLookup.FatherId != null)
            {
                families = getParentsById(familyLookup.FatherId.Value, familyLookup.Level, families, data);
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
            Person person = new Person()
            {
                ID = personId,
            };
            //CoordinatesBasedComparer c = new CoordinatesBasedComparer();
            //data.ToList().Sort(c);
            //int index = data.ToList().BinarySearch(person, c);
            //List<Person> familyLookups = data.ToList().BinarySearchMultiple(person, (a, b) => a.FatherId.Value.CompareTo(b.ID));
            List<Person> familyLookups = data.Where(p => p.FatherId == personId || p.MotherId == personId).ToList();
            
            families.AddRange(familyLookups);

            foreach (var member in familyLookups)
            {
                families = getChildrenById(member.ID, member.Gender, member.Level, families, data);
            }
            return families;
        }
    }
}

//public class CoordinatesBasedComparer : IComparer<Person>
//{
//    public int Compare(Person person, int personId)
//    {
//        if (person.FatherId.Value.CompareTo(personId) != 0)
//        {
//            return person.FatherId.Value.CompareTo(personId);
//        }
//        else if (person.MotherId.Value.CompareTo(personId) != 0)
//        {
//            return person.MotherId.Value.CompareTo(personId);
//        }
//        else
//        {
//            return 0;
//        }
//    }
//}

public static class ListExtensions
{
    public static int BinarySearch<T>(this List<T> list,
                                     T item,
                                     Func<T, T, int> compare)
    {
        return list.BinarySearch(item, new ComparisonComparer<T>(compare));
    }

    public static T BinarySearchOrDefault<T>(this List<T> list,
                                         T item,
                                         Func<T, T, int> compare)
    {
        int i = list.BinarySearch(item, compare);
        if (i >= 0)
            return list[i];
        return default(T);
    }
    public static List<T> BinarySearchMultiple<T>(this List<T> list,
                                                T item,
                                                Func<T, T, int> compare)
    {
        var results = new List<T>();
        int i = list.BinarySearch(item, compare);
        if (i >= 0)
        {
            results.Add(list[i]);
            int below = i;
            while (--below >= 0)
            {
                int belowIndex = compare(list[below], item);
                if (belowIndex < 0)
                    break;
                results.Add(list[belowIndex]);
            }

            int above = i;
            while (++above < list.Count)
            {
                int aboveIndex = compare(list[above], item);
                if (aboveIndex > 0)
                    break;
                results.Add(list[aboveIndex]);
            }
        }
        return results;
    }
}



public class ComparisonComparer<T> : IComparer<T>
{
    private readonly Comparison<T> comparison;

    public ComparisonComparer(Func<T, T, int> compare)
    {
        if (compare == null)
        {
            throw new ArgumentNullException("comparison");
        }
        comparison = new Comparison<T>(compare);
    }

    public int Compare(T x, T y)
    {
        return comparison(x, y);
    }
}