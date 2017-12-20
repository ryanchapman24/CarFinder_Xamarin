using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFinder_Xamarin.Models
{
    public class CarRecall
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public List<RecallResults> Results { get; set; }
    }

    public class RecallResults
    {
        public string Manufacturer { get; set; }
        public string NHTSACampaignNumber { get; set; }
        public string ReportReceivedDate { get; set; }
        public string Component { get; set; }
        public string Summary { get; set; }
        public string Conequence { get; set; }
        public string Remedy { get; set; }
        public string Notes { get; set; }
        public string ModelYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
