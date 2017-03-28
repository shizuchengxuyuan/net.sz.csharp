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
namespace Net.Sz.Framework.SzThreading
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public abstract class TaskModel
    {
        private static LongId0Util ids = new LongId0Util();

        /// <summary>
        /// 
        /// </summary>
        public long ID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 是否已经删除状态
        /// </summary>
        public bool IsDel { get; set; }


        private ObjectAttribute _RunAttribute;

        /// <summary>
        /// 
        /// </summary>
        public ObjectAttribute RunAttribute
        {
            get
            {
                /*未考虑并发*/
                if (_RunAttribute == null) _RunAttribute = new ObjectAttribute();
                return _RunAttribute;
            }
            set { this._RunAttribute = value; }
        }
        /// <summary>
        /// 是否已经取消
        /// </summary>         
        public bool Cancel { get; set; }

        internal long CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskModel(string name = "无名")
        {
            this.ID = ids.GetId();
            this.Name = name;
            this.Cancel = false;
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
