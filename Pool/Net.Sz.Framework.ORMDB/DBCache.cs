using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ORMDB
{
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
