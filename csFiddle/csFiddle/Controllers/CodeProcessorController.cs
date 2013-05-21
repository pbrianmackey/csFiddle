using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using csFiddle.BL;
using csFiddle.MsBuildLauncher;

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
        public string Post([FromBody]string codeToProcess)
        {
            var cc = new CodeGenerator();
            AppdomainHelper newAppDomain = new AppdomainHelper(System.Web.HttpContext.Current.Request.ApplicationPath);

            string rootprojectname = cc.GenerateCodeFile(codeToProcess);
            var projectPathAndFileName = Path.Combine(rootprojectname, CodeTemplateProjectInfo.PROJECTNAME);
            string output = newAppDomain.LoadIntoAppDomain(projectPathAndFileName, rootprojectname);
            
            return output;
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
