using Net.Sz.Framework.Utils;
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
namespace Net.Sz.Framework.Log
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class TaskModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否已经删除状态
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObjectAttribute RunAttribute { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskModel(long id, string name)
        {
            this.ID = id;
            this.Name = name;
            RunAttribute = new ObjectAttribute();
            this.RunAttribute["submitTime"] = TimeUtil.CurrentTimeMillis();
        }
        /// <summary>
        /// 
        /// </summary>
        public TaskModel() : this(0, "无名") { }

        /// <summary>
        /// 
        /// </summary>
        public TaskModel(string name) : this(0, name) { }

        /// <summary>
        /// 获取脚本提交时间
        /// </summary>
        /// <returns></returns>
        internal long GetSubmitTime()
        {
            return this.RunAttribute.GetlongValue("submitTime");
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Run();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "TaskModel{" + "ID=" + ID + ", Name=" + Name + '}';
        }
    }
}
