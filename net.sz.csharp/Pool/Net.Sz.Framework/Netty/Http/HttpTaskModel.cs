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
    internal class HttpTaskModel : Net.Sz.Framework.Threading.TaskModel
    {

        HttpSession session;
        IHttpHandler httphandler;

        public HttpTaskModel(HttpSession session, IHttpHandler handler)
        {
            this.httphandler = handler;
            this.session = session;
        }

        public override void Run()
        {
            session.process(this.httphandler);
        }

    }
}
