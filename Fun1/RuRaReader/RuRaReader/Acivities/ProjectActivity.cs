using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using RuRaReader.Model;
using RuRaReader.Model.Bindings;

namespace RuRaReader.Acivities
{
    [Activity(Label = "Project")]
    public class ProjectActivity : BaseActivity
    {
        private ObservableCollection<VolumeModel> mVolumesCollection = new ObservableCollection<VolumeModel>();

        protected override async Task StartLoad()
        {
            var id = Intent.GetIntExtra("Id", -1);
            var projects = new CollectionBinding<VolumeModel>(mVolumesCollection, ContentContainer, ApplyTemplate);

            var proj = await SaveDataManager.Instance.GetProject(id);
            ActionBar.Title = proj.Title;
            mVolumesCollection.Clear();
            foreach (var vol in await SaveDataManager.Instance.GetVolumes(id))
            {
                mVolumesCollection.Add(vol);
            }
        }

        private View ApplyTemplate(VolumeModel volumeModel)
        {
            var result = new Button(this);
            result.Text = volumeModel.Title;
            result.Tag = volumeModel;
            result.Click += ResultOnClick;
            return result;
        }

        private void ResultOnClick(object sender, EventArgs eventArgs)
        {
            var btn = (Button) sender;
            var model = (VolumeModel) btn.Tag;
            var toStart = new Intent(this, typeof(VolumeActivity));
            toStart.PutExtra("Id", model.Id);
            toStart.PutExtra("Url", model.Url);
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var toStart = new Intent(this, typeof(MainActivity));
                toStart.SetFlags(ActivityFlags.ClearTop);
                StartActivity(toStart);
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}