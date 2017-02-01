using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using RuRaReader.Model;
using RuRaReader.Model.Bindings;
using SuperJson;

namespace RuRaReader
{
    [Activity(Label = "Project")]
    public class ProjectActivity : Activity
    {
        private ObservableCollection<VolumeModel> mVolumesCollection = new ObservableCollection<VolumeModel>(); 

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var id = Intent.GetIntExtra("Id", -1);

            SetContentView(Resource.Layout.ProjectLayout);

            var projects = new CollectionBinding<VolumeModel>(mVolumesCollection, (LinearLayout)FindViewById(Resource.Id.Volumes), ApplyTemplate);

            var client = new HttpClient();
            var result = await client.GetAsync($"http://ruranobe.ru/api/projects/{id}/volumes");
            var textResult = await result.Content.ReadAsStringAsync();
            var ser = new SuperJsonSerializer();
            dynamic res = ser.Deserialize(textResult);

            mVolumesCollection.Clear();
            foreach (var vol in res)
            {
                mVolumesCollection.Add(new VolumeModel(vol));
            }
        }

        private View ApplyTemplate(VolumeModel volumeModel)
        {
            var result = new Button(this);
            result.Text = volumeModel.Title;
            return result;
        }
    }
}