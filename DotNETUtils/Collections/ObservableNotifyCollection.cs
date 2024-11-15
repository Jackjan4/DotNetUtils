using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Roslan.DotNetUtils.Collections {



    /// <summary>
    /// An ObservableCollection that listens to the PropertyChanged event of its items.
    /// When an item has one of its obsverved properties changed, the ItemChanged event is raised.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableNotifyCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged {



        /// <summary>
        /// 
        /// </summary>
        public PropertyChangedEventHandler ItemPropertyChanged;



        /// <summary>
        /// 
        /// </summary>
        public ObservableNotifyCollection() {
            base.CollectionChanged += Items_CollectionChanged;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public ObservableNotifyCollection(IEnumerable<T> collection) : base(collection) {
            base.CollectionChanged += Items_CollectionChanged;

            foreach (var item in collection)
                item.PropertyChanged += Item_PropertyChanged;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e == null) return;

            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= Item_PropertyChanged;

            if (e.NewItems != null)
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += Item_PropertyChanged;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            ItemPropertyChanged?.Invoke(sender, e);
        }
    }
}
