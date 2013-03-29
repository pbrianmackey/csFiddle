using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csFiddle.MsBuildLauncher
{
    public static class CodeTemplateProjectInfo
    {
        public const string MSBUILDPROJECTPATH = @"C:\Users\Brian\Documents\GitHub\csFiddle\csFiddle\csFiddle.MsBuildLauncher\bin\Debug\csFiddle.MsBuildLauncher.exe";//TODO: Make this a relative path
        public const string THISPROJECTTEMPPATH = @"C:\Users\Brian\Documents\GitHub\csFiddle\csFiddle\csFiddle\temp";//TODO: Make this a relative path
        public const string PROJECTPATH = @"C:\Users\Brian\Documents\GitHub\csFiddle\csFiddle\ProjectTemplate";//TODO: Make this a relative path from this directory
        public const string PROJECTNAME = @"ProjectTemplate.csproj";
        public const string BLASSEMBLYLOCATION = @"C:\Users\Brian\Documents\GitHub\csFiddle\csFiddle\csFiddle.CustomCompiler\bin\Debug\csFiddle.CustomCompiler.dll";//TODO: Make this a relative path
        public static string BLASSEMBLYLOCATIONPATHONLY { get { return Path.GetDirectoryName(BLASSEMBLYLOCATION); } }
    }
}
