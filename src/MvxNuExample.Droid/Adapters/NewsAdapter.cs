using System;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.ExtensionMethods;
using System.Collections;
using MvvmCross.Binding.Droid.BindingContext;


namespace MvxNuExample.Droid.Adapters
{
    public class NewsAdapter : MvxRecyclerAdapter
    {
        public NewsAdapter(IMvxAndroidBindingContext bindingContext) : base (bindingContext)
		{
        }

        public override int ItemCount
        {
            get
            {
                return (ItemsSource as IList).Count + 2;
            }
        }

        public override object GetItem(int position)
        {
            if (position == 0 || position == (ItemsSource.Count() - 1))
                return null;
            else
                return base.GetItem(position - 1);
        }

        public override int GetItemViewType(int position)
        {
            if (position == 0)
                return 0;

            if (position > ItemsSource.Count())
                return 2;

            return 1;
        }

        public override Android.Support.V7.Widget.RecyclerView.ViewHolder OnCreateViewHolder(Android.Views.ViewGroup parent, int viewType)
        {
            return base.OnCreateViewHolder(parent, viewType);
        }

        public override void OnBindViewHolder(Android.Support.V7.Widget.RecyclerView.ViewHolder holder, int position)
        {
            if (position == 0 || position > ItemsSource.Count())
                ((IMvxRecyclerViewHolder)holder).DataContext = BindingContext.DataContext;
            else
                ((IMvxRecyclerViewHolder)holder).DataContext = ItemsSource.ElementAt(position - 1);
        }

        protected override Android.Views.View InflateViewForHolder(Android.Views.ViewGroup parent, int viewType, MvvmCross.Binding.Droid.BindingContext.IMvxAndroidBindingContext bindingContext)
        {
            switch (viewType)
            {
                case 0:
                    return bindingContext.BindingInflate(Resource.Layout.listitem_news, parent, false);

                case 1:
                    return bindingContext.BindingInflate(Resource.Layout.listitem_news, parent, false);

                case 2:
                    return bindingContext.BindingInflate(Resource.Layout.listitem_news, parent, false);

                default:
                    return base.InflateViewForHolder(parent, viewType, bindingContext);
            }

        }
    }
}