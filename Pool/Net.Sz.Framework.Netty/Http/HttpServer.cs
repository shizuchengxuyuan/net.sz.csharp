using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Net.Sz.Framework.Szlog;
using System.Collections.Concurrent;
using Net.Sz.Framework.SzThreading;


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
    /// 
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class HttpServer
    {
        private static SzLogger log = SzLogger.getLogger();

        private IPEndPoint _IP;
        private TcpListener _Listeners;
        private volatile bool IsInit = false;
        private long ThreadId = 0;
        internal ConcurrentDictionary<string, HttpActionBean> httpHandlers = new ConcurrentDictionary<string, HttpActionBean>();

        /// <summary>
        /// 处理绑定，路由
        /// </summary>
        /// <param name="url">不区分大小写的路由</param>
        /// <param name="handler">处理函数</param>
        /// <param name="threadCount">如果需要并发处理，线程数量大于1，不能等于或小于0</param>
        public void AddHandler(string url, IHttpHandler handler, int threadCount = 1)
        {
            url = url.ToLower();

            if (!this.httpHandlers.ContainsKey(url))
            {
                long tId = ThreadPool.GetThreadModel("http -> " + url, SzThreading.SzThread.ThreadType.Sys, threadCount);
                this.httpHandlers[url] = new HttpActionBean() { ihttpHandler = handler, threadId = tId };
            }

            log.Info(string.Format("Http Listener -> {0}:{1}/{2} threadcount -> {3}", this._IP.Address.ToString(), this._IP.Port, url, threadCount));
        }


        /// <summary>
        /// 初始化服务器
        /// </summary>
        /// <param name="ip">绑定ip</param>
        /// <param name="port">绑定端口</param>
        /// <param name="threadCount">处理接入连接线程数量</param>
        internal HttpServer(string ip, int port, int threadCount = 1)
        {
            this._IP = new IPEndPoint(IPAddress.Parse(ip), port);
            this.ThreadId = ThreadPool.GetThreadModel(string.Format("Http Listener -> {0}:{1} ", this.IP.Address.ToString(), this.IP.Port), SzThreading.SzThread.ThreadType.Sys, threadCount);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            try
            {
                this._Listeners = new TcpListener(_IP);
                this._Listeners.Start(5000);
                this.IsInit = true;
                this.AcceptAsync();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled()) log.Error("初始化httpserver", ex);
                this.Dispose();
            }

        }

        private void AcceptAsync()
        {
            try
            {
                this._Listeners.BeginAcceptTcpClient(new AsyncCallback(AcceptAsync_Async), null);
            }
            catch (Exception) { }
        }

        private void AcceptAsync_Async(IAsyncResult iar)
        {
            this.AcceptAsync();
            try
            {
                TcpClient client = this._Listeners.EndAcceptTcpClient(iar);
                /*先添加到队列，让他缓冲数据块，执行是读取客户端传来的数据*/
                HttpSession socket = new HttpSession(client.Client);
                ThreadPool.AddTask(this.ThreadId, new HttpAction()
                {
                    httpServer = this,
                    session = socket
                });
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled()) log.Error("执行http", ex);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (IsInit)
            {
                IsInit = false;
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        /// <param name="flag1"></param>
        protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool flag1)
        {
            if (flag1)
            {
                if (_Listeners != null)
                {
                    try
                    {
                        if (log.IsInfoEnabled()) log.Info(string.Format("Stop Http Listener -> {0}:{1} ", this.IP.Address.ToString(), this.IP.Port));
                        _Listeners.Stop();
                        _Listeners = null;
                    }
                    catch { }
                    _Listeners = null;
                }
            }
        }

        /// <summary>
        /// 获取绑定终结点
        /// </summary>
        public IPEndPoint IP { get { return this._IP; } }
    }
}
