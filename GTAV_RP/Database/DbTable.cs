using System;

namespace GTAV_RP.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    class DbTable : Attribute
    {
        public string name;

        public DbTable(string Name)
        {
            name = Name;
        }
    }
}
