using System.Collections.Generic;
using Object = Java.Lang.Object;

namespace RuRaReader.Model
{
    public class TextPartModel : Object
    {
        public string Header { get; set; }
        public List<string> Lines { get; } = new List<string>();

        public TextPartModel(dynamic data)
        {
            Header = data[0];
            bool firstLine = true;
            foreach (var row in data)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }
                Lines.Add(row);
            }
        }
    }
}