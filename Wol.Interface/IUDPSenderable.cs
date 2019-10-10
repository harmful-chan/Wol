using System;
using System.Collections.Generic;
using System.Text;

namespace Wol.Interface
{
    public interface IUDPSenderable
    {
        public void SendUDPPackage(string ip, int port, byte[] package);
    }
}
