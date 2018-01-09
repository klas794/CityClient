using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;

namespace CityClient.Droid
{
    [Activity(Label = "City Client", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //AppCenter.Start("b55b2a7f-47e0-4323-8d88-5811179b4dfc",
            //       typeof(Analytics), typeof(Crashes));
            Push.SetSenderId("91929649101");

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            string dbPath = FileAccessHelper.GetLocalFilePath("local.db3");


            LoadApplication(new App(dbPath));

            this.ActionBar.SetIcon(Android.Resource.Color.Transparent);
        }

    }
}

