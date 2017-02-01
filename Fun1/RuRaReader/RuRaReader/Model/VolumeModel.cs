using Object = Java.Lang.Object;

namespace RuRaReader.Model
{
    class VolumeModel : Object
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public VolumeModel(dynamic vol)
        {
            Title = vol.nameTitle;
            Id = (int)vol.volumeId;
        }
    }
}