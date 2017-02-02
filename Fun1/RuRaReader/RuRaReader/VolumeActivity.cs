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
    [Activity(Label = "Volume")]
    public class VolumeActivity : Activity
    {
        private ObservableCollection<ChapterModel> mChaptersCollection = new ObservableCollection<ChapterModel>(); 

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var id = Intent.GetIntExtra("Id", -1);
            SetContentView(Resource.Layout.Main);

            var projects = new CollectionBinding<ChapterModel>(mChaptersCollection, (LinearLayout)FindViewById(Resource.Id.Container), ApplyTemplate);

            var client = new HttpClient();
            var result = await client.GetAsync($"http://ruranobe.ru/api/projects/{id}/volumes");
            var textResult = await result.Content.ReadAsStringAsync();
            var ser = new SuperJsonSerializer();
            dynamic res = ser.Deserialize(textResult);

            mChaptersCollection.Clear();
            foreach (var vol in res)
            {
                mChaptersCollection.Add(new ChapterModel(vol));
            };
        }

        private View ApplyTemplate(ChapterModel chapterModel)
        {
            return null;
        }
    }
}