using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csFiddle.MsBuildLauncher
{
    public static class RegistryPaths
    {
        internal const string DOTNETFOURPATH = @"Software\Microsoft\MSBuild\4.0";
        internal const string VSPATH = @"Software\Wow6432Node\Microsoft\VisualStudio\10.0\Setup\VC";//TODO: consider other paths
    }
}
