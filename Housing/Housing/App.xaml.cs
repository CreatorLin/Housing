using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Housing
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            //test();
            MainPage = new Housing.MainPage();
            //MainPage = new NavigationPage(new Housing.MainPage());
            //MainPage = new SearchResultPage();
            //MainPage = new DetailPage();

        }

        private void test()
        {
            var stack = new StackLayout();
            var absoluteLayout = new AbsoluteLayout();
            var time = new Label { Text = DateTime.Now.ToString() };
            var search = new SearchBar();
            var label = new Label();
            var geocoder = new Geocoder();
            //var guangFu = geocoder.GetAddressesForPositionAsync(new Position(24.9914854, 121.4628387)).Result;
            //var position = geocoder.GetPositionsForAddressAsync(guangFu.First()).Result;
            //label.Text = $"test:{guangFu.Count()}";
            //var mapSpan = MapSpan.FromCenterAndRadius(position.First(), Distance.FromKilometers(1));
            var map = new Map();
            //map.MoveToRegion(mapSpan);

            absoluteLayout.Children.Add(map, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            //absoluteLayout.Children.Add(search, new Rectangle(10, 10, 300, 30));
            //stack.Children.Add(time);
            //stack.Children.Add(map);

            MainPage = new ContentPage
            {
                Content = absoluteLayout
            };
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
