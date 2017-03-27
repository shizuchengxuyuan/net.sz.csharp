using Net.Sz.Framework.Netty.Http;
using System;

/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */

namespace Net.Sz.Framework.Netty.Http
{

    /// <summary>
    /// http监听处理
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class ActionHttpHandler : IHttpHandler
    {

        private Action<HttpSession> ARun;

        public ActionHttpHandler(Action<HttpSession> run)
        {
            this.ARun = run;
        }

        /// <summary>
        /// 并发处理的
        /// </summary>
        /// <param name="session">连接对象</param>
        public void Run(HttpSession session)
        {
            this.ARun(session);
        }

    }
}