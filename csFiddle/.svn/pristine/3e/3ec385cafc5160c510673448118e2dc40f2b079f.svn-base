using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Win32;

namespace csFiddle.BL
{
    public static class RegistryPathLocator
    {        
        public static string GetValue(string pathToKey, string keyName)
        {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(pathToKey))
            {
                var result = registryKey.GetValue(keyName).ToString();
                return result;
            }
        }
    }
}