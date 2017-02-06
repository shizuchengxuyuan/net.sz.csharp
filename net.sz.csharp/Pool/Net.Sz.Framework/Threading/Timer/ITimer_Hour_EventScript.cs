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
    public interface ITimer_Hour_EventScript : IBaseScript
    {

        void Action(int hour);
    }

}
