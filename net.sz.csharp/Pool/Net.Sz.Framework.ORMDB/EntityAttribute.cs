using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ORMDB
{
    /// <summary>
    /// 数据库关联类标识符
    /// </summary>
    public class EntityAttribute : Attribute
    {

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
