using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using HtmlAgilityPack;
using RuRaReader.Model;
using RuRaReader.Model.Bindings;

namespace RuRaReader.Acivities
{
    [Activity(Label = "Volume")]
    public class VolumeActivity : BaseActivity
    {
        private readonly ObservableCollection<ChapterModel> mChaptersCollection = new ObservableCollection<ChapterModel>();
        private VolumeModel mVolume;

        protected override async Task StartLoad()
        {
            var id = Intent.GetIntExtra("Id", -1);

            var projects = new CollectionBinding<ChapterModel>(mChaptersCollection, (LinearLayout)FindViewById(Resource.Id.Container), ApplyTemplate);

            mVolume = await SaveDataManager.Instance.GetVolume(id);
            ActionBar.Title = mVolume.Title;
            foreach (var chapter in await SaveDataManager.Instance.GetChapters(id))
            {
                mChaptersCollection.Add(chapter);
            }
        }

        private View ApplyTemplate(ChapterModel chapterModel)
        {
            var result = new Button(this);
            result.Text = chapterModel.Title;
            result.Tag = chapterModel;
            if (SaveDataManager.Instance.ReadChapterUrls.Contains(chapterModel.Url))
            {
                result.SetTextColor(Color.Green);
            }
            result.Click += ResultOnClick;
            return result;
        }

        private void ResultOnClick(object sender, EventArgs eventArgs)
        {
            var btn = (Button) sender;
            var model = (ChapterModel) btn.Tag;

            var toStart = new Intent(this, typeof (TextActivity1));
            toStart.PutExtra("Id", model.Id);
            StartActivity(toStart);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var toStart = new Intent(this, typeof(ProjectActivity));
                toStart.PutExtra("Id", mVolume.Project.Id);
                StartActivity(toStart);
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}