using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csFiddle.BL;
using System.IO;

namespace csFiddleTests
{
    [TestClass]
    public class MsBuildTestBattery
    {
        [TestMethod]
        public void BasicRun()
        {
            string projectPath = Path.Combine(CodeTemplateProjectInfo.PROJECTPATH, CodeTemplateProjectInfo.PROJECTNAME);
            MSBuildHelper.Build(projectPath);
        }

        [TestMethod]
        public void RunGeneralTest()
        {
            string userCode = @"using System;
                                using System.Collections.Generic;
                                using System.Linq;
                                using System.Text;
                                using System.Threading.Tasks;

                                namespace ProjectTemplate
                                {
                                    class Program
                                    {
                                        static void Main(string[] args)
                                        {
                                            Console.WriteLine(""Hello World"");
                                        }
                                    }
                                }";

            var cc = new CodeGenerator();
            string rootprojectname = cc.GenerateCodeFile(userCode);
            var projectPathAndFileName = Path.Combine(rootprojectname, CodeTemplateProjectInfo.PROJECTNAME);
            MSBuildHelper.Build(projectPathAndFileName);
            System.Diagnostics.Debug.Write(MSBuildHelper.Execute(rootprojectname));
        }

        /// <summary>
        /// Not currently supported
        /// </summary>
        [TestMethod]
        public void RunCodeStatementTests()
        {
            string userCode = "Console.WriteLine(\"Hello Chuck!\");Console.ReadLine();";
            var cc = new CodeGenerator();
            string rootprojectname = cc.GenerateCodeFile(userCode);
            var projectPathAndFileName = Path.Combine(rootprojectname, CodeTemplateProjectInfo.PROJECTNAME);
            MSBuildHelper.Build(projectPathAndFileName);
            MSBuildHelper.Execute(rootprojectname);
        }
    }
}
