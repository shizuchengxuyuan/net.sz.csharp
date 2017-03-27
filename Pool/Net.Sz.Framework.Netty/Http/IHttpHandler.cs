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
namespace Net.Sz.Framework.Netty.Http
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        /// 并发处理的
        /// </summary>
        /// <param name="session">连接对象</param>
        void Run(HttpSession session);
    }
}
