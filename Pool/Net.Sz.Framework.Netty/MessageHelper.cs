using Net.Sz.Framework.Szlog;
using Net.Sz.Framework.Netty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Netty
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageHelper
    {

        private static SzLogger log = SzLogger.getLogger();

        private class MessageBean
        {
            public int MsgId { get; set; }

            public long ThreadID { get; set; }

            public Type Handler { get; set; }

            public ProtoBuf.IExtensible MsgInstance { get; set; }

        }

        private static Dictionary<int, MessageBean> MessageHandlers = new Dictionary<int, MessageBean>();

        public static void RegisterMessage(int messageid, long threadid, Type handler, ProtoBuf.IExtensible msgInstance)
        {
            log.Debug("注册消息处理器 id: " + messageid + " type: " + handler.Name + " msgInstance：" + msgInstance.GetType().Name);

            MessageHandlers[messageid] = new MessageBean() { MsgId = messageid, ThreadID = threadid, Handler = handler, MsgInstance = msgInstance };

        }

        public static void ActionMessage(IOSession session, int messageid, byte[] buffer)
        {
            log.Debug("message id :" + messageid);
            //验证是否注册消息
            if (MessageHandlers.ContainsKey(messageid))
            {
                //获取消息体
                MessageBean ins = MessageHandlers[messageid];
                //
                object obj = Activator.CreateInstance(ins.Handler);
                //
                if (obj is TcpHandler)
                {
                    TcpHandler objhandler = (obj as TcpHandler);
                    if (objhandler != null)
                    {
                        objhandler.Session = session;
                        //拷贝字节数组
                        byte[] msgbuf = new byte[buffer.Length - 4];
                        for (int i = 0; i < msgbuf.Length; i++)
                        {
                            msgbuf[i] = buffer[i + 4];
                        }
                        //创建消息
                        objhandler.Message = MessageHelper.MessageDeserialize(msgbuf, ins.MsgInstance);
                        //将消息放入执行线程
                        Net.Sz.Framework.SzThreading.ThreadPool.AddTask(ins.ThreadID, objhandler);
                        return;
                    }
                }
            }
            log.Error("尚未注册的消息：" + messageid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        public static void SendMessage(IOSession session, int msgid, ProtoBuf.IExtensible message)
        {
            if (session != null)
            {
                session.WriteAndFlush(msgid, MessageSerialize(message));
            }
            else
            {
                log.Error("session：为空 发送消息 " + message.GetType().FullName);
            }
        }

        public static byte[] MessageSerialize(object message)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.NonGeneric.Serialize(ms, message);
                return ms.ToArray();
            }
        }

        public static object MessageDeserialize(byte[] msgbuffer, object instance)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(msgbuffer))
            {
                return ProtoBuf.Serializer.NonGeneric.Merge(ms, instance);
            }
        }
    }
}
