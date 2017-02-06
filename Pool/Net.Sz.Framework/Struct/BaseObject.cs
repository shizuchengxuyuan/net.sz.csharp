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
    /// </summary>
    [Serializable]
    public class BaseObject
    {

        public long ID { get; set; }

        /// <summary>
        /// 这个是不会被序列化的字段
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public ObjectAttribute TmpOther { get; set; }

        public BaseObject()
            : this(SzExtensions.GetGUID())
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public BaseObject(long id)
        {

            this.ID = id;
            TmpOther = new ObjectAttribute();

        }
    }
}
