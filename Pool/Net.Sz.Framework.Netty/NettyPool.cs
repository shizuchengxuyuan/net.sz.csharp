using Net.Sz.Framework.Netty.Buffer;
using Net.Sz.Framework.Netty.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class NettyPool
    {
        /// <summary>
        /// NettyServer 服务器需要的处理逻辑
        /// </summary>
        public static IIOSessionHandler SessionHandler { get; set; }
        /// <summary>
        /// NettyClient 客户端需要的处理逻辑
        /// </summary>
        public static IIOSessionHandler ClientSessionHandler { get; set; }

        /// <summary>
        /// tcp需要的编码器，解码器
        /// </summary>
        public static Type MarshalEndianType = typeof(DefaultMarshalEndian);

        public static System.Collections.Concurrent.ConcurrentDictionary<long, IOSession> Sessions { get; set; }

        static List<Netty.Tcp.NettyServer> Service = new List<Tcp.NettyServer>();

        static Dictionary<string, HttpServer> httpServers = new Dictionary<string, HttpServer>();

        static internal long MessageThread = 0;
        static internal long SessionThread = 0;

        /// <summary>
        /// 消息的线程数
        /// </summary>
        /// <param name="count"></param>
        static public void InitMessageThread(int count = 2)
        {
            lock (typeof(NettyPool))
            {
                if (MessageThread == 0)
                {
                    MessageThread = SzThreading.ThreadPool.GetThreadModel("Netty Message Pool Thread", SzThreading.SzThread.ThreadType.Sys, count);
                }
            }
        }

        static NettyPool()
        {
            Sessions = new System.Collections.Concurrent.ConcurrentDictionary<long, IOSession>();
            SessionThread = SzThreading.ThreadPool.GetThreadModel("Netty Session Pool Thread", SzThreading.SzThread.ThreadType.Sys, 1);
            SzThreading.ThreadPool.AddTimerTask(SessionThread, new CheckIOSessionTimerTask());
        }

        static public void AddTcpBind(string ip, int port)
        {
            if (NettyPool.SessionHandler == null)
            {
                throw new Exception("NettyPool.SessionHandler 尚未初始化");
            }
            Netty.Tcp.NettyServer server = new Tcp.NettyServer(ip, port);
            Service.Add(server);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="threadcount">处理连接请求线程池</param>
        static public HttpServer AddHttpBind(string ip, int port, int threadcount = 1)
        {
            string ipkey = ip + ":" + port;
            HttpServer server;
            if (httpServers.ContainsKey(ipkey))
            {
                server = httpServers[ipkey];
            }
            else
            {
                server = new HttpServer(ip, port, threadcount);
                httpServers[ipkey] = server;
            }
            return server;
        }

        static public void Stop()
        {

            var temp = new List<Netty.Tcp.NettyServer>(Service);
            foreach (var item in temp)
            {
                try
                {
                    item.Dispose();
                }
                catch { }
            }
        }

    }
}
