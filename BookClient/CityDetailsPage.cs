using Xamarin.Forms;
using System;
using System.Linq;
using CityClient.Data;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace CityClient
{
    public class CityDetailsPage : TabbedPage
    {
        readonly City city;
        readonly IList<City> cities;
        readonly CityManager manager;
        
        public CityDetailsPage(CityManager manager, IList<City> cities, City city = null)
        {
            this.manager = manager;
            this.cities = cities;
            this.city = city;

        }

        public async static Task<Page> CreateAsync(CityManager manager, IList<City> cities, City city = null)
        {
            var mainPage = new CityDetailsPage(manager, cities, city);

            mainPage.Title = "City Life Quality";

            //Device.BeginInvokeOnMainThread(InitCity);
            await CityManager.InitCity(mainPage.city);


            var tableView = new TableView
            {
                BackgroundColor = Color.White,
                Intent = TableIntent.Data,
                Root = new TableRoot("City") {
                    new TableSection(city.Name) {
                    },
                },
            };

            if (mainPage.city.BasicInfo != null)
            {

                tableView.Root.First().Add(new TextCell
                {
                    Text = "Population:",
                    Detail = mainPage.city.BasicInfo.population.ToString(),
                    DetailColor = Color.FromRgb(50, 50, 50),
                    TextColor = Color.Black
                });

                tableView.Root.First().Add(new TextCell
                {
                    Text = "Latitude / longitude:",
                    Detail = string.Format("{0}, {1}",
                        mainPage.city.BasicInfo.location.latlon.latitude.ToString(),
                        mainPage.city.BasicInfo.location.latlon.longitude.ToString()
                    ),
                    DetailColor = Color.FromRgb(50, 50, 50),
                    TextColor = Color.Black
                });

                //tableView.Root.Last().Add(new ViewCell
                //{
                //    View = new Map(MapSpan.FromCenterAndRadius(new Position(mainPage.city.BasicInfo.location.latlon.latitude, mainPage.city.BasicInfo.location.latlon.longitude), new Distance(50000))),
                //    Height = 400
                //});
            }

            if (mainPage.city.DetailedInfo != null)
            {
                tableView.Root.Add(new TableSection("Living info"));

                foreach (var item in mainPage.city.DetailedInfo.categories)
                {
                    tableView.Root.Last().Add(new TextCell
                    {
                        Text = item.name,
                        Detail = string.Format("Score {0} of 10", Math.Round(item.score_out_of_10, 2)),
                        DetailColor = Color.FromRgb(50, 50, 50),
                        TextColor = Color.Black,
                    });
                }

            }

            mainPage.Children.Add(new ContentPage { Title = "Info", Content = tableView, Icon = "ic_action_about.png" });

            mainPage.Children.Add(
                new ContentPage
                {
                    Title = "Map",
                    Content = 
                    new Map(
                        MapSpan.FromCenterAndRadius(
                            new Position(
                                mainPage.city.BasicInfo.location.latlon.latitude,
                                mainPage.city.BasicInfo.location.latlon.longitude), new Distance(50000)))
                });


            var image = string.IsNullOrEmpty(mainPage.city.ImageUrl) ? null :
                new Image { Source = mainPage.city.ImageUrl, VerticalOptions = LayoutOptions.FillAndExpand };

            if (image != null)
            {
                mainPage.Children.Add(new ContentPage { Title = "Photo", Content = image, Icon = "ic_action_picture.png" });

                //var layout = new StackLayout
                //{
                //    VerticalOptions = LayoutOptions.FillAndExpand,
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    Orientation = StackOrientation.Vertical,
                //    //Spacing = 10,
                //    HeightRequest = 500
                //};

                //layout.Children.Add(image);

                //tableView.Root.Add(new TableSection {
                //    new ViewCell { View = layout, Height = 500 },
                //});
            }

            //mainPage.Content = layout;
            
            return mainPage;
        }
    }
}

