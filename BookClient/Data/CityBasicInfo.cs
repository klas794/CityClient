using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityClient.Data.CityBasicInfo
{

    public class Rootobject
    {
        public _Links _links { get; set; }
        public Alternate_Names[] alternate_names { get; set; }
        public int geoname_id { get; set; }
        public Location location { get; set; }
        public string name { get; set; }
        public int population { get; set; }
    }

    public class _Links
    {
        [JsonProperty("city:admin1_division")]
        public CityAdmin1_Division cityadmin1_division { get; set; }
        [JsonProperty("city:country")]
        public CityCountry citycountry { get; set; }
        [JsonProperty("city:timezone")]
        public CityTimezone citytimezone { get; set; }
        [JsonProperty("city:urban_area")]
        public CityUrban_Area cityurban_area { get; set; }
        public Self self { get; set; }
    }

    public class CityAdmin1_Division
    {
        public string href { get; set; }
    }

    public class CityCountry
    {
        public string href { get; set; }
    }

    public class CityTimezone
    {
        public string href { get; set; }
    }

    public class CityUrban_Area
    {
        public string href { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Location
    {
        public string geohash { get; set; }
        public Latlon latlon { get; set; }
    }

    public class Latlon
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
    }

    public class Alternate_Names
    {
        public string name { get; set; }
    }

}
