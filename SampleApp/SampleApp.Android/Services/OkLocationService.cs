using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IO.Okhi.Android_core;
using IO.Okhi.Android_core.Interfaces;
using IO.Okhi.Android_core.Models;
using IO.Okhi.Android_okcollect.Utilities;
using IO.Okhi.Android_okverify;
using IO.Okhi.Android_okverify.Models;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(SampleApp.Droid.Services.OkLocationService))]
namespace SampleApp.Droid.Services
{

    public class OkLocationService : IOkLocationService
    {
        Activity CurrentActivity;
        OkHi okHi;
        private IO.Okhi.Android_okcollect.OkCollect okCollect;
        private OkHiConfig config;
        private OkHi okhi;
        private OkHiTheme theme;
       private IO.Okhi.Android_okverify.OkVerify okVerify;
        private OkCollectCallback okCollectCallback;
        public OkLocationService()
        {
            CurrentActivity = Xamarin.Essentials.Platform.CurrentActivity;
            okHi = new OkHi(CurrentActivity);
            okCollect = new IO.Okhi.Android_okcollect.OkCollect.Builder(CurrentActivity).Build();
            okVerify = new IO.Okhi.Android_okverify.OkVerify.Builder(CurrentActivity).Build();
            okCollectCallback = new OkCollectCallback(okVerify);

           
            // Should be invoked one time on app start.
            // (optional) OkHiNotification, use to start a foreground service to transmit verification signals to OkHi servers
            int importance = Build.VERSION.SdkInt >=Android.OS.BuildVersionCodes.N ? (int)Android.App.NotificationImportance.Default : 3;
           
            OkVerify.Init(CurrentActivity, new OkHiNotification(
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
        public string GetAddress(string phoneNumber, string firstName, string lastName)
        {
            try
            {
                var canStart = this.canStartAddressCreation();
                if (canStart)
                {
                    
                    OkHiUser user = new OkHiUser.Builder(phoneNumber)
                        .WithFirstName(firstName)
                        .WithLastName(lastName)
                        .Build();

                    OkHiLocation location = new OkHiLocation.Builder("test", 3.3, 4.4).Build();
                    okCollectCallback.OnSuccess(user, location);

                    okCollect.Launch(user, okCollectCallback);
                }
            }
            catch (Exception ex) 
            {

                return "";
            }

            return "";
        }

        private bool canStartAddressCreation()
        {
            RequestHandler requestHandler = new RequestHandler();
            // Check and request user to enable location services
            if (!OkHi.IsLocationServicesEnabled(CurrentActivity))
            {
                OkHi.OpenLocationServicesSettings(CurrentActivity);
                // okhi.requestEnableLocationServices(requestHandler);
            }
            else if (!OkHi.IsGooglePlayServicesAvailable(CurrentActivity))
            {
                // Check and request user to enable google play services
                okHi.RequestEnableGooglePlayServices(requestHandler);
            }
            else if (!OkHi.IsLocationPermissionGranted(CurrentActivity))
            {
                // Check and request user to grant location permission
                okHi.RequestLocationPermission("Hey we need location permissions", "Pretty please..", requestHandler);
            }
            else
            {
                return true;
            }
            return false;
        }


    }

    public class RequestHandler : Java.Lang.Object, IOkHiRequestHandler
    {
        public void OnError(OkHiException p0)
        {

        }

        public void OnResult(Java.Lang.Object p0)
        {

        }
    }

    public class OkCollectCallback: Java.Lang.Object, IO.Okhi.Android_okcollect.Callbacks.IOkCollectCallback
    {
        private readonly OkVerify okVerify;
        private OkVerifyCallback okVerifyCallback;

        public OkCollectCallback(IO.Okhi.Android_okverify.OkVerify okVerify)
        {
            this.okVerify = okVerify;
            this.okVerifyCallback = new OkVerifyCallback();
        }
        public void OnError(OkHiException p0)
        {
            var errorMessage = p0.Message;
        }

        public void OnSuccess(Java.Lang.Object p0, Java.Lang.Object p1)
        {
            OkHiUser user = (OkHiUser)p0;
            OkHiLocation location = (OkHiLocation)p1;
            startAddressVerification(user, location);
        }

        private void startAddressVerification(OkHiUser user, OkHiLocation location)
        {
            okVerify.Start(user, location, okVerifyCallback);
        }
    }

    public class OkVerifyCallback : Java.Lang.Object, IO.Okhi.Android_okverify.Interfaces.IOkVerifyCallback
    {
        public void OnError(OkHiException p0)
        {
           
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
           
        }
    }
}