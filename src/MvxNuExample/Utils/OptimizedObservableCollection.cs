using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MvxNuExample.Utils
{
    /// <summary>
    /// Optimized version of the <see cref="OptimizedObservableCollection{T}"/> class.
    /// </summary>
    public class OptimizedObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressEvents;

        public bool SuppressEvents
        {
            get { return _suppressEvents; }
            set
            {
                if (_suppressEvents != value)
                {
                    _suppressEvents = value;

                    OnPropertyChanged(new PropertyChangedEventArgs("SuppressEvents"));
                }
            }
        }

        public OptimizedObservableCollection() { }

        public OptimizedObservableCollection(IEnumerable<T> items) : base(items) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressEvents)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            try
            {
                SuppressEvents = true;

                foreach (var item in items)
                    Add(item);
            }
            finally
            {
                SuppressEvents = false;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void ReplaceWith(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            SuppressEvents = true;
            Clear();
            AddRange(items);
        }

        public void ReplaceRange(IEnumerable<T> items, int firstIndex, int oldSize)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            try
            {
                SuppressEvents = true;

                var lastIndex = firstIndex + oldSize - 1;

                // If there are more items in the previous list, remove them.
                while (firstIndex + items.Count() <= lastIndex)
                    RemoveAt(lastIndex--);

                foreach (var item in items)
                {
                    if (firstIndex <= lastIndex)
                        SetItem(firstIndex++, item);
                    else
                        Insert(firstIndex++, item);
                }
            }
            finally
            {
                SuppressEvents = false;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void SwitchTo(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            var itemIndex = 0;
            var count = Count;

            foreach (var item in items)
            {
                if (itemIndex >= count)
                {
                    Add(item);
                }
                else if (!Equals(this[itemIndex], item))
                {
                    this[itemIndex] = item;
                }

                itemIndex++;
            }

            while (count > itemIndex)
                RemoveAt(--count);
        }
    }
}
