using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using csFiddle.MsBuildLauncher;

namespace csFiddle.BL
{



    public static class PathFinder
    {
        //private const string vsCmdPromptOriginal ="%comspec% /k \"\"C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\VC\\vcvarsall.bat\"\" x86";
        private const string VSCMDPROMPTSTART = "%comspec% /k \"\"";
        private const string VSCMDPROMPTEND = "\"\" x86";
        //private const string simpleCmdPath = @"%windir%\System32\cmd.exe";//TODO: Correct path.  I think this fails because UseShellExecute is false                        
        private const string MSBUILD = "msbuild.exe";        
        private const string VC = "vcvarsall.bat";        

        private static string msBuildPath;
        internal static string MsBuildPath
        {
            get
            {
                if (msBuildPath == null)
                {
                    msBuildPath = Path.Combine(RegistryPathLocator.GetValue(RegistryPaths.DOTNETFOURPATH, RegistryKeys.MSBUILDREGISTRYKEY), MSBUILD);
                }
                return msBuildPath;
            }
        }

        private static string vcVarsallPath;
        internal static string VcVarsallPath
        {
            get
            {
                if (vcVarsallPath == null)
                {
                    vcVarsallPath = VSCMDPROMPTSTART + Path.Combine(RegistryPathLocator.GetValue(RegistryPaths.VSPATH, RegistryKeys.VSKEY), VC) + VSCMDPROMPTEND;
                }
                return vcVarsallPath;
            }
        }
    }
}