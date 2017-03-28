using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ORMDB
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class DBCache
    {
        public DBCache()
        {
            ColumnPs = new List<ColumnAttribute>();
            Columns = new List<ColumnAttribute>();
        }
        public Type Instance { get; set; }

        public string TableName { get; set; }
        /// <summary>
        /// 主键列
        /// </summary>
        public List<ColumnAttribute> ColumnPs { get; set; }
        /// <summary>
        /// 所有列
        /// </summary>
        public List<ColumnAttribute> Columns { get; set; }

    }
}
