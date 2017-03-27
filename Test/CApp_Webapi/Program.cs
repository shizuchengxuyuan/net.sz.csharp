using Net.Sz.Framework.Netty;
using Net.Sz.Framework.Netty.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CApp_Webapi
{
    class Program
    {
        static void Main(string[] args)
        {

            HttpServer httpserver = NettyPool.AddHttpBind("127.0.0.1", 9527, 2);

            httpserver.AddHandler("*", new ActionHttpHandler((session) =>
            {
                session.AddContent("<html><body>webapi！</body></html>");
            }));

            httpserver.AddHandler("login", new ActionHttpHandler((session) =>
            {
                session.AddContent("<html><body>login holle ！</body></html>");
            }));

            httpserver.Start();

        }
    }
}
