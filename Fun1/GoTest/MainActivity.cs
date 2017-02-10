using Android.App;
using Android.OS;
using Android.Widget;
using Atom;

namespace GoTest
{
    [Activity(Label = "GoTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var cont = (LinearLayout)FindViewById(Resource.Id.Container);
            var dir = FilesDir.AbsolutePath;
            var args = new[] {"Name=Common.LS", "IP=192.168.0.1", "Port=55566"};
            Config.LoadConfig(args, dir,
                connectResult =>
                {
                    RunOnUiThread(() =>
                    {
                        var text = new TextView(this);
                        text.Text = connectResult.ToString();
                        cont.AddView(text);
                    });
                });
        }
    }
}