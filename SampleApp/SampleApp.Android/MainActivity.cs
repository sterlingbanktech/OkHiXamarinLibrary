using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using IO.Okhi.Android_core;
using IO.Okhi.Android_okcollect;
using IO.Okhi.Android_core.Models;
using IO.Okhi.Android_okcollect.Utilities;
using System.Linq;
using Android.Content;

namespace SampleApp.Droid
{
    [Activity(Label = "SampleApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
       
        private OkHi okhi;
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            try
            {
                okhi = new OkHi(this);
                
            }
            catch (OkHiException exception)
            {
                // exception.printStackTrace();
            }


            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            // Pass permission results to okhi
            var grants = grantResults.Select(c=>(int)c).ToArray();
            okhi.OnRequestPermissionsResult(requestCode, permissions, grants);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            okhi.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}