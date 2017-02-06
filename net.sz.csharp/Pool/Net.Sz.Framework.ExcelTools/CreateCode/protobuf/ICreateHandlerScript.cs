using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Net.Sz.Framework.Script;

namespace Net.Sz.Framework.ExcelTools.CreateCode.protobuf
{
    public interface ICreateHandlerScript : IBaseScript
    {
        void Create(Type memberType, MemberInfo member);
    }
}
