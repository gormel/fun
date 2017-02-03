using Java.Lang;

namespace RuRaReader.Model
{
    public class TextModel : Object
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public ChapterModel Chapter { get; set; }

        public TextModel(dynamic data)
        {
            Title = data.textTitle;
            Text = data.text;
        }
    }
}