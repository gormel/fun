using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRoomServerListener
    {
        void UserUpdated(IUserInfo user);

        void LaserFiered(int x, int y, Direction direction, int lenght);
    }
}
