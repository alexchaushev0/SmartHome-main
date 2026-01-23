using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BL.Interfaces
{
    
    public interface ISmartHomeManager
    {
        Task<string> GetRoomStatus(string roomId);
    }
}
