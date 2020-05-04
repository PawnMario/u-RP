using System;
using GTANetworkAPI;

using uRP.Database;
using uRP.Database.Model;
using uRP.Database.Repository;

namespace uRP
{
    public class Core : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            using(var context = new Context())
            {
                context.Database.EnsureCreated();
            }
        }

    }
}
