﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RuRaReader.Model;
using RuRaReader.Model.Bindings;

namespace RuRaReader.Acivities
{
    [Activity(Label = "RuRaReader", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        private readonly ObservableCollection<ProjectModel> mProjectsCollection = new ObservableCollection<ProjectModel>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (!SaveDataManager.Instance.Loaded)
                SaveDataManager.Instance.Load(FilesDir.AbsolutePath);
            base.OnCreate(savedInstanceState);
        }

        protected override async Task StartLoad()
        {
            var projects = new CollectionBinding<ProjectModel>(mProjectsCollection, ContentContainer, ApplyProjectTemplate);
            
            mProjectsCollection.Clear();
            foreach (var model in await SaveDataManager.Instance.GetProjects())
            {
                mProjectsCollection.Add(model);
            }
        }

        private View ApplyProjectTemplate(ProjectModel s)
        {
            var result = new Button(this);
            result.Text = s.Title;
            result.Tag = s;
            result.Click += ResultOnClick;
            return result;
        }

        private void ResultOnClick(object sender, EventArgs eventArgs)
        {
            var btn = (Button) sender;
            var model = (ProjectModel) btn.Tag;
            var toStart = new Intent(this, typeof(ProjectActivity));
            toStart.PutExtra("Id", model.Id);
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var dialogBuilder = new AlertDialog.Builder(this);
                dialogBuilder
                    .SetMessage("Real quit?")
                    .SetTitle("Alert")
                    .SetCancelable(true)
                    .SetPositiveButton("Yes", (sender, args) => { Finish(); })
                    .SetNegativeButton("No", (sender, args) => { })
                    .Create().Show();
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}

