using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Wol.Interface;

namespace Wol.Test
{
    [TestClass]
    public class MagicPackageTest
    {
        private IMagicPackageable t;

        public MagicPackageTest()
        {
            this.t = new WakeUp() { IP = "127.0.0.1", MAC = "12ab34cd56ef" };

        }

        [TestMethod]
        public void TestGetPageage()
        {
            Assert.IsTrue(t.GetPageage().Length == 108);
        }

        [TestMethod]
        public void TestFillHeader()
        {
            t.FillHeader();
            byte[] dst = new byte[6];
            Array.ConstrainedCopy(t.GetPageage(), 0, dst, 0, 6);
            Assert.AreEqual(new byte[]{ 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, dst);
        }

        [TestMethod]
        public void TestFillMacAddress(string macAddress)
        {
            t.FillMacAddress();
            byte[] dst = new byte[6];
            
            Array.ConstrainedCopy(t.GetPageage(), 6, dst, 0, 6);
            Assert.AreEqual(new byte[] { 0x12, 0xab, 0x34, 0xcd, 0x56, 0xef }, dst);
            Array.ConstrainedCopy(t.GetPageage(), 102, dst, 0, 6);
            Assert.AreEqual(new byte[] { 0x12, 0xab, 0x34, 0xcd, 0x56, 0xef }, dst);
        }
    }
}
