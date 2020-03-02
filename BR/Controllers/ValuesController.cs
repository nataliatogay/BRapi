using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values


        private async Task HandleTimer(string key, Timer timer)
        {
            var k = key;
             Console.WriteLine(key);
            timer.Stop();
            //return;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var key = Guid.NewGuid().ToString();
            
            Timer timer = new Timer(1000);
            timer.Elapsed += async (sender, e) => await HandleTimer(key, timer);

         //   timer.Elapsed += OnTimedEvent;
            timer.Start();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
