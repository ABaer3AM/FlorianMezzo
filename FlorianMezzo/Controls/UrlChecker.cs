using FlorianMezzo.Constants;
using FlorianMezzo.Controls.db;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FlorianMezzo.Controls
{
    internal class UrlChecker
    {

        private static readonly HttpClient httpClient = new HttpClient();

        public UrlChecker()
        {
        }

        private async Task<Tuple<int, string>> FetchApiStatus(string apiUrl)
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                watch.Stop();

                if (response.IsSuccessStatusCode)
                {
                    //Debug.WriteLine($"API is reachable. Status Code: {response.StatusCode} [{watch.ElapsedMilliseconds:F0}ms]");
                    return Tuple.Create(1, $"[{watch.ElapsedMilliseconds:F0}ms]");
                }
                else
                {
                    //Debug.WriteLine($"API is not reachable. Status Code: {response.StatusCode} [{watch.ElapsedMilliseconds:F0}ms]");

                    if (response.ReasonPhrase != null)
                    {
                        if (response.ReasonPhrase.Length < 100){ return Tuple.Create(0, $"{response.ReasonPhrase} [{watch.ElapsedMilliseconds:F0}ms]"); }
                        else{
                            return Tuple.Create(0, $"{response.ReasonPhrase.Substring(0, 100)} [{watch.ElapsedMilliseconds:F0}ms]");
                        }
                    }else{
                        return Tuple.Create(0, $"NULL [{watch.ElapsedMilliseconds:F0}ms]");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking API connectivity: {ex.Message} [{watch.ElapsedMilliseconds:F0}ms]");
                return Tuple.Create(-1, ex.Message);
            }

        }

        public async Task<List<SoftDependencyData>> testSoftDependencies(string groupId)
        {
            Urls urlsObj = new();
            Tuple<string, string>[] coreDependencies = urlsObj.getCoreDependencies();
            Tuple<string, string>[] tileDependencies = urlsObj.getTiles();

            List<SoftDependencyData> softStatuses = [];

            // for every element in Constants.Urls.CoreDependencies...
            foreach (var dependency in coreDependencies)
            {
                //Debug.WriteLine("dependency: "+dependency.Item1 + " ,"+dependency.Item2);
                // test the url, build the state display, add it to the list, maybe update the main state
                Tuple<int, string> res = await FetchApiStatus(dependency.Item2);
                softStatuses.Add(new SoftDependencyData(groupId, dependency.Item1, res.Item1, res.Item2, false, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));     // digest status into a data-structure
            }
            foreach (var dependency in tileDependencies)
            {
                //Debug.WriteLine("dependency: " + dependency.Item1 + " ," + dependency.Item2);
                // test the url, build the state display, add it to the list, maybe update the main state
                Tuple<int, string> res = await FetchApiStatus(dependency.Item2);
                softStatuses.Add(new SoftDependencyData(groupId, dependency.Item1, res.Item1, res.Item2, true, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));     // digest status into a data-structure
            }


            return softStatuses;
        }
    }
}
