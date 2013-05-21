using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace csFiddleTests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public void ReadFromRegistry()
        {
            const string dotNetFourPath = "Software\\Microsoft\\MSBuild\\4.0";
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(dotNetFourPath))
            {
                Console.WriteLine(registryKey.SubKeyCount);
                foreach (var VARIABLE in registryKey.GetSubKeyNames())
                {
                    Console.WriteLine(VARIABLE);
                }

                var result = registryKey.GetValue("MSBuildOverrideTasksPath").ToString();
                Console.WriteLine(result);
            }
        }
    }
}
