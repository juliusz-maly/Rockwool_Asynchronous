using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class SynchronousApiController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return "";
        }

        // GET api/<controller>/1?time=2000
        public string Get([FromUri] int time, int id = 0)
        {
            var result = InnerMethod(time, id);

            return $"Action {id} run synchronously !";
        }

        private int InnerMethod(int time, int id)
        {
            Task.Delay(time).Wait();
            //Task.Run(() => Task.Delay(time).Wait());

            return id++;
        }
    }
}