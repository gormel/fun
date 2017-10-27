using System.Collections.Generic;
using Java.Lang;
using Exception = System.Exception;

namespace RuRaReader.Model
{
    public class TextModel : Object
    {
        public List<TextPartModel> Text { get; } = new List<TextPartModel>();

        public ChapterModel Chapter { get; set; }

        public TextModel(dynamic data)
        {
            foreach (var pt in data)
            {
                 Text.Add(new TextPartModel(pt));
            }
        }
    }
}