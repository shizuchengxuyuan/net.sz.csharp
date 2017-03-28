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
    /// 每分钟能产生 100亿 个id 0 ~ 9999999999，
    /// <para>不保证多程序重复性</para>
    /// <para>启动程序后，重复周期是 10 年,</para>
    /// <para>上一次启动和下一次启动之间重启周期是1分钟</para>
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class LongId0Util
    {

        private int id = -1;

        /// <summary>
        /// 每分钟能产生 100亿 个id 0 ~ 9999999999，
        /// <para>不保证多程序重复性</para>
        /// <para>启动程序后，重复周期是 10 年,</para>
        /// <para>上一次启动和下一次启动之间重启周期是1分钟</para>
        /// </summary>
        public LongId0Util()
        {

        }

        private String newformatter = "";
        /// <summary>
        /// 年月日时分服务器id和自增id组合的19位id
        /// </summary>
        /// <returns></returns>
        public Int64 GetId()
        {
            /*相对耗时*/
            string datekey = DateTime.Now.ToString("yyyyMMddHHmm").Substring(3);
            long tmpid = 0;
            lock (typeof(string))
            {
                if (!newformatter.Equals(datekey))
                {
                    newformatter = datekey;
                    id = -1;
                }
                tmpid = ++id;
            }
            if (tmpid > 9999999999L)
            {
                throw new Exception("超过每分钟创建量 9999999999");
            }
            return Convert.ToInt64(datekey + id.ToString().PadLeft(10, '0'));
        }
    }
}
