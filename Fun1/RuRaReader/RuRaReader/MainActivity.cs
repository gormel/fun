using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using RuRaReader.Model;
using RuRaReader.Model.Bindings;
using SuperJson;
using SuperJson.Objects;
using SuperJson.Parser;
using String = Java.Lang.String;

namespace RuRaReader
{
    [Activity(Label = "RuRaReader", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly ObservableCollection<String> mProjectsCollection = new ObservableCollection<String>(); 
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            
            var projects = new CollectionBinding<String>(mProjectsCollection, (LinearLayout)FindViewById(Resource.Id.Projects), ApplyProjectTemplate);

            var client = new HttpClient();
            var result = await client.GetAsync("http://ruranobe.ru/api/projects");
            var textResult = await result.Content.ReadAsStringAsync();
            var ser = new SuperJsonSerializer();
            dynamic res = ser.Deserialize(textResult);

            mProjectsCollection.Clear();
            foreach (var project in res)
            {
                mProjectsCollection.Add(new String(project.title));
            }

        }

        private View ApplyProjectTemplate(String s)
        {
            var result = new Button(this);
            result.Text = s.ToString();
            return result;
        }
    }
}

