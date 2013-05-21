using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;

namespace csFiddle.BL
{
    public class MSBuildHelper
    {
        private const string SIMPLECMDPATH = @"C:\windows\System32\cmd.exe";//hard coded value.  find a fix        

        public static string Build(string projectPathAndFileName)
        {
            string output = LaunchCommandLine(PathFinder.MsBuildPath + " " + projectPathAndFileName);
            return output;
        }

        public static string Execute(string rootProjectPath)
        {
            string executeablePath = Path.Combine(rootProjectPath, @"bin\Debug\ProjectTemplate.exe");
            string output = LaunchCommandLine(executeablePath);
            return output;
        }

        public static string LaunchCommandLine(string command)
        {
            StringBuilder output = new StringBuilder();

            var info = new ProcessStartInfo(SIMPLECMDPATH)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

            var p = new Process() {StartInfo = info};            
            p.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                        return; 
                    else 
                        output.AppendLine(e.Data);
                };
            
            p.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                        return;
                    else
                        output.AppendLine(e.Data);
                };

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.StandardInput.WriteLine(PathFinder.VcVarsallPath);//preload environment variables for visual studio
            p.StandardInput.WriteLine(command);
            p.StandardInput.Close();//Is this thread safe?  May need to setup ManualResetEvent or something
            p.WaitForExit();

            return output.ToString();
        }
    }
}