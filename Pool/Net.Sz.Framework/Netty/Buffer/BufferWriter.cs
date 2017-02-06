using System;
using System.Collections.Generic;
using System.Text;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Netty.Buffer
{
    public class BufferWriter : System.IO.BinaryWriter
    {
        public BufferWriter()
            : base(new System.IO.MemoryStream(), UTF8Encoding.Default)
        {
            
        }

        public byte[] GetBuffer()
        {
            return ((System.IO.MemoryStream)this.BaseStream).ToArray();
        }

    }
}
