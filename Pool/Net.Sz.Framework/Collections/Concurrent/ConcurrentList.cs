using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;

namespace Net.Sz.Framework.Collections.Concurrent
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {

        List<T> _list;

        // 摘要: 
        //     初始化 System.Collections.Generic.List<T> 类的新实例，该实例为空并且具有默认初始容量。
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public ConcurrentList()
        {
            _list = new List<T>();
        }
        //
        // 摘要: 
        //     初始化 System.Collections.Generic.List<T> 类的新实例，该实例包含从指定集合复制的元素并且具有足够的容量来容纳所复制的元素。
        //
        // 参数: 
        //   collection:
        //     一个集合，其元素被复制到新列表中。
        //
        // 异常: 
        //   System.ArgumentNullException:
        //     collection 为 null。
        public ConcurrentList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }
        //
        // 摘要: 
        //     初始化 System.Collections.Generic.List<T> 类的新实例，该实例为空并且具有指定的初始容量。
        //
        // 参数: 
        //   capacity:
        //     新列表最初可以存储的元素数。
        //
        // 异常: 
        //   System.ArgumentOutOfRangeException:
        //     capacity 小于 0。
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public ConcurrentList(int capacity)
        {
            _list = new List<T>(capacity);
        }


        public void CopyTo(T[] array, int index)
        {
            lock (this)
                _list.CopyTo(array, index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Clear()
        {
            lock (this) _list.Clear();
        }

        public bool Contains(T value)
        {
            lock (this)
                return _list.Contains(value);
        }

        public int IndexOf(T value)
        {
            lock (this)
                return _list.IndexOf(value);
        }

        public void Insert(int index, T value)
        {
            lock (this)
                _list.Insert(index, value);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (this)
                return _list.GetEnumerator();
        }

        public void Add(T item)
        {
            lock (this)
                _list.Add(item);
        }

        public bool Remove(T item)
        {
            lock (this)
                return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            lock (this)
                _list.RemoveAt(index);
        }

        T IList<T>.this[int index]
        {
            get
            {
                lock (this)
                    return _list[index];
            }
            set
            {
                lock (this)
                    _list[index] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (this)
                return _list.GetEnumerator();
        }


        public bool IsReadOnly
        {
            get { return false; }
        }



    }
}
