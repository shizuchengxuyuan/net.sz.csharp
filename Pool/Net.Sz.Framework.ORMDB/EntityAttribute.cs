using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ORMDB
{
    /// <summary>
    /// 数据库关联类标识符
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class EntityAttribute : Attribute
    {

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
