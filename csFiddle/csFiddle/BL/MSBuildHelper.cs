using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace csFiddle.BL
{
    public class MSBuildHelper
    {
        private const string vsCmdPrompt ="%comspec% /k \"C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\VC\\vcvarsall.bat\" x86";
        private const string dotNetFourPath = "Software\\Microsoft\\MSBuild\\4.0";
        private const string msBuildRegistryValue = "MSBuildOverrideTasksPath";        
        private const string simpleCmdPath = @"C:\Windows\SysWOW64\cmd.exe";
        public static void Execute()
        {
            ProcessStartInfo info = new ProcessStartInfo(simpleCmdPath);
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;

            Process p = new Process();
            p.StartInfo = info;
            p.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                        ; 
                    else 
                        Console.WriteLine(e.Data);
                };
            
            p.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                        ; 
                    else
                        Console.WriteLine(e.Data);
                };

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.StandardInput.WriteLine(vsCmdPrompt);
            p.StandardInput.WriteLine(GetMsBuildPath());
            p.StandardInput.Close();//Is this thread safe?  May need to setup ManualResetEvent or something
            p.WaitForExit();
        }

        public static string GetMsBuildPath()
        {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(dotNetFourPath))
            {
                var msBuildPath = registryKey.GetValue(msBuildRegistryValue).ToString();
                string result = Path.Combine(msBuildPath, "msbuild.exe");
                return result;
            }
        }
    }
}