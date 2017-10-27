using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Atom;
using Atom.Db;
using Java.Util;

namespace GoTest
{
    [Activity(Label = "DboActivity")]
    public class DboActivity : Activity
    {
        DbO table;
        LinearLayout mContainer;
        private Dictionary<string, TextView> mTextViews = new Dictionary<string, TextView>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            mContainer = (LinearLayout)FindViewById(Resource.Id.Container);

            table = DbcActivity.Database.SubscribeDbO(Intent.GetStringExtra("table"), connect: Connect, changeField:ChangeField);
        }

        private void ChangeField(DbO dbo, VariantChangeEventArgs args)
        {
            RunOnUiThread(() =>
            {
                if (!mTextViews.ContainsKey(args.Name))
                {
                    mTextViews[args.Name] = new TextView(this);
                    mContainer.AddView(mTextViews[args.Name]);
                }

                mTextViews[args.Name].Text = args.Name + ": " + args.NewValue;
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DbcActivity.Database.DestroyDbO(ref table);
        }

        private SResult Connect(DbO dbo, Variant save)
        {
            return SResult.Success();
        }
    }
}