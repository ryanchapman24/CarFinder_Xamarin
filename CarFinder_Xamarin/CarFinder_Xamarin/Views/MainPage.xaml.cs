using CarFinder_Xamarin.Models;
using CarFinder_Xamarin.ViewModels;
using CarFinder_Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using static CarFinder_Xamarin.ViewModels.RecallSearch;

namespace CarFinder_Xamarin
{
    public partial class MainPage : ContentPage
    {     
        private CarService svcCar = new CarService();
        private ImageSearchViewModel imgCar = new ImageSearchViewModel();
        ImageResult car { get; set; }
        private string _searchForCar = string.Empty;
        List<ImageResult> _carList { get; set; }
        List<RecallResults> _recallList { get; set; }
        private string yearSelected;
        private string makeSelected;
        private string modelSelected;

        public MainPage()
        {
            InitializeComponent();
            //ScrollView scroll = new ScrollView();
            //Content = scroll;
            pickerYears.SelectedIndexChanged += SelectedIndexChanged;
            pickerMakes.SelectedIndexChanged += SelectedIndexChanged;
            pickerModels.SelectedIndexChanged += SelectedIndexChanged;

            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
        }

        private async void SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker pk = (Picker)sender;
            if (string.Compare(pk.Title, "Years") == 0)
            {
                Picker makes = Content.FindByName<Picker>("pickerMakes");
                List<string> mks = await svcCar.GetMakes(pk.Items[pk.SelectedIndex]);
                foreach (string m in mks)
                {
                    if (m != null)
                    {
                        makes.Items.Add(m);
                    }
                }
            }
            else if (string.Compare(pk.Title, "Makes") == 0)
            {
                Picker years = Content.FindByName<Picker>("pickerYears");
                Picker models = Content.FindByName<Picker>("pickerModels");
                List<string> mdls = await svcCar.GetModels(years.Items[years.SelectedIndex], pk.Items[pk.SelectedIndex]);
                var ic = models.Items.Count;
                if (ic > 0)
                {
                    for(int i = 0; i < ic; i++)
                    {
                        models.Items.RemoveAt(0);
                    }
                }

                if (mdls.Count == 1)
                {
                    models.Items.Add("");
                }
                else
                {
                    foreach (string m in mdls)
                    {
                        if (m != null)
                        {
                            models.Items.Add(m);
                        }
                    }
                }
            }
            else if (string.Compare(pk.Title, "Models") == 0)
            {
                Picker years = Content.FindByName<Picker>("pickerYears");
                Picker makes = Content.FindByName<Picker>("pickerMakes");
                Picker models = Content.FindByName<Picker>("pickerModels");
                ActivityIndicator busy = Content.FindByName<ActivityIndicator>("busyIndicator");

                yearSelected = years.Items[years.SelectedIndex];
                makeSelected = makes.Items[makes.SelectedIndex];
                modelSelected = models.Items[models.SelectedIndex];

                _searchForCar = "new " + yearSelected + " " + makeSelected + " " + modelSelected;

                busy.IsRunning = true;
                busy.IsVisible = true;

                List<ImageResult> result = await imgCar.SearchForImagesAsync(_searchForCar);
                _carList = result;
                car = result.FirstOrDefault();

                carLabel.Text = car.Title;
                carImage.Source = car.ImageLink;

                busy.IsRunning = false;
                busy.IsVisible = false;
            }

            pk.IsEnabled = false;
        }

        async void onRecallsClicked(object sender, EventArgs e)
        {
            RecallSearch rclCar = new RecallSearch(yearSelected, makeSelected, modelSelected);
            _recallList = rclCar.Results;

            await Navigation.PushAsync(new RecallsPage(car.Title, _recallList), true);
        }

        async void onRefreshClicked(object sender, EventArgs e)
        {
            InitializeComponent();

            pickerYears.SelectedIndexChanged += SelectedIndexChanged;
            pickerMakes.SelectedIndexChanged += SelectedIndexChanged;
            pickerModels.SelectedIndexChanged += SelectedIndexChanged;

            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            CarService svcCar = new CarService();
            List<string> yrs = await svcCar.GetYears();

            foreach (string y in yrs)
            {
                if (y != null)
                {
                    pickerYears.Items.Add(y);
                }
            }
        }
    }
}