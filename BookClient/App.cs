using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CityClient
{
	public class App : Application
	{
        public static FavouritesRepository FavouritesRepo { get; private set; }

        public App(string dbPath)
        {
            //set database path first, then retrieve main page
            FavouritesRepo = new FavouritesRepository(dbPath);

            // The root page of your application
            MainPage = new NavigationPage(new MainPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
