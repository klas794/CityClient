using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;

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
            
            AppCenter.Start("android=b55b2a7f-47e0-4323-8d88-5811179b4dfc;"
                //+ "uwp={Your UWP App secret here};" +
                //   "ios={Your iOS App secret here}"
                , typeof(Analytics), typeof(Crashes), typeof(Push));

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
