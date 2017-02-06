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
    internal class CheckIOSessionTimerTask : Threading.TimerTask
    {

        public CheckIOSessionTimerTask()
            : base(200)
        {

        }

        public override void Run()
        {
            var sessions = Netty.NettyPool.Sessions.Values.ToArray();
            foreach (var item in sessions)
            {
                if (item.LastRecvTime == 0)
                {
                    item.LastRecvTime = Utils.TimeUtil.CurrentTimeMillis();
                }
                else if ((Utils.TimeUtil.CurrentTimeMillis() - item.LastRecvTime) > 60 * 1000)
                {
                    item.Close("Long time did not receive the message processing line");
                }
            }
        }

    }
}
