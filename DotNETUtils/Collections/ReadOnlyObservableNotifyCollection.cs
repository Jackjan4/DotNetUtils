using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Roslan.DotNetUtils.Collections {


    /// <summary>
    /// Acts as a read-only wrapper around an ObservableNotifyCollection.
    /// 
    /// </summary>
    public class ReadOnlyObservableNotifyCollection<T> : ReadOnlyObservableCollectionEx<T> where T : INotifyPropertyChanged {



        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler ItemPropertyChanged;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public ReadOnlyObservableNotifyCollection(ObservableNotifyCollection<T> list) : base(list) {
            list.ItemPropertyChanged += List_ItemPropertyChanged;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void List_ItemPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (ItemPropertyChanged == null) return;
            ItemPropertyChanged(sender, e);
        }
    }
}
