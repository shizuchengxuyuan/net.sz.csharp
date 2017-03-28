using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Sz.Framework.Netty.Buffer
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class MessageBean
    {
        public IOSession Session { internal set; get; }
        public Int32 MsgId { internal set; get; }
        public byte[] MsgBuff { internal set; get; }
    }
}
