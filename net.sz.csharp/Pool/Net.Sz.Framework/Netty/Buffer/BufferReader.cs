using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// 
    /// </summary>
    public class BufferReader : System.IO.BinaryReader, IDisposable
    {

        public BufferReader(byte[] buffer)
            : base(new System.IO.MemoryStream(buffer), UTF8Encoding.Default)
        {
        }

        public byte[] GetBuffer()
        {
            return ((System.IO.MemoryStream)this.BaseStream).ToArray();
        }

    }
}
