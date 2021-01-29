using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class AsynchronousApiController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return "";
        }

        // GET api/<controller>/1?time=2000
        public async Task<string> Get([FromUri] int time, int id = 0)
        {
            var sc1 = SynchronizationContext.Current;

            var result = await InnerMethodAsync(time, id);
            
            var sc2 = SynchronizationContext.Current;

            return $"Action {id} run asynchronously !";
        }

        private async Task<int> InnerMethodAsync(int time, int id)
        {
            var sc1 = SynchronizationContext.Current;

            await Task.Delay(time).ConfigureAwait(false);

            var sc2 = SynchronizationContext.Current;

            return id++;
        }

        [Route("deadlock")]
        [HttpGet]
        public string GetDeadlock()
        {
            var sc1 = SynchronizationContext.Current;

            InnerMethod2Async().Wait();

            async Task InnerMethod2Async()
            {
                await Task.Delay(2000);
            }
            
            return $"Action run asynchronously !";
        }
    }
}