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

            var textScroller = new ScrollView(this);

            mTextContainer = new TextView(this);

            textScroller.AddView(mTextContainer);
            ContentContainer.AddView(textScroller);

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
            StartActivity(toStart);
        }

        private void PrevBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (mTextModel.Chapter.PrevModel == null)
                return;
            
            var toStart = new Intent(this, typeof(TextActivity1));
            toStart.PutExtra("Id", mTextModel.Chapter.PrevModel.Id);
            StartActivity(toStart);
        }

        protected override void OnScrollChanged(View.ScrollChangeEventArgs scrollChangeEventArgs)
        {
            var delta = scrollChangeEventArgs.OldScrollY - scrollChangeEventArgs.ScrollY;
        }
    }
}