using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SynchronousApiController : ControllerBase
    {
        // GET api/<controller>
        [HttpGet()]
        public string Get()
        {
            return "";
        }

        // GET api/<controller>/1?time=2000
        [HttpGet("{id}")]
        public string Get([FromQuery] int time, int id = 0)
        {
            var result = InnerMethod(time, id);

            return $"Action {id} run synchronously !";
        }

        private int InnerMethod(int time, int id)
        {
            Task.Delay(time).Wait();
            //Task.Run(() => Task.Delay(time + 10000).Wait());

            return id++;
        }
    }
}