using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityClient.Data.CitySearchResult
{

    public class Rootobject
    {
        public _Embedded _embedded { get; set; }
        public _Links1 _links { get; set; }
        public int count { get; set; }
    }

    public class _Embedded
    {
        [JsonProperty("city:search-results")]
        public List<CitySearchResults> citysearchresults { get; set; }
    }

    public class CitySearchResults
    {
        public _Links _links { get; set; }
        public List<object> matching_alternate_names { get; set; }
        public string matching_full_name { get; set; }
    }

    public class _Links
    {
        [JsonProperty("city:item")]
        public CityItem cityitem { get; set; }
    }

    public class CityItem
    {
        public string href { get; set; }
    }

    public class _Links1
    {
        public Cury[] curies { get; set; }
        public Self self { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Cury
    {
        public string href { get; set; }
        public string name { get; set; }
        public bool templated { get; set; }
    }

}
