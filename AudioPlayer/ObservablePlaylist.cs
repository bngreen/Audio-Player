using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AudioPlayer
{
    public class ObservablePlaylist : Core.AudioSourceCollection, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public override void Add(Core.AudioSource item)
        {
            base.Add(item);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        public override void Clear()
        {
            base.Clear();
            NotifyCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        public override bool Remove(Core.AudioSource item)
        {
            var succd = base.Remove(item);
            if (succd)
                NotifyCollectionChanged(NotifyCollectionChangedAction.Reset);
            return succd;
        }
        private void NotifyCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
