using CarFinder_Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CarFinder_Xamarin
{
    public partial class App : Application
    {
        private MainPage mp = new MainPage();

        public App()
        {
            InitializeComponent();

            //if (Device.RuntimePlatform == Device.iOS)
            //    MainPage = new MainPage();
            //else
                MainPage = new NavigationPage(mp);
        }

        protected async override void OnStart()
        {
            //this handles when your app starts
            Picker years = mp.Content.FindByName<Picker>("pickerYears");
            mp.BackgroundImage = "alright_alright_alright.jpg";

            CarService svcCar = new CarService();

            List<string> yrs = await svcCar.GetYears();
            foreach (string y in yrs)
            {
                if (y != null)
                {
                    years.Items.Add(y);
                }
            }
        }
    }
}