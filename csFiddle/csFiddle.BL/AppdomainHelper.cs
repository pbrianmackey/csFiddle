using System;
using System.Reflection;
using System.Text;
using csFiddle.Interfaces;
using csFiddle.MsBuildLauncher;

namespace csFiddle.BL
{
    /// <summary>
    /// For security purposes all the user code is executed inside of this restricted appdomain.
    /// </summary>
    public class AppdomainHelper
    {
        private AppDomain appDomain;
        private string _appBase;
        public AppdomainHelper(string appBase = null)
        {
            if (appBase == null)
            {
                appBase = Environment.CurrentDirectory;
            }
            _appBase = appBase;
            CreateAppdomain();
        }

        public void CreateAppdomain()
        {
            var appdomainSetup = new AppDomainSetup();
            //string appBase = Environment.CurrentDirectory;
            //string appBase = HttpContext.Current.Request.ApplicationPath;
            appdomainSetup.ApplicationBase = CodeTemplateProjectInfo.BLASSEMBLYLOCATIONPATHONLY;
            //System.Diagnostics.Debug.WriteLine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            appdomainSetup.DisallowBindingRedirects = false;
            appdomainSetup.DisallowCodeDownload = true;
            appdomainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            
            appDomain = AppDomain.CreateDomain("compilerAppdomain", null, appdomainSetup);
            appDomain.Load(AssemblyName.GetAssemblyName(CodeTemplateProjectInfo.BLASSEMBLYLOCATION));
        }

        public string LoadIntoAppDomain(string projectPath, string rootprojectname)
        {
            var sb = new StringBuilder();
            var compiler = appDomain.CreateInstanceAndUnwrap("csFiddle.CustomCompiler", "csFiddle.BL.MSBuildHelper") as MSBuildHelper;
            sb.Append(compiler.Build(projectPath));
            sb.Append(compiler.Execute(rootprojectname));

            return sb.ToString();
        }

        public void UnloadAppdomain()
        {
            AppDomain.Unload(appDomain);
        }
    }

    public class A
    {
        public string SomeField { get; set; }
    }

}