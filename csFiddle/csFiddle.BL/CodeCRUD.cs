using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using csFiddle.MsBuildLauncher;

namespace csFiddle.BL
{
    public class CodeGenerator
    {
        private const string CODETOKEN = "//$$$BeginCode";
        private Guid uniqueProjectIdentifier;
        private string uniqueProjectPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">The code to insert into the template.  The template already defines Main()</param>
        /// <returns>The filename of the generated file</returns>
        public string GenerateCodeFile(string userCode)
        {
            uniqueProjectIdentifier = Guid.NewGuid();
            uniqueProjectPath = Path.Combine(CodeTemplateProjectInfo.THISPROJECTTEMPPATH, uniqueProjectIdentifier.ToString());
            FileSystemHelper.DirectoryCopy(CodeTemplateProjectInfo.PROJECTPATH, uniqueProjectPath, true);

            string fileName = Path.Combine(uniqueProjectPath, "Program.cs");
            //string updatedProgramCode = CreateFromTemplate(fileName, userCode);

            SaveCode(fileName, userCode);
            return uniqueProjectPath;
        }

        private void SaveCode(string fileName, string updatedProgramCode)
        {
            using(FileStream fs = File.Open(fileName,FileMode.Open))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write("");//clear contents
                sw.Write(updatedProgramCode);
            }
        }

        /// <summary>
        /// This can be used to quickly run code statements. Doesn't support classes and such, just statements.
        /// Have to consider namespace qualifications as well.  Perhaps import many using statements and what's leftover require fully qualified names.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        private string CreateFromTemplate(string fileName, string userCode)
        {
            using(FileStream fs = File.Open(fileName,FileMode.Open))
            using (var sr = new StreamReader(fs))
            {
                string templateCode = sr.ReadToEnd();

                int startIndex = templateCode.IndexOf(CODETOKEN);

                StringBuilder sb = new StringBuilder(templateCode);
                sb.Insert(startIndex, userCode);

                return sb.ToString();
            }                        
        }
    }
}