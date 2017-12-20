using Acr.UserDialogs;
using CarFinder_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarFinder_Xamarin.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecallsPage : ContentPage
    {
        public RecallsPage(string car, List<RecallResults> recallMsgList)
        {
            InitializeComponent();
            try
            {
                infoFor.Text += car;
                recallCount.Text += $"{recallMsgList.Count.ToString()} recalls";
                int counter = 1;
                foreach (var recallMsg in recallMsgList.Select(r => r.Summary))
                {
                    recallText.Text += $"{counter}. {recallMsg}";
                    counter++;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.AlertAsync("Unable to get recalls: " + ex.Message);
            }
        }
    }
}