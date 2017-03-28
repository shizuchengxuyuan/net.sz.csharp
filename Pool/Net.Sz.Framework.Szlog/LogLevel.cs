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
namespace Net.Sz.Framework.Szlog
{

    /// <summary>
    /// 日志级别
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 完全不输出任何日志
        /// </summary>
        Null = 0,
        /// <summary>
        /// 输出 DEBUG 以上级别
        /// </summary>
        DEBUG = 1,
        /// <summary>
        /// 输出 INFO 以上级别
        /// </summary>
        INFO = 2,
        /// <summary>
        /// 输出 WARN 以上级别
        /// </summary>
        WARN = 3,
        /// <summary>
        /// 输出 ERROR 以上级别
        /// </summary>
        ERROR = 4
    }

}
