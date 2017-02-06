using Net.Sz.Framework.Log;
using Net.Sz.Framework.Netty.Buffer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Netty
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IOSession
    {

        /// <summary>
        /// 
        /// </summary>
        public long ID = 0;
        /// <summary>
        /// 可以用于临时变量
        /// </summary>
        public ObjectAttribute RunAttribute = new ObjectAttribute();
        //封装socket
        internal Socket _Socket;
        //回调
        internal AsyncCallback aCallback;
        //接受数据的缓冲区
        internal byte[] Buffers;
        //标识是否已经释放
        public volatile bool IsDispose = true;
        //4K的缓冲区空间
        internal const int BufferSize = 4 * 1024;
        //收取消息状态码
        internal SocketError ReceiveError;
        //发送消息的状态码
        internal SocketError SenderError;
        //每一次接受到的字节数
        internal int ReceiveSize = 0;
        //接受空消息次数
        internal byte ZeroCount = 0;
        public string IP;
        internal int Port = 0;
        internal long LastRecvTime = 0;
        public long CreateTime = 0;

        public bool Connected { get { return this._Socket.Connected; } }

        /// <summary>
        /// 消息编码器
        /// </summary>
        internal DefaultMarshalEndian mershaEncoder = new DefaultMarshalEndian();
        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public IPEndPoint RemoteEndPoint { get { return (IPEndPoint)_Socket.RemoteEndPoint; } }

        /// <summary>
        /// 获取本地终结点
        /// </summary>
        public IPEndPoint LocalEndPoint { get { return (IPEndPoint)_Socket.LocalEndPoint; } }

        public abstract void Connect();

        internal void SetSocket()
        {
            this.IsDispose = false;
            this.aCallback = new AsyncCallback(this.ReceiveCallback);
            this._Socket.ReceiveBufferSize = BufferSize;
            this._Socket.SendBufferSize = BufferSize;
            this.Buffers = new byte[BufferSize];
            this.ReceiveAsync();
            NettyPool.SessionHandler.ChannelActive(this);

        }

        public IOSession()
        {
            
        }

        /// <summary>
        /// 关闭并释放资源
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Close(string msg)
        {
            lock (this)
            {
                if (!this.IsDispose)
                {
                    this.IsDispose = true;
                    try
                    {
                        Logger.Debug("Close Tcp Socket Remote Socket LocalEndPoint：" + LocalEndPoint + " RemoteEndPoint：" + RemoteEndPoint + " : " + msg);
                        NettyPool.SessionHandler.ChannelUnregistered(this, msg);
                        IOSession outsession = null;
                        NettyPool.Sessions.TryRemove(this.ID, out outsession);
                        try { this._Socket.Close(); }
                        catch { }
                        IDisposable disposable = this._Socket;
                        if (disposable != null) { disposable.Dispose(); }
                        this.Buffers = null;
                        GC.SuppressFinalize(this);
                    }
                    catch (Exception) { }
                }
            }
        }


        /// <summary>
        /// 递归接收消息方法
        /// </summary>
        internal virtual void ReceiveAsync()
        {
            try
            {
                if (!this.IsDispose && this._Socket.Connected)
                {
                    this._Socket.BeginReceive(this.Buffers, 0, BufferSize, SocketFlags.None, out SenderError, this.aCallback, this);
                    CheckSocketError(ReceiveError);
                }
            }
            catch (System.Net.Sockets.SocketException) { this.Close("链接已经被关闭"); }
            catch (System.ObjectDisposedException) { this.Close("链接已经被关闭"); }
        }



        /// <summary>
        /// 接收消息回调函数
        /// </summary>
        /// <param name="iar"></param>
        internal virtual void ReceiveCallback(IAsyncResult iar)
        {
            if (!this.IsDispose)
            {
                try
                {
                    //接受消息
                    ReceiveSize = _Socket.EndReceive(iar, out ReceiveError);
                    //检查状态码
                    if (!CheckSocketError(ReceiveError) && SocketError.Success == ReceiveError)
                    {
                        //判断接受的字节数
                        if (ReceiveSize > 0)
                        {
                            LastRecvTime = Utils.TimeUtil.CurrentTimeMillis();
                            byte[] rbuff = new byte[ReceiveSize];
                            Array.Copy(this.Buffers, rbuff, ReceiveSize);
                            var msgs = mershaEncoder.Decoder(this, rbuff);
                            foreach (var msg in msgs)
                            {
                                try
                                {
                                    if (NettyPool.MessageThread == 0)
                                    {
                                        NettyPool.InitMessageThread();
                                    }
                                    Threading.ThreadPool.AddTask(NettyPool.MessageThread, new IOMessageTask(this, msg));
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error("处理消息", ex);
                                }
                            }
                            //重置连续收到空字节数
                            ZeroCount = 0;
                        }
                        else
                        {
                            ZeroCount++;
                            if (ZeroCount == 5) { this.Close("错误链接"); return; }
                        }
                        //继续开始异步接受消息
                        ReceiveAsync();
                    }
                }
                catch (System.Net.Sockets.SocketException) { this.Close("链接已经被关闭"); return; }
                catch (System.ObjectDisposedException) { this.Close("链接已经被关闭"); return; }
            }
        }

        /// <summary>
        /// 错误判断
        /// </summary>
        /// <param name="socketError"></param>
        /// <returns></returns>
        internal bool CheckSocketError(SocketError socketError)
        {
            switch ((socketError))
            {
                case SocketError.SocketError:
                case SocketError.VersionNotSupported:
                case SocketError.TryAgain:
                case SocketError.ProtocolFamilyNotSupported:
                case SocketError.ConnectionAborted:
                case SocketError.ConnectionRefused:
                case SocketError.ConnectionReset:
                case SocketError.Disconnecting:
                case SocketError.HostDown:
                case SocketError.HostNotFound:
                case SocketError.HostUnreachable:
                case SocketError.NetworkDown:
                case SocketError.NetworkReset:
                case SocketError.NetworkUnreachable:
                case SocketError.NoData:
                case SocketError.OperationAborted:
                case SocketError.Shutdown:
                case SocketError.SystemNotReady:
                case SocketError.TooManyOpenSockets:
                    this.Close(socketError.ToString());
                    return true;
            }
            return false;
        }

        public void WriteAndFlush(byte[] buf)
        {
            BufferWriter outBuf = new BufferWriter();
            this.mershaEncoder.Encoder(this, buf, outBuf);
            byte[] buffer = outBuf.GetBuffer();

            try
            {
                if (!this.IsDispose)
                {
                    //封包
                    int size = this._Socket.Send(buffer, 0, buffer.Length, SocketFlags.None, out SenderError);
                    Log.Logger.Debug("发送消息Length：" + buf.Length + " flush：" + size);
                    CheckSocketError(SenderError);
                    LastRecvTime = Utils.TimeUtil.CurrentTimeMillis();
                }
            }
            catch (System.ObjectDisposedException) { this.Close("链接已经被关闭"); }
            catch (System.Net.Sockets.SocketException) { this.Close("链接已经被关闭"); }
            catch (Exception ex) { NettyPool.SessionHandler.ExceptionCaught(this, ex); }
            finally
            {
                outBuf.Close();
                outBuf.Dispose();
            }
        }
    }
}
