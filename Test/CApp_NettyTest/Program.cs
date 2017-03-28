using Net.Sz.Framework.Netty;
using Net.Sz.Framework.Netty.Http;
using Net.Sz.Game.MMOGame.GameMessages.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CApp_NettyTest
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ProtoGLSMessage.ReqUpdateServerInfoMessage glmessage = new ProtoGLSMessage.ReqUpdateServerInfoMessage();
            glmessage.Connects = 5;
            glmessage.ServerID = 1;
            glmessage.ZoneID = 1;
            byte[] buff = MessageHelper.MessageSerialize(glmessage);
            Console.WriteLine(buff.Length);

            HttpServer httpserver = NettyPool.AddHttpBind("127.0.0.1", 9527, 2);

            httpserver.AddHandler("*", new ActionHttpHandler((session) =>
            {
                
            }));

            httpserver.AddHandler("login", new ActionHttpHandler((session) =>
            {
                session.AddContent("<html><body>login holle ！</body></html>");
            }));

            httpserver.Start();

            string msg = HttpClient.SendUrl("http://127.0.0.1:9527", "GET");
            Console.WriteLine(msg);
            Console.ReadLine();
        }
    }
}
