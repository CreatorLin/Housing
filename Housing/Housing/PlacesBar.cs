using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Housing
{
    /// <summary>
	/// Place type.
	/// </summary>
	public enum PlaceType
    {
        ///<summary>Get all place types</summary>
        All,
        ///<summary>Get geocode place types</summary>
        Geocode,
        ///<summary>Get address place types</summary>
        Address,
        ///<summary>Get establishment place types</summary>
        Establishment,
        ///<summary>Get regional place types</summary>
        Regions,
        ///<summary>Get city place types</summary>
        Cities
    }

    /// <summary>
	/// Places.
	/// </summary>
	public static class Places
    {
        /// <summary>
        /// Gets the place.
        /// </summary>
        /// <returns>The place.</returns>
        /// <param name="placeID">Place identifier.</param>
        /// <param name="apiKey">API key.</param>
        public static async Task<Place> GetPlace(string placeID, string apiKey)
        {
            try
            {
                var requestURI = CreateDetailsRequestUri(placeID, apiKey);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PlacesBar HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == "ERROR")
                {
                    Debug.WriteLine("PlacesSearchBar Google Places API returned ERROR");
                    return null;
                }

                return new Place(JObject.Parse(result));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Creates the details request URI.
        /// </summary>
        /// <returns>The details request URI.</returns>
        /// <param name="place_id">Place identifier.</param>
        /// <param name="apiKey">API key.</param>
        static string CreateDetailsRequestUri(string place_id, string apiKey)
        {
            var url = "https://maps.googleapis.com/maps/api/place/details/json";
            return $"{url}?placeid={Uri.EscapeUriString(place_id)}&key={apiKey}";
        }
    }

    /// <summary>
	/// LocationBias object enables location biasing for PlacesBar Google Places API requests
	/// </summary>
	public class LocationBias
    {
        /// <summary>
        /// The latitude.
        /// </summary>
        public readonly double latitude;

        /// <summary>
        /// The longitude.
        /// </summary>
        public readonly double longitude;

        /// <summary>
        /// The radius.
        /// </summary>
        public readonly int radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DurianCode.PlacesSearchBar.LocationBias"/> class.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        /// <param name="radius">Radius.</param>
        public LocationBias(double latitude, double longitude, int radius)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.radius = radius;
        }

        /// <summary>
        /// Returns a formatted <see cref="T:System.String"/> 
        /// that represents the current <see cref="T:DurianCode.PlacesSearchBar.LocationBias"/> for PlacesBar API calls.
        /// </summary>
        /// <returns>A formatted <see cref="T:System.String"/> 
        /// that represents the current <see cref="T:DurianCode.PlacesSearchBar.LocationBias"/> for PlacesBar API calls..</returns>
        public override string ToString()
        {
            return $"&location={latitude},{longitude}&radius={radius}";
        }
    }

    /// <summary>
    /// The Components object enables filtering locations for PlacesBar Google Places API requests
    /// The components parameter of the google places API allows restricting results to specific countries
    /// </summary>
    public class Components
    {
        private string components;

        /// <summary>
        /// Initialises a new instance of the <see cref="T:DurianCode.PlacesSearchBar.Components"/> class
        /// </summary>
        /// <param name="components">A components string as per the google places API (eg. contry:au|country=nz)</param>
        public Components(string components)
        {
            this.components = components;
        }

        public override string ToString()
        {
            return $"&components={components}";
        }
    }

    /// <summary>
    /// Places retrieved event handler.
    /// </summary>
    public delegate void PlacesRetrievedEventHandler(object sender, AutoCompleteResult result);


    /// <summary>
    /// Places bar.
    /// </summary>
    public class PlacesBar : SearchBar
    {
        /// <summary>
        /// The place type.
        /// </summary>
        PlaceType placeType = PlaceType.All;

        /// <summary>
        /// The location bias.
        /// </summary>
        LocationBias locationBias;

        /// <summary>
        /// The components
        /// </summary>
        Components components;

        /// <summary>
        /// The API key.
        /// </summary>
        string apiKey;

        /// <summary>
        /// The minimum search text.
        /// </summary>
        int minimumSearchText;

        #region Property accessors
        /// <summary>
        /// Gets or sets the place type.
        /// </summary>
        /// <value>The type.</value>
        public PlaceType Type
        {
            get
            {
                return placeType;
            }
            set
            {
                placeType = value;
            }
        }

        /// <summary>
        /// Gets or sets the location bias.
        /// </summary>
        /// <value>The bias.</value>
        public LocationBias Bias
        {
            get
            {
                return locationBias;
            }
            set
            {
                locationBias = value;
            }
        }

        /// <summary>
        /// Gets or sets the components
        /// </summary>
        public Components Components
        {
            get
            {
                return components;
            }
            set
            {
                components = value;
            }
        }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey
        {
            get
            {
                return apiKey;
            }
            set
            {
                apiKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum search text.
        /// </summary>
        /// <value>The minimum search text.</value>
        public int MinimumSearchText
        {
            get
            {
                return minimumSearchText;
            }
            set
            {
                minimumSearchText = value;
            }
        }
        #endregion

        /// <summary>
        /// The places retrieved handler.
        /// </summary>
        public PlacesRetrievedEventHandler PlacesRetrieved;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DurianCode.PlacesSearchBar.PlacesBar"/> class.
        /// </summary>
        public PlacesBar()
        {
            TextChanged += OnTextChanged;
        }

        /// <summary>
        /// Handles changes to search text.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length >= minimumSearchText)
            {
                var predictions = await GetPlaces(e.NewTextValue);
                if (PlacesRetrieved != null && predictions != null)
                    PlacesRetrieved(this, predictions);
                else
                    PlacesRetrieved(this, new AutoCompleteResult());
            }
            else
            {
                PlacesRetrieved(this, new AutoCompleteResult());
            }
        }

        /// <summary>
        /// Calls the Google Places API to retrieve autofill suggestions
        /// </summary>
        /// <returns>The places.</returns>
        /// <param name="newTextValue">New text value.</param>
        async Task<AutoCompleteResult> GetPlaces(string newTextValue)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception(
                    string.Format("You have not assigned a Google API key to PlacesBar"));
            }

            try
            {
                var requestURI = CreatePredictionsUri(newTextValue);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PlacesBar HTTP request denied.");
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result == "ERROR")
                {
                    Debug.WriteLine("PlacesSearchBar Google Places API returned ERROR");
                    return null;
                }

                return JsonConvert.DeserializeObject<AutoCompleteResult>(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Creates the predictions URI.
        /// </summary>
        /// <returns>The predictions URI.</returns>
        /// <param name="newTextValue">New text value.</param>
        string CreatePredictionsUri(string newTextValue)
        {
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/json";
            var input = Uri.EscapeUriString(newTextValue);
            var pType = PlaceTypeValue(placeType);
            var constructedUrl = $"{url}?input={input}&types={pType}&key={apiKey}&key=zh-tw";

            if (locationBias != null)
                constructedUrl = constructedUrl + locationBias;
            if (components != null)
                constructedUrl += components;
            
            return constructedUrl;
        }

        /// <summary>
        /// Returns a string representation of the specified place type.
        /// </summary>
        /// <returns>The type value.</returns>
        /// <param name="type">Type.</param>
        string PlaceTypeValue(PlaceType type)
        {
            switch (type)
            {
                case PlaceType.All:
                    return "";
                case PlaceType.Geocode:
                    return "geocode";
                case PlaceType.Address:
                    return "address";
                case PlaceType.Establishment:
                    return "establishment";
                case PlaceType.Regions:
                    return "(regions)";
                case PlaceType.Cities:
                    return "(cities)";
                default:
                    return "";
            }
        }

    }

    /// <summary>
	/// Place.
	/// </summary>
	public class Place
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the raw.
        /// </summary>
        /// <value>The raw.</value>
        public string Raw { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DurianCode.PlacesSearchBar.Place"/> class.
        /// </summary>
        /// <param name="jsonObject">Json object.</param>
        public Place(JObject jsonObject)
        {
            Name = (string)jsonObject["result"]["name"];
            Latitude = (double)jsonObject["result"]["geometry"]["location"]["lat"];
            Longitude = (double)jsonObject["result"]["geometry"]["location"]["lng"];
            Raw = jsonObject.ToString();
        }
    }

    /// <summary>
	/// Auto complete result.
	/// </summary>
	public class AutoCompleteResult
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the auto complete places.
        /// </summary>
        /// <value>The auto complete places.</value>
        [JsonProperty("predictions")]
        public List<AutoCompletePrediction> AutoCompletePlaces { get; set; }
    }

    /// <summary>
	/// Auto complete prediction.
	/// </summary>
	public class AutoCompletePrediction
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the place identifier.
        /// </summary>
        /// <value>The place identifier.</value>
        [JsonProperty("place_id")]
        public string Place_ID { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
