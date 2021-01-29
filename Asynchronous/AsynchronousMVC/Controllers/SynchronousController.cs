using AsynchronousMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AsynchronousMVC.Controllers
{
    public class SynchronousController : Controller
    {
        // GET: Asynchronous
        public async Task<ActionResult> Index()
        {
            var synchronousApiUri = "http://localhost:51075/api/AsynchronousApi/?time=2000";
            var resultList = new List<string>();

            var result = await GetResponseAsync(synchronousApiUri);
            resultList.Add(result);

            return View("Asynchronous", new CommonModel { RequestResults = resultList });

            return View("Asynchronous");
        }

        // GET: Synchronous
        //public ActionResult Index()
        //{
        //    var synchronousApiUri = "http://localhost:51075/api/SynchronousApi/?time=2000";
        //    var resultList = new List<string>();

        //    var result = GetResponse(synchronousApiUri);
        //    resultList.Add(result);

        //    return View("Synchronous", new CommonModel { RequestResults = resultList });
        //}

        public static string GetResponse(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public async static Task<string> GetResponseAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync()))
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}