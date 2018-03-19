using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Housing
{
    public partial class MainPage : ContentPage
    {
        const string GooglePlacesApiKey = "AIzaSyDS1n5O84EBjIP5_t54lxCQFSDOh77lJQU";

        public MainPage()
        {
            //var geocoder = new Geocoder();
            //var positions = await geocoder.GetPositionsForAddressAsync(searchBar.Text);

            InitializeComponent();
            PlacesBarInit();
            //listView.ItemTapped += async (object sender, ItemTappedEventArgs args) =>
            //{
            //    //listView.IsVisible = false;
            //    listView.ItemsSource = null;
            //    await Search();

            //};

            //searchBar.SearchButtonPressed += async (object sender, EventArgs args) =>
            //{

            //    await Search();

            //};

            //searchBar.TextChanged += (object sender, TextChangedEventArgs args) =>
            //{
            //    //listView.ItemsSource = Enumerable.Repeat("Ttest", 10);
            //    var geocoder = DependencyService.Get<IGeocoder>();
            //    listView.ItemsSource = geocoder.GetFromLocationName(searchBar.Text, 10);
            //};


        }


        public void PlacesBarInit()
        {
            searchBar.ApiKey = GooglePlacesApiKey;
            searchBar.Type = PlaceType.Address;
            //searchBar.Components = new Components("country:tw");
            searchBar.PlacesRetrieved += (object sender, AutoCompleteResult result) =>
            {
                listView.ItemsSource = result.AutoCompletePlaces;
                spinner.IsRunning = false;
                spinner.IsVisible = false;

                if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                    listView.IsVisible = true;

                searchBar.TextChanged += (object s, TextChangedEventArgs e) =>
                {
                    var isNewTextValueNull = string.IsNullOrEmpty(e.NewTextValue);

                    listView.IsVisible = isNewTextValueNull;
                    spinner.IsVisible = !isNewTextValueNull;
                    spinner.IsRunning = !isNewTextValueNull;
                };

                searchBar.MinimumSearchText = 2;
                listView.ItemSelected += async (object s, SelectedItemChangedEventArgs e) =>
                    {
                        if (e.SelectedItem == null)
                            return;

                        var prediction = (AutoCompletePrediction)e.SelectedItem;
                        listView.SelectedItem = null;
                        listView.IsVisible = false;

                        var place = await Places.GetPlace(prediction.Place_ID, GooglePlacesApiKey);
                        var position = new Position(place.Latitude, place.Longitude);

                        map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.5)));

                        var pin = new Pin
                        {
                            Type = PinType.SavedPin,
                            Position = position,
                            Label = "test1 Label",
                            Address = "test Address"
                        };
                        map.Pins.Add(pin);

                        //if (place != null)
                        //    await DisplayAlert(
                        //        place.Name, string.Format("Lat: {0}\nLon: {1}", place.Latitude, place.Longitude), "OK");
                    };
            };
        }
        /*
        private async Task Search()
        {
            //var client = new HttpClient();
            //var apiResponesString = client.GetStringAsync("https://www.jinma.io/MsgsByGeoAppUser?CLat=24.961477894537222&CLng=121.43368281249022&SLat=0.08372720381083454&SLng=0.6533432006835938&AppID=16VHVHiLd3NzX&UserID=128DEi3hheGXG").Result;
            var apiResponesString = RESULT.Text;

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponesString);
            var houses = apiResponse.Msgs.Select(p => JsonConvert.DeserializeObject<House>(p.Body.Replace("\\", ""))).ToArray();
            await Navigation.PushAsync(new SearchResultPage(houses));

        }*/
    }
}
