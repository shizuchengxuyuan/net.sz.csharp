//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

///**
// * 
// * @author 失足程序员
// * @Blog http://www.cnblogs.com/ty408/
// * @mail 492794628@qq.com
// * @phone 13882122019
// * 
// */
//namespace Sz.Network.SocketPool
//{
//    public class MarshalEndian : IMarshalEndian
//    {

//        public enum JavaOrNet
//        {
//            Java,
//            Net,
//        }

//        public MarshalEndian()
//        {

//        }

//        public static JavaOrNet JN = JavaOrNet.Net;

//        /// <summary>
//        /// 读取大端序的int
//        /// </summary>
//        /// <param name="value"></param>
//        public int ReadInt(byte[] intbytes)
//        {
//            Array.Reverse(intbytes);
//            return BitConverter.ToInt32(intbytes, 0);
//        }

//        /// <summary>
//        /// 写入大端序的int
//        /// </summary>
//        /// <param name="value"></param>
//        public byte[] WriterInt(int value)
//        {
//            byte[] bs = BitConverter.GetBytes(value);
//            Array.Reverse(bs);
//            return bs;
//        }

//        //用于存储剩余未解析的字节数
//        private List<byte> _LBuff = new List<byte>(2);

//        //字节数常量一个消息id4个字节
//        const long ConstLenght = 4L;

//        public void Dispose()
//        {
//            this.Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool flag1)
//        {
//            if (flag1)
//            {
//                IDisposable disposable = this._LBuff as IDisposable;
//                if (disposable != null) { disposable.Dispose(); }
//            }
//        }

//        public byte[] Encoder(SocketMessage msg)
//        {
//            MemoryStream ms = new MemoryStream();
//            BinaryWriter bw = new BinaryWriter(ms, UTF8Encoding.Default);
//            byte[] msgBuffer = msg.MsgBuffer;

//            if (msgBuffer != null)
//            {
//                switch (JN)
//                {
//                    case JavaOrNet.Java:
//                        bw.Write(WriterInt(msgBuffer.Length + 4));
//                        bw.Write(WriterInt(msg.MsgID));
//                        break;
//                    case JavaOrNet.Net:
//                        bw.Write((Int32)(msgBuffer.Length + 4));
//                        bw.Write(msg.MsgID);
//                        break;
//                }

//                bw.Write(msgBuffer);
//            }
//            else
//            {
//                switch (JN)
//                {
//                    case JavaOrNet.Java:
//                        bw.Write(WriterInt(0));
//                        break;
//                    case JavaOrNet.Net:
//                        bw.Write((Int32)0);
//                        break;
//                }
//            }
//            bw.Close();
//            ms.Close();
//            bw.Dispose();
//            ms.Dispose();
//            return ms.ToArray();
//        }

//        public List<SocketMessage> Decoder(byte[] buff, int len)
//        {
//            //拷贝本次的有效字节
//            byte[] _b = new byte[len];
//            Array.Copy(buff, 0, _b, 0, _b.Length);
//            buff = _b;
//            if (this._LBuff.Count > 0)
//            {
//                //拷贝之前遗留的字节
//                this._LBuff.AddRange(_b);
//                buff = this._LBuff.ToArray();
//                this._LBuff.Clear();
//                this._LBuff = new List<byte>(2);
//            }
//            List<SocketMessage> list = new List<SocketMessage>();
//            MemoryStream ms = new MemoryStream(buff);
//            BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
//            try
//            {
//                byte[] _buff;
//            Label_0073:
//                //判断本次解析的字节是否满足常量字节数 
//                if ((buffers.BaseStream.Length - buffers.BaseStream.Position) < ConstLenght)
//                {
//                    _buff = buffers.ReadBytes((int)(buffers.BaseStream.Length - buffers.BaseStream.Position));
//                    this._LBuff.AddRange(_buff);
//                }
//                else
//                {
//                    long offset = 0;
//                    switch (JN)
//                    {
//                        case JavaOrNet.Java:
//                            offset = ReadInt(buffers.ReadBytes(4));
//                            break;
//                        case JavaOrNet.Net:
//                            offset = buffers.ReadInt32();
//                            break;
//                    }

//                    //剩余字节数大于本次需要读取的字节数
//                    if (offset <= (buffers.BaseStream.Length - buffers.BaseStream.Position))
//                    {
//                        int msgID = 0;
//                        switch (JN)
//                        {
//                            case JavaOrNet.Java:
//                                msgID = ReadInt(buffers.ReadBytes(4));
//                                break;
//                            case JavaOrNet.Net:
//                                msgID = buffers.ReadInt32();
//                                break;
//                        }
//                        _buff = buffers.ReadBytes((int)(offset - 4));
//                        list.Add(new SocketMessage(msgID, _buff));
//                        goto Label_0073;
//                    }
//                    else
//                    {
//                        //剩余字节数刚好小于本次读取的字节数 存起来，等待接受剩余字节数一起解析
//                        buffers.BaseStream.Seek(ConstLenght, SeekOrigin.Current);
//                        _buff = buffers.ReadBytes((int)(buffers.BaseStream.Length - buffers.BaseStream.Position));
//                        this._LBuff.AddRange(_buff);
//                    }
//                }
//            }
//            catch { }
//            finally
//            {
//                buffers.Close();
//                if (buffers != null) { buffers.Dispose(); }
//                ms.Close();
//                if (ms != null) { ms.Dispose(); }
//            }
//            return list;
//        }
//    }
//}
