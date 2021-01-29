using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousConsoleCore
{
    class Exercise_ChangeToAsync
    {
        private const int WaitingTime = 2000;

        public async void Execute()
        {
            var result = await GetHttpResultAsync();

            await LoadingOtherDataAsync();


            Console.WriteLine(result);
            Console.ReadKey();
        }

        private async Task LoadingOtherDataAsync()
        {
            Console.WriteLine("Started loading other data");
            await Task.Delay(WaitingTime * 2);
            Console.WriteLine("Other data loaded");
        }

        private async Task<string> GetHttpResultAsync()
        {
            await Task.Delay(WaitingTime);
            var response = await GetResponseAsync("http://www.onet.pl");
            return "Operation completed";
        }

        private static async Task<string> GetResponseAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
