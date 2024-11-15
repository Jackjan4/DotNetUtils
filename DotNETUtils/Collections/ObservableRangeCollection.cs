using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Roslan.DotNetUtils.Collections {



    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
    /// It overrides the ObservableCollection class to provide additional functionalities to add and remove multiple items at once while raising only one CollectionChanged event.
    /// See: https://stackoverflow.com/questions/670577/observablecollection-doesnt-support-addrange-method-so-i-get-notified-for-each
    /// The implementation is based on the answer by "Shimmy Weitzhandler" on the StackOverflow thread.
    /// The missing of the Range methods in ObservableCollection is a known issue and it is not fixed in .NET 9.
    /// See GitHub requests here: https://github.com/dotnet/runtime/issues/18087#issuecomment-359197102
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableRangeCollection<T> : ObservableCollection<T> {



        /// <summary>
        /// 
        /// </summary>
        public ObservableRangeCollection() : base() { }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection) { }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<T> collection) {
            InsertRange(Count, collection);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void InsertRange(int index, IEnumerable<T> collection) {
            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }

            // Check if the collection is empty. If yes, do nothing.
            if (collection is ICollection<T> countable) {
                if (countable.Count == 0) return;
            } else if (!collection.Any()) {
                return;
            }

            CheckReentrancy();

            foreach (var i in collection) {
                Items.Insert(index++, i);
            }

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();

            if (!(collection is IList list))
                list = new List<T>(collection);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list, index));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveRange(IEnumerable<T> collection) {
            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }

            // Check if the collection is empty. If yes, do nothing.
            if (collection is ICollection<T> countable) {
                if (countable.Count == 0) return;
            } else if (!collection.Any()) {
                return;
            }

            CheckReentrancy();

            var removedItems = new List<T>();


            // Other implementations on the internet use clusters to remove items to call OnCollectionChanged multiple times for each clusters that contains consecutive items.
            foreach (var i in collection) {
                bool removed = Items.Remove(i);
                if (removed) {
                    removedItems.Add(i);
                }
            }

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();

            if (!(collection is IList list)) {
                list = new List<T>(collection);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
        }



        #region "Helper Functions"
        private void OnCountPropertyChanged() {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        private void OnIndexerPropertyChanged() {
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
        #endregion

    }
}
