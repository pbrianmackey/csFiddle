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
            const string fileName = "MyCompilation3";
            string outputFileName = Path.Combine(Path.GetTempPath(), fileName + ".exe");
            string pdbFileName = Path.Combine(Path.GetTempPath(), fileName + ".pdb");
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

            var output = "";
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
                    output += diag;//Assume few concatenations, skipping out on StringBuilder
                }
            }

            return output;

            //ProjectId pid1;
            //DocumentId did1;
            //ISolution solution = Solution.Create(SolutionId.CreateNewId("Solution"))
            //                            .AddCSharpProject("Project1.dll", "Project1", out pid1)
            //                            .AddDocument(pid1, "A.cs", codeToProcess, out did1);

            //var workspaceServices = (IHaveWorkspaceServices)solution;
            //var projectDependencyService = workspaceServices.WorkspaceServices.GetService<IProjectDependencyService>();
            //var assemblies = new List<MemoryStream>();
            //foreach (var projectId in projectDependencyService.GetDependencyGraph(solution).GetTopologicallySortedProjects())
            //{
            //    var stream = new MemoryStream();
            //    {
            //        solution.GetProject(projectId).GetCompilation().Emit(stream);
            //        assemblies.Add(stream);
            //    }
            //    stream.Position = 0;
            //}

            ////Check whats in the stream
            //foreach (var s in assemblies)
            //{
            //    var resultOuter = System.Text.Encoding.ASCII.GetString(s.ToArray());//s is empty string
            //    using (var sr = new StreamReader(s))
            //    {
            //        string result = sr.ReadToEnd();

            //        System.Diagnostics.Trace.Write(result);
            //        s.Close();
            //    }
            //}
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
