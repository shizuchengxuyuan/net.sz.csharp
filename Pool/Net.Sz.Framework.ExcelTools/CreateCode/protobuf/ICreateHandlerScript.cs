using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Net.Sz.Framework.Script;

namespace Net.Sz.Framework.ExcelTools.CreateCode.protobuf
{

    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public interface ICreateHandlerScript : IBaseScript
    {
        void Create(Type memberType, MemberInfo member);
    }
}
