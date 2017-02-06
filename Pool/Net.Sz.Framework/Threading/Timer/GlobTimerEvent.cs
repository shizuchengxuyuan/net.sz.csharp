using Net.Sz.Framework.Script;
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
namespace Net.Sz.Framework.Threading.Timer
{

    /// <summary>
    /// 
    /// </summary>
    internal class GlobTimerEvent : TimerTask
    {
        ScriptPool _ScriptPool;
        public GlobTimerEvent(ScriptPool scriptPool)
            : base(995)
        {
            this._ScriptPool=scriptPool;
        }

        int houer = -1;
        int minute = -1;

        public override void Run()
        {
            DateTime dt = DateTime.Now;
            {
                var scripts = this._ScriptPool.GetScripts<ITimer_Second_EventScript>();
                foreach (var item in scripts)
                {
                    item.Action(dt.Second);
                }
            }
            
            if (dt.Minute != minute)
            {
                minute = dt.Minute;
                var scripts = this._ScriptPool.GetScripts<ITimer_Minute_EventScript>();
                foreach (var item in scripts)
                {
                    item.Action(dt.Minute);
                }
            }

            if (dt.Hour != houer)
            {
                houer = dt.Hour;
                var scripts = this._ScriptPool.GetScripts<ITimer_Minute_EventScript>();
                foreach (var item in scripts)
                {
                    item.Action(dt.Minute);
                }
            }
        }
    }
}
