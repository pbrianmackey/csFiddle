using Roslyn.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Roslyn.Compilers.CSharp;
using Roslyn.Services.Host;
using Roslyn.Compilers;
using System.Diagnostics;
using csFiddle.BL;
using System.Web;

namespace csFiddle.Controllers
{
    public class CodeProcessorController : ApiController
    {
        // POST api/codeprocessor
        public string Post([FromBody]string codeToProcess)
        {
            return CompileWithRoslyn(codeToProcess);
            //CompileWithVisualStudioInstallation(codeToProcess);
        }

        private static string CompileWithRoslyn(string codeToProcess)
        {
            var output = "";
            try
            {
                output = "test";
                string webRoot = HttpContext.Current.Server.MapPath("~/");
                string tempDirectory = Path.Combine(webRoot, "TempProgs");
                //output = tempDirectory;//Environment.CurrentDirectory returns: C:\Windows\SysWOW64\inetsrv\TempProgs
                const string fileName = "MyCompilation3";
                string outputFileName = Path.Combine(Path.GetTempPath(), fileName + ".exe");
                string pdbFileName = Path.Combine(Path.GetTempPath(), fileName + ".pdb");
                output = codeToProcess;
                //var aType = Roslyn.Compilers.CSharp.TypeKind.Class;//Test call to load a roslyn dll
                var tree = SyntaxTree.ParseText(codeToProcess);

                MetadataReference mscorlib = MetadataReference.CreateAssemblyReference("mscorlib");

                var comp = Compilation.Create(
                    fileName,
                    syntaxTrees: new[] { tree },
                    references: new[] { mscorlib });

                EmitResult result = null;

                using (var ilStream = new FileStream(outputFileName, FileMode.OpenOrCreate))
                using (var pdbStream = new FileStream(pdbFileName, FileMode.OpenOrCreate))
                {
                    // Emit IL for the compiled program.
                    result = comp.Emit(
                        outputStream: ilStream,
                        pdbFileName: pdbFileName,
                        pdbStream: pdbStream);
                }


                if (result.Success)
                {
                    // Run the compiled program.
                    output = ProcessHelper.Start(outputFileName);
                }
                else
                {
                    foreach (var diag in result.Diagnostics)
                    {
                        //Console.WriteLine(diag.ToString());
                        output += diag; //Assume few concatenations, skipping out on StringBuilder
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }

            return output;
        }

        private void CompileWithVisualStudioInstallation(string codeToProcess)
        {
            string uniqueFileName = Guid.NewGuid() + ".cs";
            string uniquePath = Path.Combine("\temp", uniqueFileName);
            using (FileStream fs = File.Create(uniquePath))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(codeToProcess);
            }
        }
    }
}
