using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace RuRaReader.Acivities
{
    public abstract class BaseActivity : Activity
    {
        protected LinearLayout ContentContainer { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Main);

            var loading = FindViewById(Resource.Id.Loading);
            loading.Visibility = ViewStates.Visible;

            ContentContainer = (LinearLayout)FindViewById(Resource.Id.Container);

            var scroller = (ScrollView)FindViewById(Resource.Id.Scroller);
            scroller.ScrollChange += ScrollerOnScrollChange;

            StartLoad().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Log.Error("ex", t.Exception.ToString());
                RunOnUiThread(() =>
                {
                    loading.Visibility = ViewStates.Gone;
                });
            });
        }

        private void ScrollerOnScrollChange(object sender, View.ScrollChangeEventArgs scrollChangeEventArgs)
        {
            OnScrollChanged(scrollChangeEventArgs);
        }

        protected abstract Task StartLoad();

        protected virtual void OnScrollChanged(View.ScrollChangeEventArgs scrollChangeEventArgs) { }
    }
}