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
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SampleApp.Droid.Services.OkLocationService))]
namespace SampleApp.Droid.Services
{

    public class OkLocationService : IOkLocationService
    {
        MainActivity CurrentActivity;
       // private IO.Okhi.Android_okcollect.OkCollect okCollect;
        //private OkHiConfig config;
        //private OkHi okHi;
        //private OkHiTheme theme;
        //private OkVerify okVerify;
       // private OkCollectCallback okCollectCallback;
        public OkLocationService()
        {
            CurrentActivity = (MainActivity)Xamarin.Essentials.Platform.CurrentActivity;
       
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

                    //  OkHiLocation location = new OkHiLocation.Builder("test", 3.3, 4.4).Build();
                    //  okCollectCallback.OnSuccess(user, location);

                    CurrentActivity.okCollect.Launch(user, CurrentActivity.okCollectCallback);
                }
            }
            catch (Exception ex)
            {
                ex.TrackError();
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
                CurrentActivity.okHi.RequestEnableLocationServices(requestHandler);
            }
            else if (!OkHi.IsGooglePlayServicesAvailable(CurrentActivity))
            {
                // Check and request user to enable google play services
                CurrentActivity.okHi.RequestEnableGooglePlayServices(requestHandler);
            }
            else if (!OkHi.IsLocationPermissionGranted(CurrentActivity))
            {
                // Check and request user to grant location permission
                CurrentActivity.okHi.RequestLocationPermission("Hey we need location permissions", "Pretty please..", requestHandler);
            }
            else if (!OkHi.IsBackgroundLocationPermissionGranted(CurrentActivity))
            {
                // Check and request user to grant location permission
                CurrentActivity.okHi.RequestBackgroundLocationPermission("Hey we need location permissions", "Pretty please..", requestHandler);
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
            p0?.TrackError();
        }

        public void OnResult(Java.Lang.Object p0)
        {

        }
    }

    public class OkCollectCallback : Java.Lang.Object, IO.Okhi.Android_okcollect.Callbacks.IOkCollectCallback
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
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
            });
            p0?.TrackError();
        }

        public void OnSuccess(Java.Lang.Object p0, Java.Lang.Object p1)
        {
            OkHiUser user = (OkHiUser)p0;
            OkHiLocation location = (OkHiLocation)p1;
            startAddressVerification(user, location);
        }

        private async void startAddressVerification(OkHiUser user, OkHiLocation location)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            okVerify.Start(user, location, okVerifyCallback);
        }
    }

    public class OkVerifyCallback : Java.Lang.Object, IO.Okhi.Android_okverify.Interfaces.IOkVerifyCallback
    {

        public void OnError(OkHiException p0)
        {
            var errorMessage = p0.Message;
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
            });
            p0!.TrackError();
        }

        public void OnSuccess(Java.Lang.Object p0)
        {

        }
    }
}