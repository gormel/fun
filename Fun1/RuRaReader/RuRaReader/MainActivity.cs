using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RuRaReader.Model;
using RuRaReader.Model.Bindings;
using SuperJson;

namespace RuRaReader
{
    [Activity(Label = "RuRaReader", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly ObservableCollection<ProjectModel> mProjectsCollection = new ObservableCollection<ProjectModel>(); 
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            
            var projects = new CollectionBinding<ProjectModel>(mProjectsCollection, (LinearLayout)FindViewById(Resource.Id.Projects), ApplyProjectTemplate);

            var client = new HttpClient();
            var result = await client.GetAsync("http://ruranobe.ru/api/projects");
            var textResult = await result.Content.ReadAsStringAsync();
            var ser = new SuperJsonSerializer();
            dynamic res = ser.Deserialize(textResult);

            mProjectsCollection.Clear();
            foreach (var project in res)
            {
                mProjectsCollection.Add(new ProjectModel(project));
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
            StartActivity(toStart);
        }
    }
}

