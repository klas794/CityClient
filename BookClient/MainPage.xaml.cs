using CityClient.Data;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CityClient
{
    public partial class MainPage : ContentPage
    {
        readonly IList<City> cities = new ObservableCollection<City>();
        readonly CityManager manager = new CityManager();
        private IEnumerable<City> citiesCache;
        private bool viewingBookmarks;

        public MainPage()
        {
            BindingContext = cities;
            InitializeComponent();

            Push.PushNotificationReceived += OnPushNotificationReceived;
        }

        private async void OnPushNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            await DisplayAlert(e.Title, e.Message, "OK");
        }

        public bool InternetConnectionExists()
        {
            if (!CrossConnectivity.IsSupported)
                return true;

            return CrossConnectivity.Current.IsConnected;
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            if (!InternetConnectionExists())
            {
                await DisplayAlert("No internet", "You do not have internet access", "OK");
                return;
            }

            Analytics.TrackEvent("Refreshing list of cities");

            // Turn on network indicator
            this.IsBusy = true;

            try {

                SearchButton.IsEnabled = false;
                ViewAllButton.IsEnabled = false;
                CitySearchText.IsEnabled = false;
                StartUpInfo.IsVisible = false;

                var cityCollection = await manager.GetAll();

                cityCollection = cityCollection.OrderBy(x => x.ShortName);


                foreach (City city in cityCollection)
                {
                    if (cities.All(b => b.Name != city.Name))
                        cities.Add(city);
                }

                SearchButton.IsEnabled = true;
                ViewAllButton.IsEnabled = true;
                CitySearchText.IsEnabled = true;

                var bookmarks = await App.FavouritesRepo.GetAllFavouritesAsync();

                if (cities.Where(x => bookmarks.Any(y => y.Name == x.ShortName)).Count() > 0)
                {
                    ViewBookmarksButton.IsEnabled = true;
                }

                citiesCache = cityCollection;
            }
            finally {
                this.IsBusy = false;
            }
        }
        
        async void OnViewCityDetails(object sender, ItemTappedEventArgs e)
        {
            Analytics.TrackEvent("Viewing city: " + ((City)e.Item).Name);
            await Navigation.PushModalAsync(
                await CityDetailsPage.CreateAsync(manager, cities, (City)e.Item));
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            var searchText = CitySearchText.Text;

            if(searchText == "secreterror")
            {
                throw new Exception("Secret error exception triggered");
            }

            Analytics.TrackEvent("Searching for " + searchText,
                 new Dictionary<string, string> {
                    { "SearchTerm", searchText },
                });

            if(!string.IsNullOrEmpty(searchText))
            {
                cities.Clear();

                var searchResult = citiesCache.Where(x => x.ShortName.ToLower().Contains(searchText.ToLower()));

                foreach (var item in searchResult)
                {
                    cities.Add(item);
                }
            }
            else
            {
                DisplayAlert("Search", "Enter a search word to search", "OK");
            }

            viewingBookmarks = false;
        }

        private void ViewAllButton_Clicked(object sender, EventArgs e)
        {
            //Crashes.GenerateTestCrash();
            Analytics.TrackEvent("Viewing all cities");

            cities.Clear();

            foreach (var item in citiesCache)
            {
                cities.Add(item);
            }

            viewingBookmarks = false;
        }

        private async void OnBookmark(object sender, EventArgs e)
        {
            

            MenuItem item = (MenuItem)sender;
            //item.Icon = "ic_action_undo.png";
            //item.Clicked -= OnBookmark;
            //item.Clicked += OnRemoveBookmark;

            City city = item.CommandParameter as City;

            var all = await App.FavouritesRepo.GetAllFavouritesAsync();

            if(all.Any(x => x.Name == city.ShortName))
            {
                OnRemoveBookmark(sender, e);
                return;
            }

            Analytics.TrackEvent("Bookmarking city");

            await App.FavouritesRepo.AddNewFavouriteAsync(city.ShortName);

            ViewBookmarksButton.IsEnabled = true;
        }

        private async void OnRemoveBookmark(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Removing city bookmark");

            MenuItem item = (MenuItem)sender;
            //item.Icon = "ic_action_favorite.png";
            //item.Clicked -= OnRemoveBookmark;
            //item.Clicked += OnBookmark;

            City city = item.CommandParameter as City;

            await App.FavouritesRepo.RemoveFavouriteAsync(city.ShortName);

            var all = await App.FavouritesRepo.GetAllFavouritesAsync();
            ViewBookmarksButton.IsEnabled = all.Count > 0;

            ViewBookmarksButton_Clicked(null, null);
        }


        private async void ViewBookmarksButton_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Viewing bookmarks list");

            var cityNames = await App.FavouritesRepo.GetAllFavouritesAsync();

            var favouriteCities = citiesCache.Where(x => cityNames.Any(y => y.Name == x.ShortName));

            cities.Clear();

            foreach (var item in favouriteCities)
            {
                cities.Add(item);
            }

            viewingBookmarks = true;
        }

        private void CitySearchText_Completed(object sender, EventArgs e)
        {
            SearchButton_Clicked(sender, e);
        }
    }
}
