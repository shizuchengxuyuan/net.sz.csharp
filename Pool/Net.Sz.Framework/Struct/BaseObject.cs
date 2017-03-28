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
namespace Net.Sz
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    [Serializable]
    public class BaseObject
    {

        public long ID { get; set; }

        /// <summary>
        /// 这个是不会被序列化的字段
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]

        private ObjectAttribute _TmpOther;

        /// <summary>
        /// 
        /// </summary>
        public ObjectAttribute TmpOther
        {
            get
            {
                /*未考虑并发*/
                if (_TmpOther == null) _TmpOther = new ObjectAttribute();
                return _TmpOther;
            }
            set { this._TmpOther = value; }
        }

        public BaseObject() : this(SzExtensions.GetGUID()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public BaseObject(long id) { this.ID = id; }
    }
}
