using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.Common.Invoke;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Web.Controllers
{        
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IInvokeHandler<ValuesController> _invokeHandler;

        public ValuesController(IInvokeHandler<ValuesController> invokeHandler)
        {
            _invokeHandler = invokeHandler;
            
            _invokeHandler.SetInvokeResultSettings(settings =>
                {
                    settings.InvokeFunctionLogSetttings.LogInvokeResult = LogInvokeResult.Always;
                });
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var res =new {
                name = "Stayrony",
                age = 24
            };
            
            await _invokeHandler.InvokeAsync(async () =>
            {
                await Task.Delay(10);
                
                return InvokeResult<object>.Ok();
            }, res);
        
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
