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
    internal class IOMessageTask : Net.Sz.Framework.Threading.TaskModel
    {

        IOSession _session;
        byte[] _msg;

        public IOMessageTask(IOSession session, byte[] msg)
        {
            this._session = session;
            this._msg = msg;
        }

        public override void Run()
        {
            NettyPool.SessionHandler.ChannelRead(this._session, this._msg);
        }

    }
}
