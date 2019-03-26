using System;
using System.Collections;
using System.Collections.Generic;

namespace De.JanRoslan.NETUtils.Collections
{

    // TODO: More general implementations
    public class ListedDictionary<K, V> : IDictionary<K, V>, IEnumerable<V>
    {
        // Root dictionary
        private Dictionary<K, V> root;


        // List that connects every key to a number
        private List<K> referenceTable;


        /// <summary>
        /// 
        /// </summary>
        public ICollection<K> Keys => root.Keys;


        /// <summary>
        /// 
        /// </summary>
        public ICollection<V> Values => root.Values;

        public int Count => referenceTable.Count;

        public bool IsReadOnly => false;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V this[K key]
        {
            get => Get(key);
            set => Add(key, value);
        }

        public V this[int pos] { get => Get(pos); }



        /// <summary>
        /// Constructor, 
        /// </summary>
        public ListedDictionary() {
            root = new Dictionary<K, V>();
            referenceTable = new List<K>();

        }

        public void Add(K key, V value) {
            root[key] = value;

            referenceTable.Add(key);
        }


        public V Get(int pos) {
            return root[referenceTable[pos]];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Get(K key) {
            V result = default(V);
            result = root[key];

            return result;
        }


        /// <summary>
        /// Returns the last element and then removes it
        /// </summary>
        /// <returns></returns>
        public V DropLast() {

            K lastKey = referenceTable[referenceTable.Count - 1];
            referenceTable.RemoveAt(referenceTable.Count - 1);
            V val = root[lastKey];
            root.Remove(lastKey);
            return val;

        }

        public void Remove(int pos) {

        }

        public bool ContainsKey(K key) {
            return root.ContainsKey(key);
        }

        public bool Remove(K key) {
            throw new NotImplementedException();
        }

        public bool TryGetValue(K key, out V value) {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<K, V> item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }



        public bool Contains(KeyValuePair<K, V> item)
        {
            return root.ContainsKey(item.Key) && root[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, V> item) {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
            return root.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return root.GetEnumerator();
        }

        IEnumerator<V> IEnumerable<V>.GetEnumerator() {
            return root.Values.GetEnumerator();
        }
    }
}
