using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            var orders = SaveDataManager.Instance.ProjectOrders;
            foreach (var model in await SaveDataManager.Instance.GetProjects())
            {
                if (!SaveDataManager.Instance.ProjectOrders.ContainsKey(model.Id))
                    SaveDataManager.Instance.ProjectOrders[model.Id] = 0;
                
                var index = BinarySearch(mProjectsCollection, model);
                mProjectsCollection.Insert(index, model);
            }
        }

        private int BinarySearch(ObservableCollection<ProjectModel> collection, ProjectModel model)
        {
            var a = 0;
            var b = collection.Count;
            var mValue = SaveDataManager.Instance.ProjectOrders[model.Id];
            while (a != b)
            {
                var c = (a + b) / 2;
                var cValue = SaveDataManager.Instance.ProjectOrders[collection[c].Id];
                if (mValue < cValue)
                    a = c;
                else
                    b = c;
            }
            return a;
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
            SaveDataManager.Instance.TopProject(model.Id);
            SaveDataManager.Instance.Save(FilesDir.AbsolutePath);
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

