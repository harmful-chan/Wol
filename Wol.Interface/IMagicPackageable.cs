using System;
using System.Collections.Generic;
using System.Text;

namespace Wol.Interface
{
    public interface IMagicPackageable
    {
        void FillHeader();
        void FillMacAddress();
        byte[] GetPageage();
        
    }
}
