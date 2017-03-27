using Net.Sz;
using Net.Sz.Framework.Szlog;
using Net.Sz.Framework.Netty;
using Net.Sz.Framework.Netty.Buffer;
using Net.Sz.Framework.Netty.Http;
using Net.Sz.Framework.Netty.Tcp;
using Net.Sz.Framework.SzThreading;
using Net.Sz.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CApp1
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {

        private static SzLogger log = SzLogger.getLogger();

        static NettyClient clientA;
        static NettyClient clientB;

        static SendTask sendtask;

        static void Main(string[] args)
        {
            
            NettyPool.SessionHandler = new Handler();
            NettyPool.ClientSessionHandler = new ClientHandler();

            NettyPool.AddTcpBind("127.0.0.1", 8001);

            clientA = new NettyClient(port: 8001);
            clientA.Connect();

            clientB = new NettyClient(port: 8001);
            clientB.Connect();

            System.Threading.Thread.Sleep(1000);

            log.Error("数量 " + NettyPool.Sessions.Count);

            sendtask = new SendTask();

            ThreadPool.AddGlobTimerTask(sendtask);

            Console.ReadLine();
        }

        class SendTask : TimerTaskModel
        {
            int msgId = 2;
            byte[] msg;
            List<IOSession> tmp;
            public SendTask()
                : base(300)/*间隔300毫秒一直执行的定时器任务*/
            {
                BufferWriter bw = new BufferWriter();
                bw.Write(SzExtensions.GetId());/*发送同步消息唯一id*/
                msg = bw.GetBuffer();
                tmp = new List<IOSession>(NettyPool.Sessions.Values);
            }

            public void remove()
            {
                lock (tmp)
                {
                    if (tmp.Count > 0)
                    {
                        /*临时处理*/
                        tmp.RemoveAt(0);
                        if (tmp.Count == 0)
                        {
                            this.Cancel = true;
                        }
                        else
                        {
                            /*自行考虑*/
                        }
                    }
                }
            }

            public override void Run()
            {
                lock (tmp)
                {
                    if (tmp.Count > 0)
                    {
                        /*没300毫秒发送一次消息*/
                        var session = tmp[0];
                        session.WriteAndFlush(msgId, msg);
                    }
                }
            }
        }

        class Handler : IIOSessionHandler
        {

            public void ChannelActive(IOSession session)
            {

            }

            public void ExceptionCaught(IOSession session, Exception ex)
            {

            }

            public void ChannelRead(IOSession session, int msgId, byte[] buffer)
            {
                BufferReader br = new BufferReader(buffer);
                if (msgId == 255)
                {
                    /*消息唯一回复id*/
                    long mmid = br.ReadInt64();

                    log.Error("收到客户端消息回复：" + session.ID + "  消息唯一回复id" + mmid);

                    Program.sendtask.remove();
                }
            }

            public void ChannelUnregistered(IOSession session, params string[] obj)
            {

            }

            public void ChannelInactive(IOSession session)
            {

            }
        }

        class ClientHandler : IIOSessionHandler
        {

            public void ChannelActive(IOSession session)
            {

            }

            public void ExceptionCaught(IOSession session, Exception ex)
            {

            }

            public void ChannelRead(IOSession session, int msgId, byte[] buffer)
            {
                /*收到服务器发送来的消息*/
                BufferReader br = new BufferReader(buffer);
                if (msgId == 2)
                {
                    /*消息唯一回复id*/
                    long mmid = br.ReadInt64();
                    /*收到消息回复消息处理*/
                    log.Error("收到服务器消息：" + session.ID + "  消息唯一回复id" + mmid);

                    BufferWriter bw = new BufferWriter();
                    bw.Write(mmid);
                    session.WriteAndFlush(255, bw.GetBuffer());
                }
            }

            public void ChannelUnregistered(IOSession session, params string[] obj)
            {

            }

            public void ChannelInactive(IOSession session)
            {
            }
        }
    }
}
