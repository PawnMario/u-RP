using System;
using System.Collections.Generic;
using System.Text;

namespace GTAV_RP.Database
{
    [DbTable("rp_characters")]
    public class Character
    {
        public const long INVALID_ID = 0;

        [DbField("id", IsUpdateKey: true)]
        public long id = INVALID_ID;

        [DbField("gid", SkipUpdate: true)]
        public long gid;

        [DbField("name", SkipUpdate: true)]
        public string name;

        [DbField("surname", SkipUpdate: true)]
        public string surname;

        [DbField("appearance")]
        public string appearance;
        /*
        [DbField("skin")]
        public int skin;

        [DbField("birth")]
        public int birth;*/
    }
}
