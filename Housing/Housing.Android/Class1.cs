using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Housing.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Class1))]
namespace Housing.Droid
{
    class Class1 : IGeocoder
    {
        public string[] GetFromLocationName(string locationName, int maxResults)
        {
            var geocoder = new Geocoder(MainActivity.Instance);
            return geocoder.GetFromLocationName(locationName, maxResults).Select(p => p.ToString()).ToArray();
        }

    }
}