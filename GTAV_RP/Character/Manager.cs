using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GTANetworkAPI;

namespace GTAV_RP.Character
{
    class Manager : IDisposable
    {
        public static Manager Instance = null;

        public Manager()
        {
            Instance = this;
        }

        public void Dispose()
        {

        }
    }
}
