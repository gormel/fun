using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace RuRaReader.Acivities
{
    public abstract class BaseActivity : Activity, ViewTreeObserver.IOnScrollChangedListener
    {
        protected LinearLayout ContentContainer { get; private set; }
        private ScrollView mScroller;
        private int mLastScroll;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Main);

            var loading = FindViewById(Resource.Id.Loading);
            loading.Visibility = ViewStates.Visible;

            try
            {
                ContentContainer = (LinearLayout)FindViewById(Resource.Id.Container);

                mScroller = (ScrollView)FindViewById(Resource.Id.Scroller);
                mScroller.ViewTreeObserver.AddOnScrollChangedListener(this);

                StartLoad().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        Log.Error("RuRaReader", t.Exception.ToString());
                    RunOnUiThread(() =>
                    {
                        loading.Visibility = ViewStates.Gone;
                    });
                });
            }
            catch (Exception e)
            {
                Log.Error("RuRaReader", e.ToString());
                throw;
            }
        }

        protected abstract Task StartLoad();

        protected virtual void OnScrollChanged(int oldScroll, int newScroll) { }
        public void OnScrollChanged()
        {
            OnScrollChanged(mLastScroll, mScroller.ScrollY);
            mLastScroll = mScroller.ScrollY;
        }
    }
}