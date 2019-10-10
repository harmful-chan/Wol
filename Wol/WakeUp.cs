using System;
using System.Collections.Generic;
using System.Text;
using Wol.Interface;

namespace Wol
{
    public class WakeUp  : IMagicPackageable, ITriggerable, IUDPSenderable
    {
        private byte[] magicPackage;
        public string IP { get; set; }
        public string MAC { get; set; }

        public WakeUp()
        {
            this.magicPackage = new byte[108];
        }

        public void FillHeader()
        {
            for (int i = 0; i < 6; i++)
                this.magicPackage[i] = 0xFF;
        }
        public void FillMacAddress()
        {
            if (this.MAC.Length != 12) throw new ArgumentException();

            //unicode 转 ascll
            byte[] macBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII,
                Encoding.Unicode.GetBytes(this.MAC));

            //byte 转 HEX
            Array.ConvertAll<byte, byte>(macBytes, b => this.ASCIIToHex(b));

            //byte数组序列化
            byte[] macHexBytes = this.Serialize(macBytes);

            //填充物理地址
            for (int i = 6; i < 108; i += 6)
            {
                for (int j = 0; j < 6; j++)
                    magicPackage[i + j] = macBytes[j];
            }


        }

        public void Trigger()
        {
            this.FillHeader();
            this.FillMacAddress();
            this.SendUDPPackage(this.IP, 9, this.magicPackage);
        }

        public void SendUDPPackage(string ip, int port, byte[] package)
        {
            throw new NotImplementedException();
        }

        public byte[] GetPageage()
        {
            return this.magicPackage;
        }

        private byte ASCIIToHex(byte a)
        {
            if (a >= 97 && a <= 122)    //>a < z
            {
                return (byte)(a - 97 + 10);
            }
            else if (a >= 65 && a <= 89)    //>A < Z
            {
                return (byte)(a - 65 + 10);
            }
            else if (a >= 48 && a <= 57)    //>0 < 9
            {
                return (byte)(a - 48);
            }
            else throw new ArgumentException();
        }

        private byte[] Serialize(byte[] bytes)
        {
            byte[] ret = new byte[6];

            for (int i = 0; i < 6; i++)
            {
                ret[i] |= (byte)(bytes[2 * i] << 4);
                ret[i] |= (byte)(bytes[2 * i + 1]);
            }
            return ret;
        }

    }
}
