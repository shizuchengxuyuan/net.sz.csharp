using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Net.Sz.Framework.AStart
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    [XmlRootAttribute("SceneInfo_Server")]
    [Serializable]
    public class MapBlockConfig
    {
        public MapBlockConfig()
        {

        }
        [XmlAttribute("MapID")]
        public int MapID { get; set; }

        [XmlElement("WalkSetting")]
        public WalkSetting Walk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class WalkSetting
        {
            public WalkSetting()
            {

            }
            [XmlAttribute("RZLEN")]
            public int Rzlen { get; set; }


            [XmlAttribute("RXLEN")]
            public int Rxlen { get; set; }


            [XmlAttribute("STARTX")]
            public int Startx { get; set; }


            [XmlAttribute("STARTY")]
            public int Starty { get; set; }


            [XmlAttribute("STARTZ")]
            public int Startz { get; set; }


            [XmlAttribute("BLOCKINFO")]
            public String Blockinfo { get; set; }
        }

    }
}
