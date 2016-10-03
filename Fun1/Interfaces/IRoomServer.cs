using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IRoomServer
    {
        void Move();

        void Rotate(Direction newDiection);

        void Fire();

        IEnumerable<IUserInfo> OtherUsers { get; }

        void AddListener(IRoomServerListener listener);

        IUserInfo Me { get; }
    }
}
