using System;
using System.Collections.Generic;

namespace CityClient.Data
{
    public class City
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public CityBasicInfo.Rootobject BasicInfo { get; set; }
        public CityDetailedInfo.Rootobject DetailedInfo { get; set; }
        public string ImageUrl { get; set; }
        public string DetailsUrl { get; set; }
        public string BasicsUrl { get; set; }
    }
}

