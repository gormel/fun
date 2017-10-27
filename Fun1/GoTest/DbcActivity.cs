using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Atom;
using Atom.Db;

namespace GoTest
{
    [Activity]
    public class DbcActivity : Activity
    {
        Timer mTimer;
        LinearLayout mContainer;
        public static DbC Database;
        private Dictionary<string, Button> mButtons = new Dictionary<string, Button>(); 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            mContainer = (LinearLayout) FindViewById(Resource.Id.Container);

            Database = new DbC(Config.Ip, Config.Port, Config.Name, 
                connect: Connect,
                tableStatus: TableStatus);
            mTimer = new Timer(Tick, this, 0, 17);
        }

        private void TableStatus(string table, bool status)
        {
            RunOnUiThread(() =>
            {
                if (!mButtons.ContainsKey(table))
                {
                    mButtons[table] = new Button(this);
                    mButtons[table].Text = table;
                    mButtons[table].Click += BtnOnClick;
                    mContainer.AddView(mButtons[table]);
                }
                var btn = mButtons[table];
                btn.Enabled = status;
            });
        }

        private void BtnOnClick(object sender, EventArgs eventArgs)
        {
            var btn = (Button) sender;
            var intent = new Intent(this, typeof(DboActivity));
            intent.PutExtra("table", btn.Text);
            StartActivity(intent);
        }

        private void Tick(object state)
        {
            if (Database == null)
                return;

            Database.Update();
        }

        private void Connect(DbC dbc, bool status)
        {
            RunOnUiThread(() =>
            {
                var text = new TextView(this);
                text.Text = (status ? "Connected" : "Disconnected") + " database!";
                mContainer.AddView(text);
            });
        }
    }
}