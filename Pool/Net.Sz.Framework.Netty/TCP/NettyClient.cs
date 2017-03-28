using Net.Sz.Framework.Szlog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;


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
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class NettyClient : IOSession
    {
        private static SzLogger log = SzLogger.getLogger();

        /// <summary>
        /// 这个是服务器收到有效链接初始化
        /// </summary>
        /// <param name="socket"></param>
        internal NettyClient(Socket socket)
        {
            this.ID = SzExtensions.GetId();
            isServer = true;
            this._Socket = socket;
            this.SetSocket();
        }

        /// <summary>
        /// 客户端主动请求服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public NettyClient(string ip = "127.0.0.1", int port = 9527)
        {
            if (NettyPool.ClientSessionHandler == null)
            {
                throw new Exception("NettyPool.SessionHandler 尚未初始化");
            }
            this.IP = ip;
            this.Port = port;
            this.ID = SzExtensions.GetId();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Connect()
        {
            if (NettyPool.ClientSessionHandler == null)
            {
                throw new Exception("NettyPool.SessionHandler 尚未初始化");
            }
            try
            {
                log.Debug("Try Connect Tcp Socket Remote：" + IP + ":" + Port);
                this._Socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._Socket.Connect(IP, Port);
                log.Debug("Connect Tcp Socket Remote Socket RemoteEndPoint：" + this.RemoteEndPoint.ToString() + " LocalEndPoint：" + this.LocalEndPoint.ToString());
                this.SetSocket();
            }
            catch (SocketException se)
            {
                this.Close("Try Connect Tcp Socket Remote：" + IP + ":" + Port + " SocketException：" + se.ToString());
            }
            catch (Exception ex)
            {
                NettyPool.ClientSessionHandler.ExceptionCaught(this, ex);
            }
        }
    }
}
