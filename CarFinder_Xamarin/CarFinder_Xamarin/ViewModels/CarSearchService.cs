using Acr.UserDialogs;
using CarFinder_Xamarin.Models;
using CarFinder_Xamarin.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarFinder_Xamarin.ViewModels
{
    public class CarService
    {
        const string carUrl = "https://rchapman-carapi.azurewebsites.net/api/Cars/";
        HttpClient client = new HttpClient();

        public async Task<List<string>> GetYears()
        {
            var json = await client.GetStringAsync(carUrl + "Year");
            var returnData = JsonConvert.DeserializeObject<List<string>>(json);
            return returnData;
        }

        public async Task<List<string>> GetMakes(string year)
        {
            string _year = string.Format("?Year={0}", year);
            var json = await client.GetStringAsync(carUrl + "Make" + _year);
            var returnData = JsonConvert.DeserializeObject<List<string>>(json);
            return returnData;
        }

        public async Task<List<string>> GetModels(string year, string make)
        {
            string _year_make = string.Format("?Year={0}&Make={1}", year, make);
            //string nhtsaModels = string.Format("https://one.nhtsa.gov/webapi/api/Recalls/vehicle/modelyear/{0}/make/{1}?format=json", year, make);
            var json = await client.GetStringAsync(carUrl + "Model" + _year_make);
            var returnData = JsonConvert.DeserializeObject<List<string>>(json);
            return returnData;
        }

        public async Task<List<string>> GetTrims(string year, string make, string model)
        {
            string _year_make_model = string.Format("?Year={0}&Make={1}&Model={2}", year, make, model);
            //string nhtsaModels = string.Format("https://one.nhtsa.gov/webapi/api/Recalls/vehicle/modelyear/{0}/make/{1}?format=json", year, make);
            var json = await client.GetStringAsync(carUrl + "Trim" + _year_make_model);
            var returnData = JsonConvert.DeserializeObject<List<string>>(json);
            return returnData;
        }
    }

    public class RecallSearch
    {
        public List<RecallResults> Results { get; set; }

        public RecallSearch(string year, string make, string model)
        {
            Results = GetRecalls(year, make, model);
        }

        private List<RecallResults> GetRecalls(string year, string make, string model)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    const string carUrl = "https://mjaang-carapi.azurewebsites.net/api/Cars/";
                    string _year_make_model = string.Format("?Year={0}&Make={1}&Model={2}", year, make, model);
                    var result = client.GetAsync(carUrl + "CarData" + _year_make_model).ContinueWith((taskwithresponse) =>
                    {
                        var response = taskwithresponse.Result;
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        Results = JsonConvert.DeserializeObject<List<RecallResults>>(jsonString.Result);
                    });
                    result.Wait();
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.AlertAsync("Unable to get recalls: " + ex.Message);
                    return null;
                }
                return Results;
            }
        }

        //Image search API call
        public class ImageSearchViewModel
        {
            public List<ImageResult> Images { get; }
            public ImageSearchViewModel()
            {
                Images = new List<ImageResult>();
            }

            public async Task<List<ImageResult>> SearchForImagesAsync(string query)
            {
                var url = $"https://api.cognitive.microsoft.com/bing/v7.0/images/" +
                    $"search?q={WebUtility.UrlEncode(query)}" +
                    $"&count=20&offset=0&mkt=en-us&safeSearch=Strict";
                var requestHeaderKey = "Ocp-Apim-Subscription-Key";
                var requestHeaderValue = CognitiveServiceKeys.BingSearch;

                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add(requestHeaderKey, requestHeaderValue);
                        var json = await client.GetStringAsync(url);
                        var result = JsonConvert.DeserializeObject<SearchResults>(json);

                        Images.AddRange(result.Images.Select(i => new ImageResult
                        {
                            ContextLink = i.HostPageUrl,
                            FileFormat = i.EncodingFormat,
                            ImageLink = i.ContentUrl,
                            ThumbnailLink = i.ThumbnailUrl,
                            Title = i.Name
                        }));
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync("Unable to get recalls: " + ex.Message);
                    return null;
                }

                return Images;
            }
        }
    }
}
