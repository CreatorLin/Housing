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
	public partial class DetailPage : ContentPage
    {
        public DetailPage(House data)
        {
            InitializeComponent();
            DataBidding();
            //var data = new House();

            listView.ItemsSource = GetCells(data);
        }

        private void DataBidding()
        {
            var textCell = listView.ItemTemplate;
            textCell.SetBinding(TextCell.TextProperty, nameof(TextCell.Text));
            textCell.SetBinding(TextCell.DetailProperty, nameof(TextCell.Detail));
        }

        private TextCell[] GetCells<T>(T value)
        {
            var type = typeof(T);
            var resutl = type.GetProperties().Select(info =>
            new TextCell
            {
                Text = info.Name,
                Detail = type.GetProperty(info.Name).GetValue(value)?.ToString() ?? "無"
            });

            return resutl.ToArray();
        }
    }
}