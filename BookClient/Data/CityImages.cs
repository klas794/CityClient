using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityClient.Data.CityImages
{

    public class Rootobject
    {
        public _Links _links { get; set; }
        public Photo[] photos { get; set; }
    }

    public class _Links
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

    public class Photo
    {
        public Attribution attribution { get; set; }
        public Image image { get; set; }
    }

    public class Attribution
    {
        public string license { get; set; }
        public string photographer { get; set; }
        public string site { get; set; }
        public string source { get; set; }
    }

    public class Image
    {
        public string mobile { get; set; }
        public string web { get; set; }
    }

}
