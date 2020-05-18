using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;
using System.Collections;

using GTANetworkAPI;
using GTANetworkMethods;

namespace GTAV_RP.Character
{
    class Data : IDisposable
    {
        public Database.Character data = null;
        private Player.Data owner;

        public static Dictionary<Data, Player.Data> playerCharacters = new Dictionary<Data, Player.Data>();

        public Data(Player.Data Owner, Database.Character charData)
        {
            data = charData;
            owner = Owner;

            playerCharacters.Add(this, Owner);
        }

        public bool Save()
        {
            Debug.Assert(data != null);

            if (data.id == Database.Character.INVALID_ID)
            {
                // Ensure that owner id is set.
                data.gid = (long)owner.AccountId;

                return Database.Context.Instance.createCharacter(ref data);
            }

            //UpdateState();
            return Database.Context.Instance.updateCharacter(data);
        }

        public void Dispose()
        {

        }
    }
}
