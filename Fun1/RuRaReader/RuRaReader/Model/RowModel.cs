using System;

namespace RuRaReader.Model
{
    public abstract class RowModel : Object
    {
    }

    public class TextRowModel : RowModel
    {
        public TextRowModel(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }

    public class ImageRowModel : RowModel
    {
        public ImageRowModel(string url)
        {
            Url = url;
        }

        public string Url { get; private set; }
    }

    public class HeaderRowModel : RowModel
    {
        public HeaderRowModel(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}