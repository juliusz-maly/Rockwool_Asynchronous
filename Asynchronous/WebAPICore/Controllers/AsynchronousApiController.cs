using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsynchronousApiController : ControllerBase
    {
        // GET api/<controller>
        [HttpGet()]
        public string Get()
        {
            return "Asynchronous API in .NET Core";
        }

        // GET api/<controller>/1?time=2000
        [HttpGet("{id}")]
        public async Task<string> Get([FromQuery] int time, int id = 0)
        {
            var sc1 = SynchronizationContext.Current;

            var result = await InnerMethodAsync(time, id);
            
            var sc2 = SynchronizationContext.Current;

            return $"Action {id} run asynchronously !";
        }

        private async Task<int> InnerMethodAsync(int time, int id)
        {
            var sc1 = SynchronizationContext.Current;

            await Task.Delay(time);
            
            var sc2 = SynchronizationContext.Current;

            return id++;
        }

        [HttpGet("/deadlock")]
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