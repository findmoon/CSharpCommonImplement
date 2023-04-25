using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRBasic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoTestController : ControllerBase
    {
        /// <summary>
        ///  Polling 用于轮询测试
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public IActionResult ResetCount()
        {
            _count = 0;
            return Ok();
        }

        #region Polling
        /// <summary>
        ///  Polling 用于轮询测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public IActionResult PollingTest_GetCount(int id)
        {
            var count = GetLastedCount();
            if (count > 10)
            {
                return Ok(new { id, count, finished = true });
            }

            if (count > 8)
            {
                return NoContent();
                //return NotFound(); // 404
            }

            if (count > 3)
            {
                return Ok(new { id, count });
            }
            return NoContent();  // 404
            //return NotFound();  // 404
        }
        #endregion

        #region Long Polling
        /// <summary>
        /// 长轮询测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public IActionResult LongPollingTest_GetCount(int id)
        {
            // 模式等待数据
            Thread.Sleep(1000 * new Random().Next(1,300*5) / 2);

            int count = GetLastedCount();
            if (count > 5)
            {
                return Ok(new { id, count, finished = true });
            }
            else
            {
                return Ok(new { id, count, finished = false });
            }
        }
        #endregion

        private static int _count;
        public int GetLastedCount()
        {
            _count++;
            return _count;
        }

        // ------------------------------------------
        // ------------------------------------------
        // ------------------------------------------

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
