using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Utils
{
    /// <summary>
    /// 每小时能产生 114749 个 id 100000 ~ 214748，
    /// <para>不保证多程序重复性</para>
    /// <para>启动程序后，重复周期是 一个月,</para>
    /// <para>上一次启动和下一次启动之间重启周期是一小时</para>
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class IntegerIdUtil
    {
        private int id = 99999;

        /// <summary>
        /// 每小时能产生 114749 个 id 100000 ~ 214748，
        /// <para>不保证多程序重复性</para>
        /// <para>启动程序后，重复周期是 一个月,</para>
        /// <para>上一次启动和下一次启动之间重启周期是一小时</para>
        /// </summary>
        public IntegerIdUtil()
        {

        }

        private String newformatter = "";
        /// <summary>
        /// 年月日时分服务器id和自增id组合的19位id
        /// </summary>
        /// <returns></returns>
        public Int32 GetId()
        {
            /*相对耗时*/
            string datekey = DateTime.Now.ToString("ddHH");
            long tmpid = 0;
            lock (typeof(string))
            {
                if (!newformatter.Equals(datekey))
                {
                    newformatter = datekey;
                    id = 99999;
                }
                tmpid = ++id;
            }
            if (214748 < tmpid)
            {
                throw new Exception("超过每分钟创建量 214748");
            }
            return Convert.ToInt32(id + datekey);
        }
    }
}
