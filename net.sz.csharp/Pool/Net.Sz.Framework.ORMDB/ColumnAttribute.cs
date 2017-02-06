using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Net.Sz.Framework.ORMDB
{

    /// <summary>
    /// 属性字段
    /// </summary>
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute()
        {

        }
        /// <summary>
        /// 数据库对应的字段名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 原始字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DBType { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 是否是数据库主键
        /// </summary>
        public bool IsP { get; set; }

        /// <summary>
        /// 是否允许为null
        /// </summary>
        public bool IsNotNull { get; set; }

        /// <summary>
        /// 自增
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// 将会被忽略的属性
        /// </summary>
        public bool IsTemp { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        internal Type ValueType { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        internal PropertyInfo PropertyType { get; set; }
    }

}
