using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASPNETWebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/Test
        [Route("api/[controller]/[action]")]
        [HttpGet]
        public IEnumerable<string> Test()
        {
            return new string[] { "test1", "test1" };
        }
        
        // GET api/values/Test/5
        [Route("api/[controller]/[action]/{id}")]
        [HttpGet]
        public string Test(int id)
        {
            return "test1";
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
