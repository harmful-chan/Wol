using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Wol.Interface;

namespace Wol.Test
{
    [TestClass]
    public class TriggerTest
    {
        private ITriggerable t;

        public TriggerTest()
        {
            this.t = new WakeUp() { IP = "127.0.0.1", MAC = "12ab34cd56ef" };
        }
    }
}
