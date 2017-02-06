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
namespace Net.Sz.Framework.FindWay
{
    /// <summary>
    /// 
    /// </summary>
    public class Point
    {
        private int y;
        //坐标点
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
                this.Key = this.x + "-" + this.y;
            }
        }

        private int x;
        //坐标点
        public int X
        {
            get { return this.x; }
            set
            {
                this.x = value;
                this.Key = this.x + "-" + this.y;
            }
        }

        public string Key { get; set; }


        private int g;
        //从起点到当前点的代价
        public int G
        {
            get { return this.g; }
            set
            {
                this.g = value;
                this.F = this.g + this.h;
            }
        }
        private int h;
        //从终点到当前点的代价
        public int H
        {
            get { return this.h; }
            set
            {
                this.h = value;
                this.F = this.g + this.h;
            }
        }

        /// <summary>
        /// 代价总和
        /// </summary>
        public int F { get; private set; }

        public Point()
        {
        }

        public Point Next { get; set; }
    }
}
