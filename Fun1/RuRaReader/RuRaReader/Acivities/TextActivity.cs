using System;
using System.Linq;
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
        private TextModel mTextModel;
        private int mCollectedDelta;

        protected override async Task StartLoad()
        {
            var id = Intent.GetIntExtra("Id", -1);

            mTextModel = await SaveDataManager.Instance.GetText(id);
            ConstructInterface();

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
            var mainContaimer = (LinearLayout)FindViewById(Resource.Id.BaseContainer);

            ContentContainer.RemoveAllViews();

            var prevBtn = new Button(this);
            prevBtn.Gravity = GravityFlags.Center;
            prevBtn.Text = "Previous";
            prevBtn.TextAlignment = TextAlignment.Gravity;
            prevBtn.Click += PrevBtnOnClick;
            mainContaimer.AddView(prevBtn, 0);

            if (mTextModel.Text.Count < 1)
            {
                var tv = new TextView(this);
                tv.Text = "Здесь текста нет!";
                tv.TextSize *= 1.5f;
                ContentContainer.AddView(tv);
            }

            foreach (var part in mTextModel.Text)
            {
                if (part.Lines.Count < 1)
                    continue;
                var headerTv = new TextView(this);
                headerTv.Text = $"{Environment.NewLine}{part.Header}{Environment.NewLine}";
                headerTv.TextSize *= 1.5f;
                headerTv.Gravity = GravityFlags.Center;
                headerTv.Tag = part;
                ContentContainer.AddView(headerTv);
                var textTv = new TextView(this);
                textTv.Text = $"{string.Join($"{Environment.NewLine}\t", part.Lines)}";
                textTv.Tag = part;
                textTv.Click += TextTvOnClick;
                ContentContainer.AddView(textTv);
            }

            if (mTextModel.Text.Count > 0)
            {
                ActionBar.Title = mTextModel.Text[0].Header;
            }

            var nextBtn = new Button(this);
            nextBtn.Gravity = GravityFlags.Center;
            nextBtn.Text = "Next";
            nextBtn.Click += NextBtnOnClick;
            mainContaimer.AddView(nextBtn);
        }

        private void TextTvOnClick(object sender, EventArgs eventArgs)
        {
            var typedSender = (View)sender;
            var typedTag = (TextPartModel)typedSender.Tag;
            ActionBar.Title = typedTag.Header;
        }

        private void NextBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (!SaveDataManager.Instance.ReadChapterUrls.Contains(mTextModel.Chapter.Url))
            {
                SaveDataManager.Instance.ReadChapterUrls.Add(mTextModel.Chapter.Url);
                SaveDataManager.Instance.Save(FilesDir.AbsolutePath);
            }

            if (mTextModel.Chapter.NextModel == null)
            {
                GoBack();
                return;
            }

            var toStart = new Intent(this, typeof(TextActivity1));
            toStart.PutExtra("Id", mTextModel.Chapter.NextModel.Id);
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        private void PrevBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (mTextModel.Chapter.PrevModel == null)
            {
                GoBack();
                return;
            }

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

            var scroller = (ScrollView)FindViewById(Resource.Id.Scroller);
        }

        private void GoBack()
        {
            var toStart = new Intent(this, typeof(VolumeActivity));
            toStart.PutExtra("Id", mTextModel.Chapter.Volume.Id);
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                GoBack();
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}