using Android.Runtime;
using Android.Views;
using Android.OS;
using MvvmCross.Droid.Shared.Attributes;
using MvxNuExample.ViewModels;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.Droid.BindingContext;
using MvxNuExample.Droid.Adapters;

namespace MvxNuExample.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("mvxnuexample.droid.fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {

        //public override View OnCreateView(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        //{
        //    base.OnC
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ShowHamburgerMenu = true;

            return base.OnCreateView(inflater, container, savedInstanceState);

            //var list = container.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            //list.Adapter = new NewsAdapter((IMvxAndroidBindingContext)BindingContext);


        }

        protected override int FragmentId => Resource.Layout.fragment_home;
    }
}