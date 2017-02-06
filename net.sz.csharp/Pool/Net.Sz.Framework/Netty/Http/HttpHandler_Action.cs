using Net.Sz.Framework.Netty.Http;
using System;
namespace Net.Sz.Framework.Netty.Http
{

    /// <summary>
    /// http��������
    /// <para>@author ʧ�����Ա</para>
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
        /// ���������
        /// </summary>
        /// <param name="session">���Ӷ���</param>
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