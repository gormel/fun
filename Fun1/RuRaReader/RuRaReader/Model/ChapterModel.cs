using Object = Java.Lang.Object;

namespace RuRaReader.Model
{
    public class ChapterModel : Object
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public ChapterModel PrevModel { get; set; }
        public ChapterModel NextModel { get; set; }

        public VolumeModel Volume { get; set; }

        public ChapterModel(dynamic ch)
        {
            Title = ch.chapterTitle;
            Url = ch.url;
            Id = ch.chapterId;
        }
    }
}