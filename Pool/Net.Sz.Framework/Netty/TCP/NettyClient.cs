using Net.Sz.Framework.Log;
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
    /// </summary>
    public class NettyClient : IOSession
    {

        internal bool isServer = false;

        /// <summary>
        /// 这个是服务器收到有效链接初始化
        /// </summary>
        /// <param name="socket"></param>
        internal NettyClient(Socket socket)
        {
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
            this.IP = ip;
            this.Port = port;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Connect()
        {
            try
            {
                Logger.Debug("Try Connect Tcp Socket Remote：" + IP + ":" + Port);
                this._Socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._Socket.Connect(IP, Port);
                Logger.Debug("Connect Tcp Socket Remote Socket RemoteEndPoint：" + this.RemoteEndPoint.ToString() + " LocalEndPoint：" + this.LocalEndPoint.ToString());
                this.SetSocket();
            }
            catch (SocketException se)
            {
                this.Close("Try Connect Tcp Socket Remote：" + IP + ":" + Port + " SocketException：" + se.ToString());
            }
            catch (Exception ex)
            {
                NettyPool.SessionHandler.ExceptionCaught(this, ex);
            }
        }
    }
}
