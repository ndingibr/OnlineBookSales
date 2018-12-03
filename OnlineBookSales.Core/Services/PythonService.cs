
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OnlineBookSales.Core
{
    public class PythonService : IPythonService
    {
        private string url = "";
        HttpClient client = new HttpClient();

        public PythonService(string url)
        {
            this.url = url;
        }

        public List<string> GetAttributes()
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("/getattributes", Method.GET);
                request.RequestFormat = DataFormat.Json;
                return client.Execute<List<string>>(request).Data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertPatrolData()
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("/getpatrolmodel", Method.POST);
                request.RequestFormat = DataFormat.Json;
                return client.Execute<int>(request).Data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<int> GetDailyPredictionAsync(Dictionary<string, string> predictionParameters)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("/predict", Method.POST);
                request.RequestFormat = DataFormat.Json;

                foreach (var pair in predictionParameters)
                {
                    request.AddParameter(pair.Key, pair.Value);
                }
                var returnValue = client.Execute<List<int>>(request).Data;
                return returnValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
