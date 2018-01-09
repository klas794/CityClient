using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CityClient.Data
{
    public class CityManager
    {
        const string Url = "https://api.teleport.org/api/cities/";
        //const string Url = "https://jsonplaceholder.typicode.com/users";
        //private string authorizationKey;

        public static HttpClient client;

        private static HttpClient GetClient()
        {
            if (CityManager.client != null)
            {
                return CityManager.client;
            }

            HttpClient client = new HttpClient();
            //if (string.IsNullOrEmpty(authorizationKey))
            //{
            //    authorizationKey = await client.GetStringAsync(Url + "login");
            //    authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
            //}

            //client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            CityManager.client = client;

            return client;
        }

        public async Task<IEnumerable<City>> GetAll()
        {
            HttpClient client = CityManager.GetClient();

            List<City> cities = new List<City>();

            string result = await client.GetStringAsync(Url);
            var citySearch = JsonConvert.DeserializeObject<CitySearchResult.Rootobject>(result);

            foreach (var item in citySearch._embedded.citysearchresults)
            {
                var shortName = item.matching_full_name.Substring(0, item.matching_full_name.IndexOf(','));

                cities.Add(new City {
                    Name = item.matching_full_name,
                    ShortName = shortName,
                    //BasicInfo = basicInfo,
                    //DetailedInfo = !string.IsNullOrEmpty(urbanAreaLink) ?
                    //    await GetDetailedInfo(client, urbanAreaLink) : null,
                    BasicsUrl = item._links.cityitem.href,
                });

                //await InitCity(cities.Last());
            }
            
            return cities;
        }

        public static async Task InitCity(City city)
        {
            CityManager.GetClient();

            city.BasicInfo = await CityManager.GetBasicInfo(CityManager.client, city.BasicsUrl);

            city.DetailsUrl = city.BasicInfo._links.cityurban_area == null ? null :
                string.Concat(city.BasicInfo._links.cityurban_area.href, "scores/");

            var imagesUrl = city.BasicInfo._links.cityurban_area == null ? null :
                string.Concat(city.BasicInfo._links.cityurban_area.href, "images/");

            if (city.DetailsUrl != null) { 
                city.DetailedInfo = await CityManager.GetDetailedInfo(CityManager.client, city.DetailsUrl);
            }

            city.ImageUrl = imagesUrl == null ? null : await CityManager.GetImage(CityManager.client, imagesUrl);
        }

        public static async Task<string> GetImage(HttpClient client, string url)
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            List<City> images = new List<City>();

            //url = string.Format("{0}images/", url);

            string result = await client.GetStringAsync(url);
            var imageSearch = JsonConvert.DeserializeObject<CityImages.Rootobject>(result);

            return imageSearch.photos.FirstOrDefault()?.image.mobile;

            //foreach (var item in imageSearch.photos)
            //{

            //    images.Add(new City
            //    {
            //        Name = item.matching_full_name,
            //        BasicInfo = basicInfo,
            //        DetailedInfo = !string.IsNullOrEmpty(urbanAreaLink) ?
            //            await GetDetailedInfo(client, urbanAreaLink) : null
            //    });
            //}
        }

        public static async Task<CityBasicInfo.Rootobject> GetBasicInfo(HttpClient client, string url)
        {
            List<City> cities = new List<City>();

            string result = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<CityBasicInfo.Rootobject>(result);
        }

        public static async Task<CityDetailedInfo.Rootobject> GetDetailedInfo(HttpClient client, string url)
        {
            List<City> cities = new List<City>();

            string result = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<CityDetailedInfo.Rootobject>(result);
        }
        
    }
}

