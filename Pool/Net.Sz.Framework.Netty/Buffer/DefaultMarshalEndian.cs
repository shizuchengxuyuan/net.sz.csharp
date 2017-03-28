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
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class DefaultMarshalEndian
    {
        public DefaultMarshalEndian()
        {
        }

        /// <summary>
        /// 读取一个int
        /// </summary>
        /// <param name="value"></param>
        public virtual int ReadInt(byte[] intbytes)
        {
            /*需要的话放开注释就好*/
            //Array.Reverse(intbytes);
            return BitConverter.ToInt32(intbytes, 0);
        }

        /// <summary>
        /// 书写一个int
        /// </summary>
        /// <param name="value"></param>
        public virtual byte[] WriterInt(int value)
        {
            byte[] bs = BitConverter.GetBytes(value);
            /*需要的话放开注释就好*/
            //Array.Reverse(bs);
            return bs;
        }

        /// <summary>
        /// 用于存储剩余未解析的字节数
        /// </summary>
        private List<byte> _LBuff = new List<byte>(2);

        /// <summary>
        /// 字节数常量一个消息id4个字节
        /// </summary>
        const long ConstLenght = 8L;

        public virtual List<MessageBean> Decoder(IOSession session, byte[] buff)
        {
            if (this._LBuff.Count > 0)
            {
                //拷贝之前遗留的字节
                this._LBuff.AddRange(buff);
                buff = this._LBuff.ToArray();
                this._LBuff = new List<byte>(2);
            }
            List<MessageBean> list = new List<MessageBean>();

            BufferReader buffers = new BufferReader(buff);

            try
            {
                byte[] _buff;

            Label_0073:

                /*判断本次解析的字节是否满足常量字节数 */
                if (buffers.BaseStream.Length - buffers.BaseStream.Position > ConstLenght)
                {
                    /*去掉包头判断，直接验证报文，物联网不能这样写，改为这样是为了和java游戏代码一致*/
                    int msgLenght = ReadInt(buffers.ReadBytes(4));
                    if (msgLenght <= (buffers.BaseStream.Length - buffers.BaseStream.Position))
                    {
                        int msgId = ReadInt(buffers.ReadBytes(4));
                        /*扣除消息Id 4个字节*/
                        _buff = buffers.ReadBytes(msgLenght - 4);
                        list.Add(new MessageBean() { MsgId = msgId, MsgBuff = _buff, Session = session });
                        goto Label_0073;
                    }
                    else
                    {
                        //剩余字节数刚好小于本次读取的字节数 存起来，等待接受剩余字节数一起解析
                        buffers.BaseStream.Seek(-4, SeekOrigin.Current);
                    }
                }

                /*如果有字符，那么全部缓存*/
                if ((buffers.BaseStream.Length - buffers.BaseStream.Position) > 0)
                {
                    _buff = buffers.ReadBytes((int)(buffers.BaseStream.Length - buffers.BaseStream.Position));
                    this._LBuff.AddRange(_buff);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="msgBuffer">消息完整报文</param>
        /// <param name="outBuf"></param>
        public virtual void Encoder(IOSession session, MessageBean bean, BufferWriter outBuf)
        {
            Encoder(session, bean.MsgId, bean.MsgBuff, outBuf);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="msgId">和java同步，消息ID</param>
        /// <param name="msgBuffer">消息完整报文</param>
        /// <param name="outBuf"></param>
        public virtual void Encoder(IOSession session, int msgId, byte[] msgBuffer, BufferWriter outBuf)
        {
            ///*包头*/
            //outBuf.Write(ConstStart1);
            ///*包头*/
            //outBuf.Write(ConstStart2);
            /*改为和java同步的游戏版本处理方案*/
            if (msgBuffer != null || msgBuffer.Length > 0)
            {
                /*长度*/
                outBuf.Write(WriterInt(msgBuffer.Length + 4));
                outBuf.Write(WriterInt(msgId));
                outBuf.Write(msgBuffer);
            }
            else
            {
                /*长度*/
                outBuf.Write(WriterInt(4));
                outBuf.Write(WriterInt(msgId));
            }
            /*道理应该压入包尾的，一般无需*/
        }
    }
}
