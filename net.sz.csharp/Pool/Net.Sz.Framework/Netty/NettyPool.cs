using Net.Sz.Framework.Netty.Http;
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
    public class NettyPool
    {
        public static IIOSessionHandler SessionHandler { get; set; }
        public static IIOSessionHandler ClientSessionHandler { get; set; }

        public static System.Collections.Concurrent.ConcurrentDictionary<long, IOSession> Sessions { get; set; }

        static List<Netty.Tcp.NettyServer> Service = new List<Tcp.NettyServer>();
        static List<Netty.Http.HttpServer> httpService = new List<Netty.Http.HttpServer>();

        static Dictionary<string, HttpServer> httpServers = new Dictionary<string, HttpServer>();

        static internal long MessageThread = 0;
        static internal long SessionThread = 0;

        /// <summary>
        /// 消息的线程数
        /// </summary>
        /// <param name="count"></param>
        static public void InitMessageThread(int count = 10)
        {
            lock (typeof(NettyPool))
            {
                if (MessageThread == 0)
                {
                    MessageThread = Threading.ThreadPool.GetThreadModel("Netty Message Pool Thread", count);
                }
            }
        }

        static NettyPool()
        {
            Sessions = new System.Collections.Concurrent.ConcurrentDictionary<long, IOSession>();
            SessionThread = Threading.ThreadPool.GetThreadModel("Netty Session Pool Thread", 1);
            Threading.ThreadPool.AddTimerTask(SessionThread, new CheckIOSessionTimerTask());
        }

        static public void AddTcpBind(string ip, int port, int threadcount)
        {
            Netty.Tcp.NettyServer server = new Tcp.NettyServer(ip, port);
            Service.Add(server);
        }

        static public void AddHttpBind(string ip, int port, string url, IHttpHandler handler, int threadcount)
        {
            string ipkey = ip + ":" + port;
            HttpServer server;
            if (httpServers.ContainsKey(ipkey))
            {
                server = httpServers[ipkey];
            }
            else
            {
                server = new HttpServer(ip, port);
                httpServers[ipkey] = server;
            }
            server.AddHandler(url, handler, threadcount);
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
                catch (Exception) { }
            }

            //Dictionary<string, IOSession> tmp = new Dictionary<string, IOSession>(Sessions);
            //foreach (var item in tmp)
            //{
            //    item.Value.Close("停服维护");
            //}
        }



    }
}
