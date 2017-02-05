using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using RuRaReader.Model;

namespace RuRaReader.Acivities
{
    [Activity]
    public class TextActivity1 : BaseActivity
    {
        private TextView mTextContainer;
        private TextModel mTextModel;
        private int mCollectedDelta;

        protected override async Task StartLoad()
        {
            var id = Intent.GetIntExtra("Id", -1);

            mTextModel = await SaveDataManager.Instance.GetText(id);
            ConstructInterface();

            ActionBar.Title = mTextModel.Title ?? "Text";
            mTextContainer.Text = mTextModel.Text;

            if (SaveDataManager.Instance.ChapterSctolls.ContainsKey(mTextModel.Chapter.Url))
            {
                var scroller = (ScrollView)FindViewById(Resource.Id.Scroller);
                var value = SaveDataManager.Instance.ChapterSctolls[mTextModel.Chapter.Url];
                RunOnUiThread(() =>
                {
                    scroller.Post(() =>
                    {
                        scroller.ScrollTo(0, value);
                    });
                });
            }
        }

        private void ConstructInterface()
        {
            ContentContainer.RemoveAllViews();

            var prevBtn = new Button(this);
            prevBtn.Gravity = GravityFlags.Center;
            prevBtn.Text = "Previous";
            prevBtn.TextAlignment = TextAlignment.Gravity;
            prevBtn.Click += PrevBtnOnClick;
            ContentContainer.AddView(prevBtn);
            
            mTextContainer = new TextView(this);
            ContentContainer.AddView(mTextContainer);

            var nextBtn = new Button(this);
            nextBtn.Gravity = GravityFlags.Center;
            nextBtn.Text = "Next";
            nextBtn.Click += NextBtnOnClick;
            ContentContainer.AddView(nextBtn);
        }

        private void NextBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (mTextModel.Chapter.NextModel == null)
                return;

            if (!SaveDataManager.Instance.ReadChapterUrls.Contains(mTextModel.Chapter.Url))
            {
                SaveDataManager.Instance.ReadChapterUrls.Add(mTextModel.Chapter.Url);
                SaveDataManager.Instance.Save(FilesDir.AbsolutePath);
            }

            var toStart = new Intent(this, typeof(TextActivity1));
            toStart.PutExtra("Id", mTextModel.Chapter.NextModel.Id);
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        private void PrevBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (mTextModel.Chapter.PrevModel == null)
                return;
            
            var toStart = new Intent(this, typeof(TextActivity1));
            toStart.PutExtra("Id", mTextModel.Chapter.PrevModel.Id);
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        protected override void OnScrollChanged(int lastScroll, int newScroll)
        {
            SaveDataManager.Instance.ChapterSctolls[mTextModel.Chapter.Url] = newScroll;
            var delta = Math.Abs(lastScroll - newScroll);
            mCollectedDelta += delta;
            if (mCollectedDelta > 10)
            {
                mCollectedDelta = 0;
                SaveDataManager.Instance.Save(FilesDir.AbsolutePath);
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var toStart = new Intent(this, typeof(VolumeActivity));
                toStart.PutExtra("Id", mTextModel.Chapter.Volume.Id);
                toStart.SetFlags(ActivityFlags.ClearTop);
                StartActivity(toStart);
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}