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
    public class FindWayPool1
    {
        static readonly FindWayPool1 instance = new FindWayPool1();
        public static FindWayPool1 GetInstance() { return instance; }

        /// <summary>
        /// 阻挡点配置，，
        /// </summary>
        public int BlockConst = 1;

        //从开启列表查找F值最小的节点
        private Point GetMinFFromOpenList(Dictionary<string, Point> Open_List)
        {
            Point Pmin = null;
            int count = Open_List.Count;
            foreach (var item in Open_List)
            {
                if (Pmin == null || Pmin.F > item.Value.F)
                    Pmin = item.Value;
            }
            return Pmin;
        }

        //判断关闭列表是否包含一个坐标的点
        private bool IsInCloseList(string key, Dictionary<string, Point> Close_List)
        {
            return Close_List.ContainsKey(key);
        }

        //从关闭列表返回对应坐标的点
        private Point GetPointFromCloseList(string key, Dictionary<string, Point> Close_List)
        {
            try
            {
                return Close_List[key];
            }
            catch (KeyNotFoundException)
            {

            }
            //if (Close_List.ContainsKey(key))
            //{
            //    return Close_List[key];
            //}
            return null;
        }

        //判断开启列表是否包含一个坐标的点
        private bool IsInOpenList(string key, Dictionary<string, Point> Open_List)
        {
            return Open_List.ContainsKey(key);
        }
        //从开启列表返回对应坐标的点
        private Point GetPointFromOpenList(string key, Dictionary<string, Point> Open_List)
        {
            //try
            //{
            //    return Open_List[key];
            //}
            //catch (KeyNotFoundException)
            //{

            //}
            if (Open_List.ContainsKey(key))
                return Open_List[key];
            return null;
        }

        //计算某个点的G值
        private int GetG(Point p)
        {
            if (p.Next == null) return 0;
            if (p.X == p.Next.X || p.Y == p.Next.Y) return p.Next.G + 10;
            else return p.Next.G + 14;
        }

        //计算某个点的H值
        private int GetH(Point p, Point pb)
        {
            return Math.Abs(p.X - pb.X) + Math.Abs(p.Y - pb.Y);
        }

        //检查当前节点附近的节点
        private void CheckP8(Point p0, byte[,] map, Point pa, Point pb, Dictionary<string, Point> Open_List, Dictionary<string, Point> Close_List)
        {
            //这里的循环其实就是8方向判断
            for (int xt = p0.X - 1; xt <= p0.X + 1; xt++)
            {
                for (int yt = p0.Y - 1; yt <= p0.Y + 1; yt++)
                {
                    //排除超过边界和等于自身的点
                    if ((xt >= 0 && xt < map.GetLongLength(1) && yt >= 0 && yt < map.GetLongLength(0)) && !(xt == p0.X && yt == p0.Y))
                    {
                        String key = xt + "-" + yt;
                        //排除障碍点和关闭列表中的点
                        if (map[yt, xt] != BlockConst && !IsInCloseList(key, Close_List))
                        {
                            Point pt = GetPointFromOpenList(key, Open_List);
                            if (pt != null)
                            {
                                //如果节点在开启列表中更新带价值
                                int G_new = 0;
                                if (p0.X == pt.X || p0.Y == pt.Y) G_new = p0.G + 10;
                                else G_new = p0.G + 14;
                                if (G_new < pt.G)
                                {
                                    //Open_List.Remove(pt);
                                    pt.Next = p0;
                                    pt.G = G_new;
                                    //Open_List.Add(pt);
                                }
                            }
                            else
                            {
                                //不在开启列表中,如果不存在创建添加到开启列表中
                                pt = new Point();
                                pt.X = xt;
                                pt.Y = yt;
                                pt.Next = p0;
                                pt.G = GetG(pt);
                                pt.H = GetH(pt, pb);
                                Open_List[pt.Key] = (pt);
                            }
                        }
                    }
                }
            }
        }

        public Point FindWay(byte[,] r, int sx, int sz, int ex, int ez)
        {
            //定义出发位置
            Point pa = new Point();
            pa.X = sx;
            pa.Y = sz;
            //定义目的地
            Point pb = new Point();
            pb.X = ex;
            pb.Y = ez;
            //如果点超出范围，或者是阻挡点
            if (0 < pb.X && pb.X < r.GetLength(1)
                && 0 < pa.X && pa.X < r.GetLength(1)
                && 0 < pb.Y && pb.Y < r.GetLength(0)
                && 0 < pa.Y && pa.Y < r.GetLength(0)
                && !CheckBlocking(r, pa)
                && !CheckBlocking(r, pb))
            {
                String key = pb.X + "-" + pb.Y;
                //开启列表
                Dictionary<string, Point> Open_List = new Dictionary<string, Point>();
                //关闭列表
                Dictionary<string, Point> Close_List = new Dictionary<string, Point>();

                Open_List[pa.Key] = (pa);
                while (!(IsInOpenList(key, Open_List) || Open_List.Count == 0))
                {
                    Point p0 = GetMinFFromOpenList(Open_List);
                    if (p0 == null) return null;
                    Open_List.Remove(p0.Key);
                    Close_List[p0.Key] = (p0);
                    CheckP8(p0, r, pa, pb, Open_List, Close_List);
                }
                Point p = GetPointFromOpenList(key, Open_List);
                return Reverse(p);
            }
            return null;
        }

        Point Reverse(Point point)
        {
            //新节点
            Point newNode = null;
            //当前节点
            Point current = point;
            while (current != null)
            {
                //取当前节点的下一个，放入临时节点中
                Point temp = current.Next;
                //将当前节点的下一个设置为新节点
                //(第一次将设置为null，也对着呢，因为第一个节点将作为尾节点)
                current.Next = newNode;
                //把当前节点给新节点
                newNode = current;
                //把临时节点给当前节点（就是取当前节点的下一个而已）
                current = temp;
            }
            //将最后的新节点给头节点
            return newNode;
        }

        public bool CheckBlocking(byte[,] r, Point point)
        {
            return CheckBlocking(r, point.X, point.Y);
        }

        public bool CheckBlocking(byte[,] r, int x, int y)
        {
            return r[y, x] == BlockConst;
        }

        public void PrintMap(byte[,] r)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int y = 0; y < r.GetLongLength(0); y++)//Y轴
            {
                for (int x = 0; x < r.GetLongLength(1); x++)//X轴
                {
                    Console.Write(r[y, x]);
                }
                Console.Write("\n");
            }

        }

        public void PrintWay(byte[,] r, Point way)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            while (way != null)
            {
                Console.CursorLeft = way.X;
                Console.CursorTop = way.Y;
                Console.Write("4");
                System.Threading.Thread.Sleep(20);
                way = way.Next;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        bool check(int x, int y, Point way)
        {
            Point f = way;
            while (f != null)
            {
                if (f.X == x && f.Y == y)
                {
                    return true;
                }
                f = f.Next;
            }
            return false;
        }
    }
}
