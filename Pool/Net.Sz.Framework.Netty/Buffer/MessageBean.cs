using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Sz.Framework.Netty.Buffer
{
    public class MessageBean
    {
        public IOSession Session { internal set; get; }
        public Int32 MsgId { internal set; get; }
        public byte[] MsgBuff { internal set; get; }
    }
}
