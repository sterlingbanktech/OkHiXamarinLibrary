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
using IO.Okhi.Android_okverify;
using SampleApp.Droid.Services;
using IO.Okhi.Android_okverify.Models;

namespace SampleApp.Droid
{
    [Activity(Label = "SampleApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
       
       
        public IO.Okhi.Android_okcollect.OkCollect okCollect;
        public OkHiConfig config;
        public OkHi okHi;
        public OkHiTheme theme;
        public OkVerify okVerify;
        public OkCollectCallback okCollectCallback;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            try
            {
                okHi = new OkHi(this);
                okCollect = new IO.Okhi.Android_okcollect.OkCollect.Builder(this).Build();
                okVerify = new IO.Okhi.Android_okverify.OkVerify.Builder(this).Build();
                okCollectCallback = new OkCollectCallback(okVerify);


                // Should be invoked one time on app start.
                // (optional) OkHiNotification, use to start a foreground service to transmit verification signals to OkHi servers
                int importance = Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N ? (int)Android.App.NotificationImportance.Default : 3;

                OkVerify.Init(this, new OkHiNotification(
                    "Verifying your address",
                    "We're currently verifying your address. This won't take long",
                    "OkHi",
                    "OkHi Address Verification",
                    "Alerts related to any address verification updates",
                    importance,
                    1, // notificationId
                    2 // notification request code
                ));


                theme = new OkHiTheme.Builder("#00fdaa")
            .SetAppBarLogo("https://cdn.okhi.co/icon.png")
            .SetAppBarColor("#ba0c2f")
            .Build();

                config = new OkHiConfig.Builder()
            .WithStreetView()
            .Build();

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
            okHi.OnRequestPermissionsResult(requestCode, permissions, grants);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            okHi.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}