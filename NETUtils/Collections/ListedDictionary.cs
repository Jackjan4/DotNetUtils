using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;



namespace De.JanRoslan.NETUtils.Collections {



    /// <summary>
    /// Represents a collection of key/value pairs that are accessible by the key or index.
    /// .NET contains a similar Dictionary (OrderedDictionary), however that one does not support generic types.
    /// .NET also has SortedList and SortedDictionary which are similar, generic collections, HOWEVER they automatically sort their entries which we do not do here.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ListedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<TValue> {
        // Root dictionary
        private readonly IDictionary<TKey, TValue> _rootDict;


        // List that connects every key to a number
        private readonly IList<TKey> _referenceList;


        /// <summary>
        /// 
        /// </summary>
        public ICollection<TKey> Keys => _rootDict.Keys;


        /// <summary>
        /// 
        /// </summary>
        public ICollection<TValue> Values => _rootDict.Values;

        public int Count => _referenceList.Count;

        public bool IsReadOnly => false;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key] {
            get => Get(key);
            set => Add(key, value);
        }



        public TValue this[int pos] => Get(pos);



        /// <summary>
        /// Constructor that generated an empty ListedDictionary
        /// </summary>
        public ListedDictionary() {
            _rootDict = new Dictionary<TKey, TValue>();
            _referenceList = new List<TKey>();

        }



        public void Add(TKey key, TValue value) {
            // Throw, if key is already contained
            if (_rootDict.ContainsKey(key)) {

            }
            _rootDict[key] = value;
            _referenceList.Add(key);
        }



        public TValue Get(int pos) {
            return _rootDict[_referenceList[pos]];
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key) {
            TValue result = default(TValue);

            if (_rootDict.ContainsKey(key)) {
                result = _rootDict[key];
            } else {
                throw new KeyNotFoundException("The given key does not exist in the ListedDictionary");
            }

            return result;
        }



        /// <summary>
        /// Removes and Returns the last element in the dictionary.
        /// </summary>
        /// <returns></returns>
        public TValue PopLast() {

            TKey lastKey = _referenceList[_referenceList.Count - 1];
            _referenceList.RemoveAt(_referenceList.Count - 1);
            TValue val = _rootDict[lastKey];
            _rootDict.Remove(lastKey);
            return val;

        }



        public void Remove(int pos) {
            // Index is out of bounds
            if (pos > _referenceList.Count || pos < 0) {
                throw new IndexOutOfRangeException("The given index is out of bounds");
            }

            TKey key = _referenceList[pos];
            _rootDict.Remove(key);
            _referenceList.RemoveAt(pos);
        }



        public bool ContainsKey(TKey key) {
            return _rootDict.ContainsKey(key);
        }



        public bool Remove(TKey key) {
            if (!_rootDict.ContainsKey(key)) {
                throw new KeyNotFoundException("The given key does not exist in the ListedDictionary");
            }

            _referenceList.Remove(key);
            _rootDict.Remove(key);

            _rootDict.Remove(key);
            return true;
        }



        public bool TryGetValue(TKey key, out TValue value) {
            if (!_rootDict.ContainsKey(key)) {
                value = default(TValue);

                return false;
            }

            value = _rootDict[key];
            return true;
        }



        public void Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }



        public void Clear() {
            _referenceList.Clear();
            _rootDict.Clear();
        }



        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return _rootDict.ContainsKey(item.Key) && _rootDict[item.Key].Equals(item.Value);
        }



        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }



        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return Remove(item.Key);
        }



        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return _rootDict.GetEnumerator();
        }



        IEnumerator IEnumerable.GetEnumerator() {
            return _rootDict.GetEnumerator();
        }



        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() {
            return _rootDict.Values.GetEnumerator();
        }
    }
}