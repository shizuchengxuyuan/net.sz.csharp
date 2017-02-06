using Net.Sz.Framework.Log;
using System;
using System.Collections.Generic;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Netty.Tcp
{
    /// <summary>
    /// 
    /// </summary>
    internal class NettyServer
    {
        private System.Net.IPEndPoint _IP;
        private System.Net.Sockets.Socket _Listeners;
        private Int64 MessageThreadID = 0;


        /// <summary>
        /// 初始化服务器
        /// </summary>
        public NettyServer(string ip = "0.0.0.0", int port = 9527, int threadcount = 1)
        {
            System.Net.IPEndPoint localEP = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), port);
            this._IP = localEP;
            try
            {
                Logger.Info(string.Format("Start Listen Tcp Socket -> {0}:{1} ", ip, port));
                this._Listeners = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                this._Listeners.Bind(this._IP);
                this._Listeners.Listen(5000);

                MessageThreadID = Framework.Threading.ThreadPool.GetThreadModel("Netty Session Pool Thread：" + ip + ":" + port, threadcount);

                System.Net.Sockets.SocketAsyncEventArgs sea = new System.Net.Sockets.SocketAsyncEventArgs();
                sea.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(this.AcceptAsync_Async);
                this.AcceptAsync(sea);
            }
            catch (Exception ex)
            {
                Logger.Error("Start Listen Tcp Socket Exception", ex);
                this.Dispose();
            }
        }

        private void AcceptAsync(System.Net.Sockets.SocketAsyncEventArgs sae)
        {
            try
            {
                if (!this._Listeners.AcceptAsync(sae))
                {
                    AcceptAsync_Async(this, sae);
                }
            }
            catch { }
        }

        private void AcceptAsync_Async(object sender, System.Net.Sockets.SocketAsyncEventArgs sae)
        {
            if (sae.SocketError == System.Net.Sockets.SocketError.Success)
            {
                try
                {
                    var socket = new NettyClient(sae.AcceptSocket);
                    NettyPool.Sessions.TryAdd(socket.ID, socket);
                    Logger.Info("Create Tcp Socket Remote Socket LocalEndPoint：" + socket.LocalEndPoint.ToString() + " RemoteEndPoint：" + socket.RemoteEndPoint.ToString());
                }
                catch { }
            }
            sae.AcceptSocket = null;
            AcceptAsync(sae);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        /// <param name="flag1"></param>
        protected virtual void Dispose([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U1)] bool flag1)
        {
            if (flag1)
            {
                if (_Listeners != null)
                {
                    try
                    {
                        Logger.Info(string.Format("Stop Listener Tcp -> {0}:{1} ", this.IP.Address.ToString(), this.IP.Port));
                        _Listeners.Close();
                        _Listeners.Dispose();
                    }
                    catch { }
                    _Listeners = null;
                }
            }
        }

        /// <summary>
        /// 获取绑定终结点
        /// </summary>
        public System.Net.IPEndPoint IP { get { return this._IP; } }
    }
}
