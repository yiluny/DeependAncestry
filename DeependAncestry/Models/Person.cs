using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeependAncestry.Models
{
    public class Person
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int FatherId { get; set; }

        public int MontherId { get; set; }

        public int PlaceId { get; set; }

        public int Level { get; set; }
    }
}