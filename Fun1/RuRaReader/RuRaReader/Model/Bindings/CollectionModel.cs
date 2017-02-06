using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Android.Views;
using Object = Java.Lang.Object;

namespace RuRaReader.Model.Bindings
{
    public class CollectionBinding<T> : BindingBase where T : Object
    {
        private readonly Func<T, View> mApplyTemplate;
        public ObservableCollection<T> ItemsSource { get; set; }
        public ViewGroup Container { get; set; }

        public CollectionBinding(ObservableCollection<T> itemsSource, ViewGroup container, Func<T, View> applyTemplate)
        {
            mApplyTemplate = applyTemplate;
            ItemsSource = itemsSource;
            Container = container;

            Fill();
            ItemsSource.CollectionChanged += ItemsSourceOnCollectionChanged;
        }

        private void Fill()
        {
            Container.RemoveAllViews();
            foreach (var obj in ItemsSource)
            {
                var view = mApplyTemplate(obj);
                view.Tag = obj;
                Container.AddView(view);
            }
        }

        private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var obj in e.NewItems.Cast<T>())
                    {
                        var view = mApplyTemplate(obj);
                        view.Tag = obj;
                        var index = ItemsSource.IndexOf(obj);
                        Container.AddView(view, index);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var obj in e.OldItems.Cast<T>())
                    {
                        for (int i = 0; i < Container.ChildCount; i++)
                        {
                            var child = Container.GetChildAt(i);
                            if (child.Tag == obj)
                            {
                                Container.RemoveView(child);
                                break;
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Fill();
                    break;
            }
        }
    }
}