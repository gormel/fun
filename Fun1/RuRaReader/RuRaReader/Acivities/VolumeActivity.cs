using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
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
            foreach (var chapter in await SaveDataManager.Instance.GetChapters(id))
            {
                mChaptersCollection.Add(chapter);
            }
            if (!mChaptersCollection.Any())
            {
                var info = new TextView(this);
                info.Gravity = GravityFlags.Center;
                info.TextSize = 18;
                info.Text = "Глав нет =(";
                ContentContainer.AddView(info);
            }
            if (mVolume == null)
                return;
            ActionBar.Title = mVolume.Title;
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
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var toStart = new Intent(this, typeof(ProjectActivity));
                var id = -1;
                if (mVolume != null)
                    id = mVolume.Project.Id;
                toStart.PutExtra("Id", id);
                toStart.SetFlags(ActivityFlags.ClearTop);
                StartActivity(toStart);
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}