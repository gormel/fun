using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
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
            if (mTextModel == null)
                return;

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

            if (mTextModel == null || mTextModel.Text.Count < 1)
            {
                var tv = new TextView(this);
                tv.Text = "Здесь текста нет!";
                tv.TextSize += 2f;
                ContentContainer.AddView(tv);
            }
            else
            {
                HttpClient imageLoader = new HttpClient();
                bool actionBarTitleSet = false;
                foreach (var part in mTextModel.Text)
                {
                    if (part.Lines.Count < 1)
                        continue;
                    foreach (var line in part.Lines)
                    {
                        if (line is HeaderRowModel)
                        {
                            var headerTv = new TextView(this);
                            headerTv.Text = Environment.NewLine + ((HeaderRowModel)line).Text + Environment.NewLine;
                            headerTv.TextSize += 2f;
                            headerTv.Gravity = GravityFlags.Center;
                            headerTv.Tag = part;
                            ContentContainer.AddView(headerTv);

                            if (!actionBarTitleSet)
                            {
                                ActionBar.Title = ((HeaderRowModel) line).Text;
                                actionBarTitleSet = true;
                            }
                        }
                        if (line is TextRowModel)
                        {
                            var textTv = new TextView(this);
                            textTv.Text = ((TextRowModel)line).Text;
                            textTv.Tag = part;
                            ContentContainer.AddView(textTv);
                        }

                        if (line is ImageRowModel)
                        {
                            var lineIv = new ImageView(this);
                            lineIv.SetScaleType(ImageView.ScaleType.FitXy);
                            lineIv.SetAdjustViewBounds(true);
                            var url = ((ImageRowModel) line).Url;
                            imageLoader.GetAsync(url).ContinueWith(t =>
                            {
                                t.Result.Content.ReadAsStreamAsync().ContinueWith(t1 =>
                                {
                                    RunOnUiThread(() =>
                                    {
                                        lineIv.SetImageBitmap(BitmapFactory.DecodeStream(t1.Result));
                                    });
                                });
                            });
                            ContentContainer.AddView(lineIv);
                        }
                    }
                }
            }


            var nextBtn = new Button(this);
            nextBtn.Gravity = GravityFlags.Center;
            nextBtn.Text = "Next";
            nextBtn.Click += NextBtnOnClick;
            mainContaimer.AddView(nextBtn);
        }

        private void NextBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (mTextModel == null)
            {
                GoBack();
                return;
            }

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
            if (mTextModel == null || mTextModel.Chapter.PrevModel == null)
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
            if (mTextModel == null)
                return;

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
            var id = -1;
            if (mTextModel != null)
                id = mTextModel.Chapter.Volume.Id;
            toStart.PutExtra("Id", id);
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