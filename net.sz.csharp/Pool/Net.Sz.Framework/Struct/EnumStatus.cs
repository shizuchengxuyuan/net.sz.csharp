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
namespace Net.Sz
{
    /// <summary>
    /// 辅助状态机
    /// </summary>
    public class EnumStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public long Value { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public long GroupValue { private set; get; }

        /// <summary>
        /// 辅助状态机
        /// </summary>
        /// <param name="value"></param>
        /// <param name="group"></param>
        public EnumStatus(long value, long group)
        {
            this.Value = value;
            this.GroupValue = group;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool HasFlag(EnumStatus status)
        {
            return (this.Value & status.Value) != 0;
        }

        public bool HasFlag(long value)
        {
            return (this.Value & value) != 0;
        }

        /// <summary>
        /// 显示结果值
        /// </summary>
        public string GetByteString()
        {
            return ("结果：" + this.Value.ToString().PadLeft(10, ' ') + " -> " + Convert.ToString(this.Value, 2).PadLeft(64, '0'));
        }


        /// <summary>
        /// 清除分组状态。当分组为0时清除所有状态
        /// </summary>
        /// <param name="statusLeft"></param>
        /// <param name="statusRight"></param>
        /// <returns></returns>
        public static EnumStatus operator |(EnumStatus statusLeft, EnumStatus statusRight)
        {
            if (statusRight.GroupValue == 0)//当分组为0的时候清除所有状态
            {
                statusLeft.Value = statusLeft.Value & (statusRight.GroupValue) | statusRight.Value;
            }
            else
            {//当分组不为零
                statusLeft.Value = statusLeft.Value & (~statusRight.GroupValue) | statusRight.Value;
            }
            return statusLeft;
        }

        /// <summary>
        /// 重载运算符会清除指定状态
        /// </summary>
        /// <param name="statusLeft"></param>
        /// <param name="statusRight"></param>
        /// <returns></returns>
        public static EnumStatus operator &(EnumStatus statusLeft, EnumStatus statusRight)
        {
            statusLeft.Value = statusLeft.Value & (~statusRight.Value);
            return statusLeft;
        }

    }
}
