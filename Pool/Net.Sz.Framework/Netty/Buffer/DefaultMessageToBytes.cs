using System;
using System.Collections.Generic;
using System.IO;
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
    internal class DefaultMarshalEndian
    {
        //用于存储剩余未解析的字节数
        private List<byte> _LBuff = new List<byte>(2);

        //字节数常量一个消息id4个字节
        const long ConstLenght = 8L;
        const short ConstStart1 = 0xaa;
        const short ConstStart2 = 0xbb;

        public List<byte[]> Decoder(IOSession session, byte[] buff)
        {
            if (this._LBuff.Count > 0)
            {
                //拷贝之前遗留的字节
                this._LBuff.AddRange(buff);
                buff = this._LBuff.ToArray();
                this._LBuff = new List<byte>(2);
            }
            List<byte[]> list = new List<byte[]>();

            BufferReader buffers = new BufferReader(buff);

            try
            {
                byte[] _buff;
            Label_0073:

                //判断本次解析的字节是否满足常量字节数 
                if ((buffers.BaseStream.Length - buffers.BaseStream.Position) < ConstLenght)
                {
                    _buff = buffers.ReadBytes((int)(buffers.BaseStream.Length - buffers.BaseStream.Position));
                    this._LBuff.AddRange(_buff);
                }
                else
                {
                    short tmpStart1 = buffers.ReadInt16();
                    if (ConstStart1 == tmpStart1)//自定义头相同
                    {
                        short tmpStart2 = buffers.ReadInt16();
                        if (ConstStart2 == tmpStart2)//自定义头相同
                        {
                            long offset = buffers.ReadInt32();
                            //剩余字节数大于本次需要读取的字节数
                            if (offset <= (buffers.BaseStream.Length - buffers.BaseStream.Position))
                            {
                                _buff = buffers.ReadBytes((int)(offset));
                                list.Add(_buff);
                                goto Label_0073;
                            }
                            else
                            {
                                //剩余字节数刚好小于本次读取的字节数 存起来，等待接受剩余字节数一起解析
                                buffers.BaseStream.Seek(ConstLenght, SeekOrigin.Current);
                                _buff = buffers.ReadBytes((int)(buffers.BaseStream.Length - buffers.BaseStream.Position));
                                this._LBuff.AddRange(_buff);
                            }
                        }
                        else
                        {
                            //往前推三个字节
                            buffers.BaseStream.Seek(-3, SeekOrigin.Current);
                            goto Label_0073;
                        }
                    }
                    else
                    {
                        //往前推一个字节
                        buffers.BaseStream.Seek(-1, SeekOrigin.Current);
                        goto Label_0073;
                    }
                }
            }
            catch { }
            finally
            {
                buffers.Close();
                buffers.Dispose();
            }
            return list;
        }

        public void Encoder(IOSession session, byte[] msgBuffer, BufferWriter outBuf)
        {
            outBuf.Write(ConstStart1);
            outBuf.Write(ConstStart2);
            if (msgBuffer != null)
            {
                outBuf.Write((Int32)(msgBuffer.Length));
                outBuf.Write(msgBuffer);
            }
            else
            {
                outBuf.Write((Int32)0);
            }
        }
    }
}
