using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace csFiddle.BL
{
    public class ProcessHelper
    {
        public static string Start(string fileName)
        {
            string parms = @"";
            string output = "";
            string error = string.Empty;
            Process reg;

            var psi = new ProcessStartInfo(fileName, parms)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = false
                };


            reg = Process.Start(psi);

            using (StreamReader myOutput = reg.StandardOutput)
            {
                output = myOutput.ReadToEnd();
            }
            using (StreamReader myError = reg.StandardError)
            {
                error = myError.ReadToEnd();
            }

            string result = output;
            if(error != string.Empty)
                result += string.Format("\r\nError Info: {0}", error);
            return result;
        }
    }
}