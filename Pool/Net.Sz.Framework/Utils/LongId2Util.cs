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
    /// 
    ///  每分钟能产生 一亿 个id 0 ~ 99999999，
    /// <para>分100组（0-99），分别计算id,</para>
    /// <para>启动程序后，重复周期是 10 年,</para>
    /// <para>上一次启动和下一次启动之间重启周期是1分钟</para>
    /// </summary>
    public class LongId2Util
    {

        private int id = -1;
        /// <summary>
        /// 
        /// </summary>
        public int CONSTID = 1;

        /// <summary>
        /// 每分钟能产生 一亿 个id 0 ~ 99999999，
        /// <para>分100组（0-99），分别计算id,</para>
        /// <para>启动程序后，重复周期是 10 年,</para>
        /// <para>上一次启动和下一次启动之间重启周期是1分钟</para>
        /// </summary>
        /// <param name="idd">0~99</param>
        public LongId2Util(int idd)
        {
            if (0 > idd || 99 < idd)
            {
                throw new Exception("idd 0~99");
            }
            CONSTID = idd;
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
            int tmpid = 0;
            lock (typeof(string))
            {
                if (!newformatter.Equals(datekey))
                {
                    newformatter = datekey;
                    id = -1;
                }
                tmpid = ++id;
            }
            if (tmpid > 99999999)
            {
                throw new Exception("超过每分钟创建量 99999999");
            }
            return Convert.ToInt64(datekey + CONSTID.ToString().PadLeft(2, '0') + id.ToString().PadLeft(8, '0'));
        }
    }
}
