using System;

namespace GTAV_RP.Database
{
    [AttributeUsage(AttributeTargets.Field)]
    class DbField : Attribute
    {
        public string name;
        public bool isUpdateKey;
        public bool skipUpdate;

        public DbField(string Name, bool IsUpdateKey = false, bool SkipUpdate = false)
        {
            name = Name;
            isUpdateKey = IsUpdateKey;
            skipUpdate = SkipUpdate;
        }
    }
}
