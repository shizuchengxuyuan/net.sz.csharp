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
namespace Net.Sz.Framework.SzThreading
{
    /// <summary>
    /// 无需再去声明实体类
    /// </summary>
    public class ActionTask : TaskModel
    {
        Action<TaskModel> arun;

        /// <summary>
        /// 
        /// </summary>
        public ActionTask(Action<TaskModel> arun) : this("无名", arun) { }

        /// <summary>
        /// 
        /// </summary>
        public ActionTask(string name, Action<TaskModel> arun)
            : base(name)
        {
            this.arun = arun;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Run()
        {
            arun(this);
        }

    }
}
