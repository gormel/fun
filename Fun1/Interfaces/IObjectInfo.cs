using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public enum ObjectType
    {
        Wall,
        
    }

    public interface IObjectInfo
    {
        int X { get; }
        int Y { get; }
        ObjectType ObjectType { get; }
    }
}
