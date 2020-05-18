using GTANetworkAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GTAV_RP.Player
{
    class Manager : IDisposable
    {
        public static Manager Instance = null;
        private Dictionary<NetHandle, Data> playersDict = new Dictionary<NetHandle, Data>();

        public Manager() {
            Instance = this;
        }
        public void Dispose()
        {
            foreach (KeyValuePair<NetHandle, Data> kv in playersDict)
            {
                Data player = kv.Value;
                player.Save();
                player.Dispose();
            }
            playersDict.Clear();
        }

        public Data registerPlayer(Client client, Database.User data)
        {
            Data player = new Data(client, data);
            playersDict.Add(client.Handle, player);

            LoadPlayerCharacters(player);
            player.InitAuth(data.id);

            return player;
        }

        public bool unregisterPlayer(NetHandle handle)
        {
            Data player = findPlayerByHandle(handle);
            if (player != null)
            {
                player.Save();
                player.Dispose();
                playersDict.Remove(handle);

                foreach(Character.Data character in Character.Data.playerCharacters.Keys)
                {
                    if(character.data.gid == (int)player.data.id)
                    {
                        character.Dispose();
                        Character.Data.playerCharacters.Remove(character);
                    }
                }
                
            }
            return false;
        }

        public Data findPlayerByHandle(NetHandle handle)
        {
            return playersDict.GetValueOrDefault(handle);
        }

        public Data findPlayerByAccountId(ulong AccountId)
        {
            foreach (Data player in playersDict.Values)
            {
                if (player.data.id == AccountId)
                {
                    return player;
                }
            }
            return null;
        }

        public void LoadPlayerCharacters(Data player)
        {
            ArrayList characters = Database.Context.Instance.getUserCharacters(player.data.id);
            if (characters.Count == 0)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SELECTOR_NEW_CHAR");
                NAPI.Chat.SendChatMessageToPlayer(player, "Na Twoim koncie nie ma żadnej postaci.");
                return;
            }

            foreach (var chars in characters)
            {
                Database.Character data = (Database.Character)chars;
                new Character.Data(player, data);
            }
        }

        public Character.Data getCharacterById(long charId)
        {
            foreach (Character.Data character in Character.Data.playerCharacters.Keys)
            {
                if (character.data.id == charId)
                {
                    return character;
                }
            }
            return null;
        }

        public ArrayList getUserCharacters(Data player)
        {
            ArrayList listCharacters = new ArrayList();
            foreach (Character.Data character in Character.Data.playerCharacters.Keys)
            {
                if (character.data.gid == (int)player.data.id)
                {
                    listCharacters.Add(character);
                }
            }
            return listCharacters;
        }
    }
}
