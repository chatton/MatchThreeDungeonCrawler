using System.Collections;
using System.Collections.Generic;

namespace Util
{
    public class MultiMap<T1, T2> : IEnumerable<T1>
    {
        private Dictionary<T1, T2> _dictionary1;
        private Dictionary<T2, T1> _dictionary2;

        public MultiMap()
        {
            _dictionary1 = new Dictionary<T1, T2>();
            _dictionary2 = new Dictionary<T2, T1>();
        }

        public void Add(T1 t1, T2 t2)
        {
            _dictionary1[t1] = t2;
            _dictionary2[t2] = t1;
        }

        public bool Remove(T1 t1)
        {
            return _dictionary1.Remove(t1);
        }

        public bool ContainsKey(T2 t2)
        {
            return _dictionary2.ContainsKey(t2);
        }

        public bool ContainsKey(T1 t1)
        {
            return _dictionary1.ContainsKey(t1);
        }

        public T1 Get(T2 t2)
        {
            return _dictionary2[t2];
        }

        public T2 Get(T1 t1)
        {
            return _dictionary1[t1];
        }


        public IEnumerator<T1> GetEnumerator()
        {
            foreach (KeyValuePair<T1, T2> pair in _dictionary1)
            {
                yield return pair.Key;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<T1, T2> pair in _dictionary1)
            {
                yield return pair.Key;
            }
        }
    }
}