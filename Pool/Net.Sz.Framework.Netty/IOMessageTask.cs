using Net.Sz.Framework.Netty.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    internal class IOMessageTask : Net.Sz.Framework.SzThreading.TaskModel
    {

        IOSession _session;
        MessageBean _bean;

        public IOMessageTask(IOSession session, MessageBean bean)
        {
            this._session = session;
            this._bean = bean;
        }

        public override void Run()
        {

            if (this._session.isServer)
                NettyPool.SessionHandler.ChannelRead(this._session, this._bean.MsgId, this._bean.MsgBuff);
            else
                NettyPool.ClientSessionHandler.ChannelRead(this._session, this._bean.MsgId, this._bean.MsgBuff);

        }

    }
}
