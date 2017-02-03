using Object = Java.Lang.Object;

namespace RuRaReader.Model
{
    public class VolumeModel : Object
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public ProjectModel Project { get; set; }

        public VolumeModel(dynamic vol)
        {
            Title = vol.nameTitle;
            Id = (int)vol.volumeId;
            Url = vol.url;
        }
    }
}