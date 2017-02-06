using Net.Sz.Framework.Netty.Http;
using System;
namespace Net.Sz.Framework.Netty.Http
{

    /// <summary>
    /// http监听处理
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class HttpHandler_Action : IHttpHandler
    {

        private Action<HttpSession> ARun;

        public HttpHandler_Action(Action<HttpSession> run)
        {
            this.ARun = run;
        }

        /// <summary>
        /// 并发处理的
        /// </summary>
        /// <param name="session">连接对象</param>
        public void Run(HttpSession session)
        {
            if (this.ARun == null)
            {
                return;
            }
            this.ARun(session);
        }

    }
}