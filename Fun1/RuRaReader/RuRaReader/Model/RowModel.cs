using System;
using System.Collections.Generic;

namespace RuRaReader.Model
{
    public class RefferenceModel
    {
        public int StartIndex { get; set; }
        public int Lenght { get; set; }
        public string Text { get; set; }
    }

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
        public List<RefferenceModel> Refferences { get; } = new List<RefferenceModel>(); 
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

    public class SubtitleRowModel : RowModel
    {
        public SubtitleRowModel(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}