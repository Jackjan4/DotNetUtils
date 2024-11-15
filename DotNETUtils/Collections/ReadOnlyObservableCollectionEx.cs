﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;



namespace Roslan.DotNetUtils.Collections {



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadOnlyObservableCollectionEx<T> : ReadOnlyObservableCollection<T> {



        /// <summary>
        /// 
        /// </summary>
        public new event NotifyCollectionChangedEventHandler CollectionChanged;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public ReadOnlyObservableCollectionEx(ObservableCollection<T> list) : base(list) {
            base.CollectionChanged += List_CollectionChanged;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (CollectionChanged == null) return;
            CollectionChanged(sender, e);
        }
    }
}
