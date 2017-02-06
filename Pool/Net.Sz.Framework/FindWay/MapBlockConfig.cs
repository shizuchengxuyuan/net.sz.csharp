using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Net.Sz.Framework.FindWay
{
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
