using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Housing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchResultPage : ContentPage
	{
        public House[] Data{ get; set; }
        private SearchResult[] ItemSource { get; set; }
        public SearchResultPage(House[] data)
        {
            this.Data = data;
            Init();
        }

		public void Init ()
		{
			InitializeComponent ();

            var ItemSource = Data.Select((p, index) => new SearchResult
            {
                Index = index,
                Title = p.土地區段位置或建物區門牌,
                Detail = p.移轉層次 + p.總樓層數 + p.建物移轉總面積平方公尺 ,
                Price = p.總價元.ToString()
            });

            listView.ItemsSource = ItemSource;
            listView.ItemSelected += async (object sender, SelectedItemChangedEventArgs args) =>
            {
                var d = Data[(args.SelectedItem as SearchResult).Index];
                
                await Navigation.PushModalAsync(new DetailPage(d));
            };
        }

        private class SearchResult
        {
            public int Index { get; set; }
            public string Title { get; set; }
            public string Detail { get; set; }
            public string Price { get; set; }
        }
	}
}