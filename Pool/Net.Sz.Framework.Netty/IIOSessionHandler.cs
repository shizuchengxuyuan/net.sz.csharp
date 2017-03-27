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
namespace Net.Sz.Framework.Netty
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIOSessionHandler
    {

        /// <summary>
        /// 新建链接
        /// </summary>
        /// <param name="session"></param>
        void ChannelActive(IOSession session);

        /// <summary>
        /// 链接发送异常
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ex"></param>
        void ExceptionCaught(IOSession session, Exception ex);

        /// <summary>
        /// 有消息,请注意消息是多线程处理的。也许你前一个消息没有处理完成，后一个消息已经来了
        /// </summary>
        /// <param name="session"></param>
        /// <param name="buffer"></param>
        void ChannelRead(IOSession session, int msgId, byte[] buffer);

        /// <summary>
        /// 链接断开
        /// </summary>
        /// <param name="session"></param>
        void ChannelUnregistered(IOSession session, params string[] obj);

        /// <summary>
        /// 链接闲置状态
        /// </summary>
        /// <param name="session"></param>
        void ChannelInactive(IOSession session);
    }
}
