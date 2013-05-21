using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace csFiddle.Interfaces
{
    public static class csFiddleExtensions
    {
        public static T CreateInstance<T>(this AppDomain appDomain, string assemblyNames) where T: new()
        {
            ObjectHandle handle = appDomain.CreateInstance("someassembly.dll", "someType");
            T a = (T)handle.Unwrap();
            return a;
        }
    }
}
