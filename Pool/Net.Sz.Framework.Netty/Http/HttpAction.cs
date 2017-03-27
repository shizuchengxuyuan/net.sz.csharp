using Net.Sz.Framework.Szlog;
using Net.Sz.Framework.SzThreading;
using System;
using System.Collections.Generic;
using System.Text;

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
    internal class HttpAction : TaskModel
    {
        private static SzLogger log = SzLogger.getLogger();

        public HttpSession session;
        public HttpServer httpServer;


        public override void Run()
        {
            session._Action();
            if (!"favicon.ico".Equals(session.Http_Url))
            {
                if (log.IsDebugEnabled()) log.Debug("Create Http Socket Remote Socket LocalEndPoint：" + session.LocalEndPoint + " RemoteEndPoint：" + session.RemoteEndPoint.ToString());
                HttpActionBean httpbean = null;
                if (!httpServer.httpHandlers.TryGetValue(session.Http_Url, out httpbean))
                {
                    httpServer.httpHandlers.TryGetValue("*", out httpbean);
                }
                if (httpbean != null)
                {
                    HttpActionRun taskmodel = new HttpActionRun(session, httpbean.ihttpHandler);
                    ThreadPool.AddTask(httpbean.threadId, taskmodel);
                    return;
                }
                if (log.IsErrorEnabled()) log.Error("未找到监听状态:" + session.Http_Url, new Exception("未找到监听状态:" + session.Http_Url));
            }
            session.Close();
        }

    }
}
