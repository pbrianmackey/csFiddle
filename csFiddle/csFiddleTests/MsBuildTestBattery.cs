using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csFiddle.BL;

namespace csFiddleTests
{
    [TestClass]
    public class MsBuildTestBattery
    {
        [TestMethod]
        public void BasicRun()
        {
            MSBuildHelper.Execute();
        }
    }
}
