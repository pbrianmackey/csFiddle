using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace csFiddle.Controllers
{
    public class CodeProcessorController : ApiController
    {
        // GET api/codeprocessor
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/codeprocessor/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/codeprocessor
        public void Post([FromBody]string codeToProcess)
        {
            string uniqueFileName = Guid.NewGuid() + ".cs";
            string uniquePath = Path.Combine("\temp", uniqueFileName);
            using (FileStream fs = File.Create(uniquePath))
            using(var sw = new StreamWriter(fs))
            {
                sw.Write(codeToProcess);
            }
        }

        // PUT api/codeprocessor/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/codeprocessor/5
        public void Delete(int id)
        {
        }
    }
}
