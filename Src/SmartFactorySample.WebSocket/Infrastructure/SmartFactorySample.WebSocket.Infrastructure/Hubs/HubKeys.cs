using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Infrastructure.Hubs
{
    public class HubKeys
    {
        public static string GenerateSensorGroupName(int id)
        {
            return $"TagInfoLiveData-{id}";

        }
    }
}
