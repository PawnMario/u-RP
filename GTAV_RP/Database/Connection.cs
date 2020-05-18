using System;

namespace GTAV_RP.Database
{
    class Connection
    {
        public String fileName { get; }
        public String host { get; set; }
        public String user { get; set; }
        public String password { get; set; }
        public String database { get; set; }

        public Connection()
        {
            this.fileName = "MySQL.json";

            this.host = "127.0.0.1";
            this.user = "root";
            this.password = "";
            this.database = "gtavrp";
        }
    }
}
