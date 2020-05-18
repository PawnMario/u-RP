using System;
using System.Collections.Generic;
using System.Text;

namespace GTAV_RP.Database
{
    [DbTable("core_members")]
    public class User
    {
        public const ulong INVALID_ID = 0;

        [DbField("member_id", IsUpdateKey: true)]
        public ulong id = INVALID_ID;

        [DbField("name", SkipUpdate: true)]
        public string name;

        [DbField("members_pass_hash")]
        public string password;
    }
}
