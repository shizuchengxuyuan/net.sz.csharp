using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Net.Sz.Framework.Log;
using System.Collections.Concurrent;
using Net.Sz.Framework.Threading;


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
    internal class HttpServer
    {
        private IPEndPoint _IP;
        private TcpListener _Listeners;
        private volatile bool IsInit = false;

        private ConcurrentDictionary<string, IHttpHandler> httpHandlers = new ConcurrentDictionary<string, IHttpHandler>();
        private ConcurrentDictionary<string, long> httpHandlerThreadCounts = new ConcurrentDictionary<string, long>();


        public void AddHandler(string url, IHttpHandler handler, int threadcount)
        {
            url = url.ToLower();
            this.httpHandlers[url] = handler;
            long temp = 0;
            if (this.httpHandlerThreadCounts.ContainsKey(url))
            {
                temp = this.httpHandlerThreadCounts[url];
                ThreadPool.RemoveThreadModel(temp);
            }
            this.httpHandlerThreadCounts[url.ToLower()] = ThreadPool.GetThreadModel("http -> " + url, threadcount);
            Logger.Info(string.Format("Start Listen Http Socket -> {0}:{1}/{2} threadcount -> {3}", this._IP.Address.ToString(), this._IP.Port, url, threadcount));
        }


        /// <summary>
        /// 初始化服务器
        /// </summary>
        public HttpServer(string ip, int port)
        {
            this._IP = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                this._Listeners = new TcpListener(_IP);
                this._Listeners.Start(5000);
                IsInit = true;
                this.AcceptAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("初始化httpserver", ex);
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
                var socket = new HttpSession(client.Client);
                if (!"favicon.ico".Equals(socket.Http_Url))
                {
                    Logger.Debug("Create Http Socket Remote Socket LocalEndPoint：" + client.Client.LocalEndPoint + " RemoteEndPoint：" + client.Client.RemoteEndPoint.ToString());
                    foreach (var item in httpHandlers)
                    {
                        if (socket.Http_Url.Equals(item.Key))
                        {
                            HttpTaskModel taskmodel = new HttpTaskModel(socket, item.Value);
                            long threadid = this.httpHandlerThreadCounts[item.Key];
                            ThreadPool.AddTask(threadid, taskmodel);
                            //socket.process(item.Value);
                            return;
                        }
                    }
                    Logger.Error("未找到监听状态:" + socket.Http_Url, new Exception("未找到监听状态:" + socket.Http_Url));
                }
                socket.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("执行http", ex);
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
                        Logger.Info(string.Format("Stop Http Listener -> {0}:{1} ", this.IP.Address.ToString(), this.IP.Port));
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
