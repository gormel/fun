using System;
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
            var args = new[] {"Name=Common.LS", "IP=192.168.110.166", "Port=55777"};
            Config.LoadConfig(args, dir,
                connectResult =>
                {
                    RunOnUiThread(() =>
                    {
                        var text = new TextView(this);
                        text.Text = string.Join("\n\r", "Read config: " + connectResult,
                            Config.Ip, Config.Name, Config.Port, Config.Version);

                        cont.AddView(text);

                        var button = new Button(this);
                        button.Text = "Go go go!";
                        button.Click += ButtonOnClick;
                        cont.AddView(button);
                    });
                });
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(DbcActivity));
        }
    }
}