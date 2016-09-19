using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IRoomServer
    {
        void Move();

        void Rotate(Direction newDiection);

        IEnumerable<IUserInfo> OtherUsers { get; }  

        IUserInfo Me { get; }
    }
}
