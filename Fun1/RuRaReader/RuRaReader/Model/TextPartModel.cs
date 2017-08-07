using System.Collections.Generic;
using Object = Java.Lang.Object;

namespace RuRaReader.Model
{
    public class TextPartModel : Object
    {
        public List<RowModel> Lines { get; } = new List<RowModel>();

        public TextPartModel(dynamic data)
        {
            foreach (var row in data)
            {
                Lines.Add(row);
            }
        }
    }
}